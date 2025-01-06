using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class PullGestureGestureTrigger : RoutedEventTriggerBase<PullGestureEventArgs>
{
    /// <inheritdoc />
    protected override RoutedEvent<PullGestureEventArgs> RoutedEvent 
        => Gestures.PullGestureEvent;
}
