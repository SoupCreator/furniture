using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using myApp.Models;

namespace myApp.Services;

public partial class CartService : ObservableObject
{
    public ObservableCollection<CartItem> Items { get; } = new();
    

    public double TotalAmount => Items.Sum(i => i.TotalPrice) ;

    public CartService()
    {
        Items.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalAmount));
        
    }

    public void AddToCart(Product product)
    {
        var existingItem = Items.FirstOrDefault(i => i.Product.ProductId == product.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            var newItem = new CartItem(product);
            newItem.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CartItem.TotalPrice))
                {
                    OnPropertyChanged(nameof(TotalAmount));
                }
            };
            Items.Add(newItem);
        }
        OnPropertyChanged(nameof(TotalAmount));
    }
    

    public void IncrementQuantity(CartItem item)
    {
        item.Quantity++;
        OnPropertyChanged(nameof(TotalAmount));
    }

    public void DecrementQuantity(CartItem item)
    {
        if (item.Quantity > 1)
        {
            item.Quantity--;
        }
        else
        {
            Items.Remove(item);
        }
        OnPropertyChanged(nameof(TotalAmount));
    }
    
    public void RemoveItem(CartItem item)
    {
        Items.Remove(item);
        OnPropertyChanged(nameof(TotalAmount));
    }
    
    public void ClearCart()
    {
        Items.Clear();
        OnPropertyChanged(nameof(TotalAmount));
    }
}
