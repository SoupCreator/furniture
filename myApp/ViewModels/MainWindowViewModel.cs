using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using myApp.Services;

namespace myApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const string buttonActiveClass = "active";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(cartPageIsActive))]
    [NotifyPropertyChangedFor(nameof(servicesPageIsActive))]
    [NotifyPropertyChangedFor(nameof(categoryPageIsActive))]
    [NotifyPropertyChangedFor(nameof(homePageIsActive))]
    [NotifyPropertyChangedFor(nameof(productPageIsActive))]
    private ViewModelBase _currentPage;

    public bool homePageIsActive => CurrentPage is HomeViewModel;
    public bool categoryPageIsActive => CurrentPage is CategoryViewModel || CurrentPage is ProductViewModel;
    public bool cartPageIsActive => CurrentPage is CartViewModel;
    public bool servicesPageIsActive => CurrentPage is ServicesViewModel;
    
    public bool productPageIsActive => CurrentPage is ProductViewModel;
    
    private readonly CartService _cartService = new();
    
    private readonly HomeViewModel _homePage = new ();
    private  CartViewModel _cartPage;
    private readonly ServicesViewModel _servicesPage;
    private CategoryViewModel _categoryPage;
    private readonly ProductViewModel _productPage;
    
    
    public MainWindowViewModel()
    {
        _cartPage = new CartViewModel(_cartService);
        _productPage = new ProductViewModel(_cartService);
        _categoryPage = new CategoryViewModel(_cartService);
        _servicesPage = new ServicesViewModel(_cartService);
        
        
        CurrentPage = _homePage;
        WeakReferenceMessenger.Default.Register<NavigateMessage>(
            this,
            (r, m) =>
            {
                CurrentPage = m.Value;
            });
    }
    
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(SideMenuImage))]
    private bool _fullMenuVisible = true;

    public SvgImage SideMenuImage =>
        new SvgImage{Source = SvgSource.Load($"avares://{nameof(myApp)}/Assets/Images/{(FullMenuVisible ? "icon" : "sofa")}.svg")};

    [RelayCommand]
    private void SideMenuResize()
    {
        FullMenuVisible = !FullMenuVisible;
    }
    
    [RelayCommand]
    private void GoHome()
    {
        CurrentPage = _homePage;
    }
    
    [RelayCommand]
    private void GoCart()
    {
        CurrentPage = _cartPage;
    }

   
    [RelayCommand]
    private void GoServices()
    {
        CurrentPage = _servicesPage;
    }
    
    
    [RelayCommand]
    private void GoCategory()
    { 
        CurrentPage = _categoryPage;
    }
    
}
