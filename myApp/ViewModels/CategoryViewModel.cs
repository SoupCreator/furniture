using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using myApp.Services;

namespace myApp.ViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ViewModelBase _currentPage;
    
    public readonly ProductViewModel _productPage;
    
    public CategoryViewModel(CartService cartService)
    {
        _productPage = new ProductViewModel(cartService);
        CurrentPage = _currentPage;
    }
    
    [RelayCommand]
    private void GoToProductPage()
    {
        
        WeakReferenceMessenger.Default.Send(new NavigateMessage(_productPage));
    }
    
}