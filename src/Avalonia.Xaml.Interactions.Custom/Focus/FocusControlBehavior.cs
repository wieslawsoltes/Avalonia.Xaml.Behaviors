using System;
using Avalonia.Controls;
using Avalonia.Reactive;
using Avalonia.Threading;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class FocusControlBehavior : AttachedToVisualTreeBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> FocusFlagProperty =
        AvaloniaProperty.Register<FocusControlBehavior, bool>(nameof(FocusFlag));

    /// <summary>
    /// 
    /// </summary>
    public bool FocusFlag
    {
        get => GetValue(FocusFlagProperty);
        set => SetValue(FocusFlagProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IDisposable OnAttachedOverride()
    {
        return this.GetObservable(FocusFlagProperty)
            .Subscribe(new AnonymousObserver<bool>(
                focusFlag =>
                {
                    if (focusFlag && IsEnabled)
                    {
                        Dispatcher.UIThread.Post(() => AssociatedObject?.Focus());
                    }
                }));
    }

    /// <summary>
    /// 
    /// </summary>
    protected override System.IDisposable OnAttachedToVisualTreeOverride()
    {
        if (FocusFlag && IsEnabled)
        {
            Dispatcher.UIThread.Post(() => AssociatedObject?.Focus());
        }
        
        return DisposableAction.Empty;
    }
}
