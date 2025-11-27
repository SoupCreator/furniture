using CommunityToolkit.Mvvm.ComponentModel;

namespace myApp.Models;

public partial class CartItem : ObservableObject
{
    public Product Product { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalPrice))]
    private int _quantity;

    public double TotalPrice => Product.ProductPrice * Quantity;

    public CartItem(Product product, int quantity = 1)
    {
        Product = product;
        Quantity = quantity;
    }
}
