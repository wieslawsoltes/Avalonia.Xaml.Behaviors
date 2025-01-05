using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class TextInputTrigger : RoutedEventTriggerBase
{
    static TextInputTrigger()
    {
        EventRoutingStrategyProperty.OverrideMetadata<TextInputTrigger>(
            new StyledPropertyMetadata<RoutingStrategies>(
                defaultValue: RoutingStrategies.Tunnel | RoutingStrategies.Bubble));
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<KeyDownTrigger, string?>(nameof(Text));

    /// <summary>
    /// 
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
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
                InputElement.TextInputEvent, 
                OnTextInput, 
                EventRoutingStrategy);
            disposables.Add(disposable);
        }
    }

    private void OnTextInput(object? sender, TextInputEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        var text = Text;
        var haveText = text is not null && e.Text == text;

        if (text is null || haveText)
        {
            return;
        }

        e.Handled = MarkAsHandled;
        Interaction.ExecuteActions(AssociatedObject, Actions, e);
    }
}
