using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class AttachedToLogicalTreeTrigger : StyledElementTrigger<StyledElement>
{
    /// <inheritdoc />
    protected override void OnAttachedToLogicalTree()
    {
        if (!IsEnabled)
        {
            return;
        }

        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
