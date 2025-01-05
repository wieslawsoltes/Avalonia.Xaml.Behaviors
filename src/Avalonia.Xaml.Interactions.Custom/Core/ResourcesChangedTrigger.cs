using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class ResourcesChangedTrigger : StyledElementTrigger<StyledElement>
{
    /// <inheritdoc />
    protected override void OnResourcesChangedEvent()
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
