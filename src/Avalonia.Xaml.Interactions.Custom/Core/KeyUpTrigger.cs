using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class KeyUpTrigger : RoutedEventTriggerBase
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<Key?> KeyProperty =
        AvaloniaProperty.Register<KeyUpTrigger, Key?>(nameof(Key));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<KeyGesture?> GestureProperty =
        AvaloniaProperty.Register<KeyUpTrigger, KeyGesture?>(nameof(Gesture));

    /// <summary>
    /// 
    /// </summary>
    public Key? Key
    {
        get => GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public KeyGesture? Gesture
    {
        get => GetValue(GestureProperty);
        set => SetValue(GestureProperty, value);
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
                InputElement.KeyUpEvent, 
                OnKeyUp, 
                EventRoutingStrategy);
            disposables.Add(disposable);
        }
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        var key = Key;
        var gesture = Gesture;
        var haveKey = key is not null && e.Key == key;
        var haveGesture = gesture is not null && gesture.Matches(e);

        if ((key is null && gesture is null) 
            || haveKey
            || haveGesture)
        {
            return;
        }

        e.Handled = MarkAsHandled;
        Interaction.ExecuteActions(AssociatedObject, Actions, e);
    }
}
