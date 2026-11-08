using Avalonia.Controls;
using RemnantOverseer.ViewModels;

namespace RemnantOverseer.Views;

public partial class PatchworkQuiltView : UserControl
{
    public PatchworkQuiltView()
    {
        if (Design.IsDesignMode)
        {
            Design.SetDataContext(this, new PatchworkQuiltViewModel([]));
        }
        InitializeComponent();
    }
}