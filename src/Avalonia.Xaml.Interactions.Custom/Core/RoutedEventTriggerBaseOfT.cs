using System.Reactive.Disposables;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class RoutedEventTriggerBase<T> : RoutedEventTriggerBase where T : RoutedEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    protected abstract RoutedEvent<T> RoutedEvent { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttached(CompositeDisposable disposables)
    {
        if (AssociatedObject is Interactive interactive)
        {
            var disposable = interactive.AddDisposableHandler(
                RoutedEvent, 
                Handler, 
                EventRoutingStrategy);
            disposables.Add(disposable);
        }
    }

    private void Handler(object? sender, T e)
    {
        if (!IsEnabled)
        {
            return;
        }

        e.Handled = MarkAsHandled;
        Interaction.ExecuteActions(AssociatedObject, Actions, e);
    }
}
