using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Avalonia.Xaml.Interactions.Core;

/// <summary>
/// An action that will get the  clipboard formats.
/// </summary>
public class GetClipboardFormatsAction : InvokeCommandActionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetClipboardFormatsAction"/> class.
    /// </summary>
    public GetClipboardFormatsAction()
    {
        PassEventArgsToCommand = true;
    }

    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Avalonia.Xaml.Interactivity.IBehavior.AssociatedObject"/> or a target object.</param>
    /// <param name="parameter">The value of this parameter is determined by the caller.</param>
    /// <returns>True if the command is successfully executed; else false.</returns>
    public override object Execute(object? sender, object? parameter)
    {
        if (sender is not Visual visual)
        {
            return false;
        }

        Dispatcher.UIThread.InvokeAsync(async () => await GetClipboardFormatsAsync(visual));

        return true;
    }

    private async Task GetClipboardFormatsAsync(Visual visual)
    {
        if (IsEnabled != true || Command is null)
        {
            return;
        }
        
        string[]? formats = null;

        try
        {
            var clipboard = (visual.GetVisualRoot() as TopLevel)?.Clipboard;
            if (clipboard is null)
            {
                return;
            }

            formats = await clipboard.GetFormatsAsync();
        }
        catch (Exception)
        {
            // ignored
        }

        var resolvedParameter = ResolveParameter(formats);

        if (!Command.CanExecute(resolvedParameter))
        {
            return;
        }

        Command.Execute(resolvedParameter);
    }
}
