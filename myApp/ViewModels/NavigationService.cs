using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace myApp.ViewModels;

public class NavigateMessage : ValueChangedMessage<ViewModelBase>
{
    public NavigateMessage(ViewModelBase viewModel) : base(viewModel) { }
}