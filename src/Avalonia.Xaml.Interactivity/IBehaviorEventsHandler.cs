namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// Interface implemented by base behaviors.
/// </summary>
public interface IBehaviorEventsHandler
{ 
    /// <summary>
    /// Called when element is attached to the visual tree.
    /// </summary>
    void AttachedToVisualTreeEventHandler();

    /// <summary>
    /// Called when element is detached from the visual tree.
    /// </summary>
    void DetachedFromVisualTreeEventHandler();

    /// <summary>
    /// Called when element is attached to the logical tree.
    /// </summary>
    void AttachedToLogicalTreeEventHandler();

    /// <summary>
    /// Called when element is detached from the logical tree.
    /// </summary>
    void DetachedFromLogicalTreeEventHandler();

    /// <summary>
    /// Called when element is loaded.
    /// </summary>
    void LoadedEventHandler();

    /// <summary>
    /// Called when element is unloaded.
    /// </summary>
    void UnloadedEventHandler();
}
