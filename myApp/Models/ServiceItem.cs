using CommunityToolkit.Mvvm.ComponentModel;

namespace myApp.Models;

public partial class ServiceItem : ObservableObject
{
    public Service Service { get; }

    [ObservableProperty]
    private bool _isSelected;

    public ServiceItem(Service service, bool isSelected = false)
    {
        Service = service;
        IsSelected = isSelected;
    }
}
