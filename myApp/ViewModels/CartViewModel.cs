using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using myApp.Data;
using myApp.Models;
using myApp.Services;

namespace myApp.ViewModels;

public partial class CartViewModel : ViewModelBase
{
    private readonly CartService _cartService;

    public ObservableCollection<CartItem> Items => _cartService.Items;
    

    public double TotalAmount => _cartService.TotalAmount;

    public CartViewModel()
    {
        
    }
    public CartViewModel(CartService cartService)
    {
        _cartService = cartService;
        _cartService.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(CartService.TotalAmount))
            {
                OnPropertyChanged(nameof(TotalAmount));
            }
        };
        
        // Pre-load some items if cart is empty, just to show "values from DB"
        if (!_cartService.Items.Any())
        {
             _ = LoadInitialData();
        }
    }

    private async Task LoadInitialData()
    {
        try
        {
            using (var context = new AppDbContext())
            {
                var products = await context.Products.Take(2).ToListAsync();
                foreach (var product in products)
                {
                    _cartService.AddToCart(product);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading initial cart data: {ex.Message}");
        }
    }

    [RelayCommand]
    private void IncrementQuantity(CartItem item)
    {
        _cartService.IncrementQuantity(item);
    }

    [RelayCommand]
    private void DecrementQuantity(CartItem item)
    {
        _cartService.DecrementQuantity(item);
    }

    [RelayCommand]
    private void RemoveItem(CartItem item)
    {
        _cartService.RemoveItem(item);
    }
}