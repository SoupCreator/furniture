using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using myApp.ViewModels;

namespace myApp;

public class ViewLocator : IDataTemplate
{

    public Control? Build(object? param)
    {
        if (param is null)
            return null;
        
        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.InvariantCulture);
        var type = Type.GetType(name);

        if (type is null)
        {
            return new TextBlock { Text = "Not Found: " + name };
        }
        
        var control = (Control)Activator.CreateInstance(type)!;
        control.DataContext = param;
        return control;
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
