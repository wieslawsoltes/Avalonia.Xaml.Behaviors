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
        Execute(parameter: null);
    }

    private void Execute(object? parameter)
    {
        if (!IsEnabled)
        {
            return;
        }

        Interaction.ExecuteActions(AssociatedObject, Actions, parameter);
    }
}
