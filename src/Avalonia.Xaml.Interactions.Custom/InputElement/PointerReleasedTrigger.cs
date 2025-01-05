using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class PointerReleasedTrigger : RoutedEventTriggerBase
{
    static PointerReleasedTrigger()
    {
        EventRoutingStrategyProperty.OverrideMetadata<PointerReleasedTrigger>(
            new StyledPropertyMetadata<RoutingStrategies>(
                defaultValue: RoutingStrategies.Tunnel | RoutingStrategies.Bubble));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttached(CompositeDisposable disposables)
    {
        if (AssociatedObject is InputElement element)
        {
            var disposable = element.AddDisposableHandler(
                InputElement.PointerReleasedEvent, 
                OnPointerReleased, 
                EventRoutingStrategy);
            disposables.Add(disposable);
        }
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        e.Handled = MarkAsHandled;
        Interaction.ExecuteActions(AssociatedObject, Actions, e);
    }
}
