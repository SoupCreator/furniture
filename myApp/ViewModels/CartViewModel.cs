using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using myApp.Data;
using myApp.Models;
using myApp.Services;

namespace myApp.ViewModels;

public partial class CartViewModel : ViewModelBase
{
    [ObservableProperty] 
    private  CartService _cartService;

    public readonly CategoryViewModel _categoryPage;
    
    public ObservableCollection<CartItem> Items => _cartService.Items;
    public ObservableCollection<ServiceItem> Services => _cartService.Services;
    public double TotalAmount => _cartService.TotalAmount;
    public double ServicesTotal => _cartService.ServicesTotal;

    public CartViewModel()
    {
        
    }
    public CartViewModel(CartService cartService)
    {
        _categoryPage = new CategoryViewModel(cartService);
        _cartService = cartService;
        _cartService.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(CartService.TotalAmount))
            {
                OnPropertyChanged(nameof(TotalAmount));
            }
            if (e.PropertyName == nameof(CartService.ServicesTotal))
            {
                OnPropertyChanged(nameof(ServicesTotal));
            }
        };
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
    
    [RelayCommand]
    private void SaveWorkOrder()
    {
        _cartService.CheckoutWorkOrder();
    }
    
    [RelayCommand]
    private void GoToCategoryPage()
    {
        WeakReferenceMessenger.Default.Send(new NavigateMessage(_categoryPage));
    }

    [RelayCommand]
    private void ToggleService(ServiceItem item)
    {
        _cartService.ToggleService(item);
    }

    
}