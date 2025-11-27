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

    public bool homePageIsActive => CurrentPage == _homePage;
    public bool categoryPageIsActive => CurrentPage == _categoryPage || CurrentPage == _productPage;
    public bool cartPageIsActive => CurrentPage == _cartPage;
    public bool servicesPageIsActive => CurrentPage == _servicesPage;
    
    public bool productPageIsActive => CurrentPage == _productPage;
    
    private readonly CartService _cartService = new();
    
    private readonly HomeViewModel _homePage = new ();
    private readonly CartViewModel _cartPage;
    private readonly ServicesViewModel _servicesPage = new();
    private CategoryViewModel _categoryPage = new ();
    private readonly ProductViewModel _productPage;
    
    
    public MainWindowViewModel()
    {
        _cartPage = new CartViewModel(_cartService);
        _productPage = new ProductViewModel(_cartService);
        
        
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
