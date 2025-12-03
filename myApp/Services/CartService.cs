using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using myApp.Data;
using myApp.Models;

namespace myApp.Services;

    public partial class CartService : ObservableObject
    {
        public ObservableCollection<CartItem> Items { get; } = new();
        public ObservableCollection<ServiceItem> Services { get; } = new();

        public double ServicesTotal => Services.Where(s => s.IsSelected).Sum(s => s.Service.ServicePrice);
        public double TotalAmount => Items.Sum(i => i.TotalPrice) + ServicesTotal;

        public CartService()
        {
            Items.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalAmount));
            Services.CollectionChanged += (s, e) => 
            {
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(ServicesTotal));
            };
            _ = LoadItemsFromDb();
            _ = LoadServicesFromDb();
        }

        private async Task LoadItemsFromDb()
        {
            try
            {
                using var context = new AppDbContext();
                var woId = await GetCurrentWorkOrderId(context);
                
                var cartProducts = await context.ProductsInCart
                    .Where(p => p.woID == woId)
                    .ToListAsync();

                // Get all product IDs to fetch details
                var productIds = cartProducts.Select(p => p.productID).ToList();
                var products = await context.Products
                    .Where(p => productIds.Contains(p.ProductId))
                    .ToDictionaryAsync(p => p.ProductId);

                // Update UI on UI thread if needed, but ObservableCollection usually handles it if bound correctly.
                // However, since we are in async method, we should be careful. 
                // For Avalonia/WPF, usually need Dispatcher if not on UI thread. 
                // Assuming this is called from UI thread initially or we use proper threading.
                // For simplicity, we will clear and add.
                
                // We need to sync Items with DB data
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    Items.Clear();
                    foreach (var cp in cartProducts)
                    {
                        if (products.TryGetValue(cp.productID, out var product))
                        {
                            var item = new CartItem(product, cp.productQuantity);
                            // Subscribe to property changes for total calculation
                            item.PropertyChanged += (s, e) =>
                            {
                                if (e.PropertyName == nameof(CartItem.TotalPrice))
                                {
                                    OnPropertyChanged(nameof(TotalAmount));
                                }
                            };
                            Items.Add(item);
                        }
                    }
                    OnPropertyChanged(nameof(TotalAmount));
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cart items: {ex.Message}");
            }
        }

        private async Task<int> GetCurrentWorkOrderId(AppDbContext context)
        {
            // Logic: Find the latest open work order or create a new one.
            // For now, let's assume we want the latest one.
            // If no work orders exist, create one.
            
            var latestOrder = await context.WorkOrders
                .OrderByDescending(w => w.WorkOrderId)
                .FirstOrDefaultAsync();

            if (latestOrder == null)
            {
                // Create new WorkOrder
                var newOrder = new WorkOrder
                {
                    Date = DateTime.Now,
                    // Default values for now
                    Price = 0,
                    ClientId = 1, // Placeholder
                    SellerId = 1  // Placeholder
                };
                context.WorkOrders.Add(newOrder);
                await context.SaveChangesAsync();
                return newOrder.WorkOrderId;
            }

            return latestOrder.WorkOrderId;
        }

        public async void AddToCart(Product product)
        {
            try
            {
                using var context = new AppDbContext();
                var woId = await GetCurrentWorkOrderId(context);

                var existingItem = await context.ProductsInCart
                    .FirstOrDefaultAsync(p => p.woID == woId && p.productID == product.ProductId);

                if (existingItem != null)
                {
                    existingItem.productQuantity++;
                }
                else
                {
                    context.ProductsInCart.Add(new ProductInCart
                    {
                        woID = woId,
                        productID = product.ProductId,
                        productQuantity = 1
                    });
                }

                await context.SaveChangesAsync();
                await LoadItemsFromDb(); // Reload to sync UI
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding to cart: {ex.Message}");
            }
        }

        public async void IncrementQuantity(CartItem item)
        {
            try
            {
                using var context = new AppDbContext();
                var woId = await GetCurrentWorkOrderId(context);
                var dbItem = await context.ProductsInCart
                    .FirstOrDefaultAsync(p => p.woID == woId && p.productID == item.Product.ProductId);

                if (dbItem != null)
                {
                    dbItem.productQuantity++;
                    await context.SaveChangesAsync();
                    await LoadItemsFromDb();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error incrementing quantity: {ex.Message}");
            }
        }

        public async void DecrementQuantity(CartItem item)
        {
            try
            {
                using var context = new AppDbContext();
                var woId = await GetCurrentWorkOrderId(context);
                var dbItem = await context.ProductsInCart
                    .FirstOrDefaultAsync(p => p.woID == woId && p.productID == item.Product.ProductId);

                if (dbItem != null)
                {
                    if (dbItem.productQuantity > 1)
                    {
                        dbItem.productQuantity--;
                    }
                    else
                    {
                        context.ProductsInCart.Remove(dbItem);
                    }
                    await context.SaveChangesAsync();
                    await LoadItemsFromDb();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decrementing quantity: {ex.Message}");
            }
        }

        public async void RemoveItem(CartItem item)
        {
            try
            {
                using var context = new AppDbContext();
                var woId = await GetCurrentWorkOrderId(context);
                var dbItem = await context.ProductsInCart
                    .FirstOrDefaultAsync(p => p.woID == woId && p.productID == item.Product.ProductId);

                if (dbItem != null)
                {
                    context.ProductsInCart.Remove(dbItem);
                    await context.SaveChangesAsync();
                    await LoadItemsFromDb();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item: {ex.Message}");
            }
        }

        public async void ClearCart()
        {
            try
            {
                using var context = new AppDbContext();
                var woId = await GetCurrentWorkOrderId(context);
                var items = await context.ProductsInCart.Where(p => p.woID == woId).ToListAsync();
                
                context.ProductsInCart.RemoveRange(items);
                await context.SaveChangesAsync();
                await LoadItemsFromDb();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing cart: {ex.Message}");
            }
        }

        public async void CheckoutWorkOrder()
        {
            try
            {
                using var context = new AppDbContext();
                var woId = await GetCurrentWorkOrderId(context);
                await context.SaveChangesAsync();
                await LoadItemsFromDb();
                // Create new WorkOrder
                var newOrder = new WorkOrder
                {
                    Date = DateTime.Now,
                    // Default values for now
                    Price = 0,
                    ClientId = 1, // Placeholder
                    SellerId = 1  // Placeholder
                };
                context.WorkOrders.Add(newOrder);
                await context.SaveChangesAsync();
                await LoadItemsFromDb();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка создания нвоого заказ наряда или сохранения текущего: {ex.Message}");
            }
        }
        private async Task LoadServicesFromDb()
        {
            try
            {
                using var context = new AppDbContext();
                var woId = await GetCurrentWorkOrderId(context);

                var allServices = await context.Services.ToListAsync();
                var servicesInCart = await context.ServicesInCart
                    .Where(s => s.woID == woId)
                    .ToListAsync();

                var selectedServiceIds = servicesInCart.Select(s => s.serviceID).ToHashSet();

                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    Services.Clear();
                    foreach (var service in allServices)
                    {
                        var isSelected = selectedServiceIds.Contains(service.ServiceId);
                        var serviceItem = new ServiceItem(service, isSelected);
                        
                        serviceItem.PropertyChanged += (s, e) =>
                        {
                            if (e.PropertyName == nameof(ServiceItem.IsSelected))
                            {
                                OnPropertyChanged(nameof(TotalAmount));
                                OnPropertyChanged(nameof(ServicesTotal));
                                // Trigger DB update if needed, but usually better to do it via command
                            }
                        };
                        
                        Services.Add(serviceItem);
                    }
                    OnPropertyChanged(nameof(TotalAmount));
                });
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Error loading services: {ex.Message}");
            }
        }

        public async void ToggleService(ServiceItem item)
        {
            try
            {
                using var context = new AppDbContext();
                var woId = await GetCurrentWorkOrderId(context);
                
                var dbItem = await context.ServicesInCart
                    .FirstOrDefaultAsync(s => s.woID == woId && s.serviceID == item.Service.ServiceId);

                item.IsSelected = !item.IsSelected;

                if (item.IsSelected)
                {
                     if (dbItem == null)
                     {
                         context.ServicesInCart.Add(new ServicesInCart
                         {
                             woID = woId,
                             serviceID = item.Service.ServiceId,
                             serviceQuantity = 1
                         });
                     }
                }
                else
                {
                    if (dbItem != null)
                    {
                        context.ServicesInCart.Remove(dbItem);
                    }
                }

                await context.SaveChangesAsync();
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(ServicesTotal));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling service: {ex.Message}");
                item.IsSelected = !item.IsSelected; 
            }
        }
    }

