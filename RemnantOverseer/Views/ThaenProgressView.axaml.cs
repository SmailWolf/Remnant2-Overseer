using Avalonia.Controls;
using Avalonia.Interactivity;
using RemnantOverseer.ViewModels;
using System;

namespace RemnantOverseer.Views;

public partial class ThaenProgressView : UserControl
{
    public ThaenProgressView()
    {
        if (Design.IsDesignMode)
        {
            Design.SetDataContext(this, new ThaenProgressViewModel(null));
        }
        InitializeComponent();
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        (DataContext as IDisposable)?.Dispose();
        base.OnUnloaded(e);
    }
}