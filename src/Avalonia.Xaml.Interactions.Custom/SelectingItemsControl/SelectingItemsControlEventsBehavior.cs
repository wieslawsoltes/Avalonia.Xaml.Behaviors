using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class SelectingItemsControlEventsBehavior : DisposingBehavior<SelectingItemsControl>
{
    /// <inheritdoc />
    protected override void OnAttached(CompositeDisposable disposables)
    {
        if (AssociatedObject is not { } selectingItemsControl)
        {
            return;
        }

        selectingItemsControl.SelectionChanged += SelectingItemsControlOnSelectionChanged;

        disposables.Add(
            Disposable.Create(
                () => selectingItemsControl.SelectionChanged -= SelectingItemsControlOnSelectionChanged));
    }

    private void SelectingItemsControlOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        OnSelectionChanged(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
    }
}
