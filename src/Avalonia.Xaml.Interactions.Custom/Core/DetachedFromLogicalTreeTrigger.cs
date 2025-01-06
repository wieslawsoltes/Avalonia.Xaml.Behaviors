using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class DetachedFromLogicalTreeTrigger : StyledElementTrigger<StyledElement>
{
    /// <inheritdoc />
    protected override void OnDetachedFromLogicalTree()
    {
        if (!IsEnabled)
        {
            return;
        }

        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
