using System.Reactive.Disposables;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class ItemsControlContainerEventsBehavior : DisposingBehavior<ItemsControl>
{
    /// <inheritdoc />
    protected override void OnAttached(CompositeDisposable disposables)
    {
        if (AssociatedObject is not { } itemsControl)
        {
            return;
        }

        itemsControl.ContainerPrepared += ItemsControlOnContainerPrepared;
        itemsControl.ContainerIndexChanged += ItemsControlOnContainerIndexChanged;
        itemsControl.ContainerClearing += ItemsControlOnContainerClearing;

        disposables.Add(Disposable.Create(() =>
        {
            itemsControl.ContainerPrepared -= ItemsControlOnContainerPrepared;
            itemsControl.ContainerIndexChanged -= ItemsControlOnContainerIndexChanged;
            itemsControl.ContainerClearing -= ItemsControlOnContainerClearing;
        }));
    }

    private void ItemsControlOnContainerPrepared(object? sender, ContainerPreparedEventArgs e)
    {
        OnContainerPrepared(sender, e);
    }
    
    private void ItemsControlOnContainerIndexChanged(object sender, ContainerIndexChangedEventArgs e)
    {
        OnContainerIndexChanged(sender, e);
    }

    private void ItemsControlOnContainerClearing(object? sender, ContainerClearingEventArgs e)
    {
        OnContainerClearing(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnContainerPrepared(object? sender, ContainerPreparedEventArgs e)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnContainerIndexChanged(object? sender, ContainerIndexChangedEventArgs e)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnContainerClearing(object? sender, ContainerClearingEventArgs e)
    {
    }
}
