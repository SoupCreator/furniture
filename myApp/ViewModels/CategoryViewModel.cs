using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace myApp.ViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ViewModelBase _currentPage;
    
    public readonly ProductViewModel _productPage = new();
    
    public CategoryViewModel()
    {
        CurrentPage = _currentPage;
    }
    
    [RelayCommand]
    private void GoToProductPage()
    {
        
        WeakReferenceMessenger.Default.Send(new NavigateMessage(_productPage));
    }
    
}