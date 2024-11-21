using System;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class KeyDownTrigger : RoutedEventTriggerBase
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<Key> KeyProperty =
        AvaloniaProperty.Register<KeyDownTrigger, Key>(nameof(Key));

    /// <summary>
    /// 
    /// </summary>
    public Key Key
    {
        get => GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool MarkAsHandled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IDisposable OnAttachedOverride()
    {
        if (AssociatedObject is InputElement element)
        {
            return element.AddDisposableHandler(InputElement.KeyDownEvent, OnKeyDown, EventRoutingStrategy);
        }
        
        return DisposableAction.Empty;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        if (e.Key == Key)
        {
            e.Handled = MarkAsHandled;
            Interaction.ExecuteActions(AssociatedObject, Actions, null);
        }
    }
}
