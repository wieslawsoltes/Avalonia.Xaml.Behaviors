using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Reactive;

namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// Defines a <see cref="BehaviorCollection"/> attached property and provides a method for executing an <seealso cref="ActionCollection"/>.
/// </summary>
public class Interaction
{
    static Interaction()
    {
        BehaviorsProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<BehaviorCollection?>>(BehaviorsChanged));
    }

    /// <summary>
    /// Gets or sets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    public static readonly AttachedProperty<BehaviorCollection?> BehaviorsProperty =
        AvaloniaProperty.RegisterAttached<Interaction, AvaloniaObject, BehaviorCollection?>("Behaviors");

    /// <summary>
    /// Gets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    /// <param name="obj">The <see cref="AvaloniaObject"/> from which to retrieve the <see cref="BehaviorCollection"/>.</param>
    /// <returns>A <see cref="BehaviorCollection"/> containing the behaviors associated with the specified object.</returns>
    public static BehaviorCollection GetBehaviors(AvaloniaObject obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var behaviorCollection = obj.GetValue(BehaviorsProperty);
        if (behaviorCollection is null)
        {
            behaviorCollection = [];
            obj.SetValue(BehaviorsProperty, behaviorCollection);
            SetVisualTreeEventHandlersFromGetter(obj);
        }

        return behaviorCollection;
    }

    /// <summary>
    /// Sets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    /// <param name="obj">The <see cref="AvaloniaObject"/> on which to set the <see cref="BehaviorCollection"/>.</param>
    /// <param name="value">The <see cref="BehaviorCollection"/> associated with the object.</param>
    public static void SetBehaviors(AvaloniaObject obj, BehaviorCollection? value)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
        obj.SetValue(BehaviorsProperty, value);
    }

    /// <summary>
    /// Executes all actions in the <see cref="ActionCollection"/> and returns their results.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> which will be passed on to the action.</param>
    /// <param name="actions">The set of actions to execute.</param>
    /// <param name="parameter">The value of this parameter is determined by the calling behavior.</param>
    /// <returns>Returns the results of the actions.</returns>
    public static IEnumerable<object> ExecuteActions(object? sender, ActionCollection? actions, object? parameter)
    {
        if (actions is null)
        {
            return [];
        }

        var results = new List<object>();

        foreach (var avaloniaObject in actions)
        {
            if (avaloniaObject is not IAction action)
            {
                continue;
            }

            var result = action.Execute(sender, parameter);
            if (result is not null)
            {
                results.Add(result);
            }
        }

        return results;
    }

    private static void BehaviorsChanged(AvaloniaPropertyChangedEventArgs<BehaviorCollection?> e)
    {
        var oldCollection = e.OldValue.GetValueOrDefault();
        var newCollection = e.NewValue.GetValueOrDefault();

        if (oldCollection == newCollection)
        {
            return;
        }

        if (oldCollection is { AssociatedObject: not null })
        {
            oldCollection.Detach();
        }

        if (newCollection is not null)
        {
            newCollection.Attach(e.Sender);
            SetVisualTreeEventHandlersFromChangedEvent(e.Sender);
        }
    }

    private static void SetVisualTreeEventHandlersFromGetter(AvaloniaObject obj)
    {
        if (obj is not Control control)
        {
            return;
        }

        // AttachedToVisualTree / DetachedFromVisualTree

        control.AttachedToVisualTree -= Control_AttachedToVisualTreeFromChangedEvent;
        control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeFromChangedEvent;
        control.AttachedToVisualTree -= Control_AttachedToVisualTreeFromGetter;
        control.AttachedToVisualTree += Control_AttachedToVisualTreeFromGetter;
        control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeFromGetter;
        control.DetachedFromVisualTree += Control_DetachedFromVisualTreeFromGetter;

        // AttachedToLogicalTree / DetachedFromLogicalTree

        control.AttachedToLogicalTree -= Control_AttachedToLogicalTreeFromChangedEvent;
        control.DetachedFromLogicalTree -= Control_DetachedFromLogicalTreeFromChangedEvent;
        control.AttachedToLogicalTree -= Control_AttachedToLogicalTreeFromGetter;
        control.AttachedToLogicalTree += Control_AttachedToLogicalTreeFromGetter;
        control.DetachedFromLogicalTree -= Control_DetachedFromLogicalTreeFromGetter;
        control.DetachedFromLogicalTree += Control_DetachedFromLogicalTreeFromGetter;

        // Loaded / Unloaded

        control.Loaded -= Control_LoadedFromChangedEvent;
        control.Unloaded -= Control_UnloadedFromChangedEvent;
        control.Loaded -= Control_LoadedFromGetter;
        control.Loaded += Control_LoadedFromGetter;
        control.Unloaded -= Control_UnloadedFromGetter;
        control.Unloaded += Control_UnloadedFromGetter;

        if (obj is TopLevel topLevel)
        {
            topLevel.Opened -= TopLevel_OpenedFromChangedEvent;
            topLevel.Opened -= TopLevel_OpenedFromGetter;
            topLevel.Opened += TopLevel_OpenedFromGetter;
        }
    }

    private static void SetVisualTreeEventHandlersFromChangedEvent(AvaloniaObject obj)
    {
        if (obj is not Control control)
        {
            return;
        }

        // AttachedToVisualTree / DetachedFromVisualTree

        control.AttachedToVisualTree -= Control_AttachedToVisualTreeFromGetter;
        control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeFromGetter;
        control.AttachedToVisualTree -= Control_AttachedToVisualTreeFromChangedEvent;
        control.AttachedToVisualTree += Control_AttachedToVisualTreeFromChangedEvent;
        control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeFromChangedEvent;
        control.DetachedFromVisualTree += Control_DetachedFromVisualTreeFromChangedEvent;

        // AttachedToLogicalTree / DetachedFromLogicalTree

        control.AttachedToLogicalTree -= Control_AttachedToLogicalTreeFromGetter;
        control.DetachedFromLogicalTree -= Control_DetachedFromLogicalTreeFromGetter;
        control.AttachedToLogicalTree -= Control_AttachedToLogicalTreeFromChangedEvent;
        control.AttachedToLogicalTree += Control_AttachedToLogicalTreeFromChangedEvent;
        control.DetachedFromLogicalTree -= Control_DetachedFromLogicalTreeFromChangedEvent;
        control.DetachedFromLogicalTree += Control_DetachedFromLogicalTreeFromChangedEvent;

        // Loaded / Unloaded

        control.Loaded -= Control_LoadedFromGetter;
        control.Unloaded -= Control_UnloadedFromGetter;
        control.Loaded -= Control_LoadedFromChangedEvent;
        control.Loaded += Control_LoadedFromChangedEvent;
        control.Unloaded -= Control_UnloadedFromChangedEvent;
        control.Unloaded += Control_UnloadedFromChangedEvent;

        if (obj is TopLevel topLevel)
        {
            topLevel.Opened -= TopLevel_OpenedFromGetter;
            topLevel.Opened -= TopLevel_OpenedFromChangedEvent;
            topLevel.Opened += TopLevel_OpenedFromChangedEvent;
        }
    }

    // AttachedToVisualTree / DetachedFromVisualTree

    private static void Control_AttachedToVisualTreeFromGetter(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Attach(d);
        GetBehaviors(d).AttachedToVisualTree();
    }

    private static void Control_DetachedFromVisualTreeFromGetter(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).DetachedFromVisualTree();
        GetBehaviors(d).Detach();
    }
 
    private static void Control_AttachedToVisualTreeFromChangedEvent(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).AttachedToVisualTree();
    }

    private static void Control_DetachedFromVisualTreeFromChangedEvent(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).DetachedFromVisualTree();
    }

    // AttachedToLogicalTree / DetachedFromLogicalTree

    private static void Control_AttachedToLogicalTreeFromGetter(object? sender, LogicalTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).AttachedToLogicalTree();
    }

    private static void Control_DetachedFromLogicalTreeFromGetter(object? sender, LogicalTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).DetachedFromLogicalTree();
    }
 
    private static void Control_AttachedToLogicalTreeFromChangedEvent(object? sender, LogicalTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).AttachedToLogicalTree();
    }

    private static void Control_DetachedFromLogicalTreeFromChangedEvent(object? sender, LogicalTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).DetachedFromLogicalTree();
    }

    // Loaded / Unloaded

    private static void Control_LoadedFromGetter(object? sender, RoutedEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Loaded();
    }

    private static void Control_UnloadedFromGetter(object? sender, RoutedEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Unloaded();
    }
 
    private static void Control_LoadedFromChangedEvent(object? sender, RoutedEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Loaded();
    }

    private static void Control_UnloadedFromChangedEvent(object? sender, RoutedEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Unloaded();
    }
    
    // TopLevel Opened

    private static void TopLevel_OpenedFromGetter(object sender, EventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Attach(d);
        GetBehaviors(d).AttachedToVisualTree();
        GetBehaviors(d).AttachedToLogicalTree();
    }

    private static void TopLevel_OpenedFromChangedEvent(object sender, EventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).AttachedToVisualTree();
        GetBehaviors(d).AttachedToLogicalTree();
    }
}
