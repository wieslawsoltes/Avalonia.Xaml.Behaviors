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
        if (!IsEnabled)
        {
            return;
        }

        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
