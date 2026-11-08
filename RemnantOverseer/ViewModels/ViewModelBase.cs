using CommunityToolkit.Mvvm.ComponentModel;

namespace RemnantOverseer.ViewModels;
public class ViewModelBase : ObservableRecipient
{
    public bool IsInitialized { get; protected set; }
}
