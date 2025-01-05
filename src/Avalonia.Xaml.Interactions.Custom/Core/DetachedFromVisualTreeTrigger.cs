using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class DetachedFromVisualTreeTrigger : StyledElementTrigger<Visual>
{
    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
