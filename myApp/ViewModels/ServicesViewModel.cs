using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using myApp.Models;
using myApp.Services;
using System;
using Avalonia.Svg.Skia;

namespace myApp.ViewModels;

public partial class ServicesViewModel : ViewModelBase
{
    private readonly CartService _cartService;

    public ObservableCollection<ServiceItem> Services => _cartService?.Services;

    public ServicesViewModel()
    {
        // Design-time constructor
    }

    public ServicesViewModel(CartService cartService)
    {
        _cartService = cartService;
    }

    [RelayCommand]
    private void ToggleService(ServiceItem item)
    {
        _cartService?.ToggleService(item);
    }
}