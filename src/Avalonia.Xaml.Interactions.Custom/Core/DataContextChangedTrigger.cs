using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class DataContextChangedTrigger : StyledElementTrigger<StyledElement>
{
    /// <inheritdoc />
    protected override void OnDataContextChangedEvent()
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
