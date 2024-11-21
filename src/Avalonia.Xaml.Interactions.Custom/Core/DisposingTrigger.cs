using System;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class DisposingTrigger : StyledElementTrigger
{
    private IDisposable? _disposable;

    /// <summary>
    /// 
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();

        _disposable?.Dispose();
        _disposable = OnAttachedOverride();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>A disposable resource to be disposed when the behavior is detached.</returns>
    protected abstract IDisposable OnAttachedOverride();

    /// <summary>
    /// 
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();

        _disposable?.Dispose();
    }
}
