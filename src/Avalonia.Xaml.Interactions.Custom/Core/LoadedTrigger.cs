using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class LoadedTrigger : StyledElementTrigger<Control>
{
    /// <inheritdoc />
    protected override void OnLoaded()
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, parameter: null);
    }
}
