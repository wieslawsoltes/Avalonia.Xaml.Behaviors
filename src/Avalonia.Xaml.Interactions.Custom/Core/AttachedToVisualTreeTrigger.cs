using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class AttachedToVisualTreeTrigger : StyledElementTrigger<Visual>
{
    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
