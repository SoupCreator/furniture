using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using myApp.ViewModels;
namespace myApp.Views;

public partial class ServicesView : UserControl
{
    public ServicesView()
    {
        InitializeComponent();
        DataContext = new ServicesViewModel();
    }
}