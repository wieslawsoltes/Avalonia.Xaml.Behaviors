using System.Reactive.Disposables;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ListBoxSelectAllBehavior : AttachedToVisualTreeBehavior<Controls.ListBox>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        AssociatedObject?.SelectAll();
    }
}
