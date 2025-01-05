using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class ActualThemeVariantChangedTrigger : StyledElementTrigger<StyledElement>
{
    /// <inheritdoc />
    protected override void OnActualThemeVariantChangedEvent()
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
