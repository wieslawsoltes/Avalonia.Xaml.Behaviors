using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DragAndDropSample.ViewModels;

namespace DragAndDropSample.Views;

public partial class DragAndDropView : UserControl
{
    public DragAndDropView()
    {
        InitializeComponent();

        DataContext = new DragAndDropSampleViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

