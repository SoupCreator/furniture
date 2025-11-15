using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using myApp.ViewModels;

namespace myApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.ClickCount !=2)
            return;
        
        (DataContext as MainWindowViewModel)?.SideMenuResizeCommand?.Execute(null);
    }
    
}