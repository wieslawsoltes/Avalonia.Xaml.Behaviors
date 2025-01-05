using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class InitializedTrigger : StyledElementTrigger<StyledElement>
{
    /// <inheritdoc />
    protected override void OnInitializedEvent()
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
