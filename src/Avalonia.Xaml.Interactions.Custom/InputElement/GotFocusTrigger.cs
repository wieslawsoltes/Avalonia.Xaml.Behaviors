using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class GotFocusTrigger : RoutedEventTriggerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttached(CompositeDisposable disposables)
    {
        if (AssociatedObject is InputElement element)
        {
            var disposable = element.AddDisposableHandler(
                InputElement.GotFocusEvent, 
                OnGotFocus, 
                EventRoutingStrategy);
            disposables.Add(disposable);
        }
    }

    private void OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        e.Handled = MarkAsHandled;
        Interaction.ExecuteActions(AssociatedObject, Actions, e);
    }
}
