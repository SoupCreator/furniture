using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace myApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(SideMenuImage))]
    private bool _fullMenuVisible = true;

    public SvgImage SideMenuImage =>
        new SvgImage{Source = SvgSource.Load($"avares://{nameof(myApp)}/Assets/Images/{(FullMenuVisible ? "icon" : "sofa")}.svg")};

    [RelayCommand]
    private void SideMenuResize()
    {
        FullMenuVisible = !FullMenuVisible;
    }
}
