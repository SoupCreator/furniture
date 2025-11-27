using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading; // Нужно для обновления UI из другого потока, если потребуется
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore; // Нужно для работы с БД
using myApp.Models;
using myApp.Data; // Подключи namespace, где лежит AppDbContext
using myApp.Services;

namespace myApp.ViewModels;

public partial class ProductViewModel : ViewModelBase
{
    // Коллекция для привязки в XAML
    public ObservableCollection<Product> Products { get; } = new();

    // Выбранный товар. Атрибут [ObservableProperty] сам создаст свойство SelectedProduct
    [ObservableProperty]
    private Product? _selectedProduct;

    private readonly CartService _cartService;

    public ProductViewModel()
    {
        _ = LoadProductsFromDb();
    }
    public ProductViewModel(CartService cartService)
    {
        _cartService = cartService;
        // Запускаем загрузку данных при создании ViewModel
        _ = LoadProductsFromDb();
    }

    // Метод загрузки данных из PostgreSQL
    private async Task LoadProductsFromDb()
    {
        try
        {
            using (var context = new AppDbContext())
            {
                // Асинхронно получаем список товаров из базы
                var items = await context.Products.ToListAsync();

                // Очищаем текущий список и добавляем новые данные
                Products.Clear();
                
                foreach (var item in items)
                {
                    Products.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            // Здесь можно добавить логирование или вывод ошибки в консоль
            Console.WriteLine($"Ошибка подключения к БД: {ex.Message}");
        }
    }

    [RelayCommand]
    private void AddToCart()
    {
        if (SelectedProduct != null)
        {
            _cartService.AddToCart(SelectedProduct);
            // Обрати внимание: используем ProductName вместо Name, так как модель изменилась
            Console.WriteLine($"Добавлено в корзину: {SelectedProduct.ProductName} (Цена: {SelectedProduct.ProductPrice})");
        }
    }
}