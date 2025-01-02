using System;
using System.Diagnostics;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Reactive;

namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// A base class for behaviors, implementing the basic plumbing of <see cref="IBehavior"/>.
/// </summary>
public abstract class StyledElementBehavior : StyledElement, IBehavior, IBehaviorEventsHandler
{
    private IDisposable? _dataContextDisposable;

    /// <summary>
    /// Identifies the <seealso cref="IsEnabled"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<StyledElementBehavior, bool>(nameof(IsEnabled), defaultValue: true);

    /// <summary>
    /// Gets the <see cref="AvaloniaObject"/> to which the behavior is attached.
    /// </summary>
    public AvaloniaObject? AssociatedObject { get; private set; }

    /// <summary>
    /// Gets the <see cref="StyledElement"/> to which this behavior is attached.
    /// </summary>
    public StyledElement? AssociatedStyledElement => AssociatedObject as StyledElement;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is enabled.
    /// </summary>
    /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
    public bool IsEnabled
    {
        get => GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// Attaches the behavior to the specified <see cref="AvaloniaObject"/>.
    /// </summary>
    /// <param name="associatedObject">The <see cref="AvaloniaObject"/> to which to attach.</param>
    /// <exception cref="ArgumentNullException"><paramref name="associatedObject"/> is null.</exception>
    public void Attach(AvaloniaObject? associatedObject)
    {
        if (Equals(associatedObject, AssociatedObject))
        {
            return;
        }

        if (AssociatedObject is not null)
        {
            throw new InvalidOperationException(string.Format(
                CultureInfo.CurrentCulture,
                "An instance of a behavior cannot be attached to more than one object at a time."));
        }

        Debug.Assert(associatedObject is not null, "Cannot attach the behavior to a null object.");
        AssociatedObject = associatedObject ?? throw new ArgumentNullException(nameof(associatedObject));
        _dataContextDisposable = SynchronizeDataContext(associatedObject);

        OnAttached();
    }

    /// <summary>
    /// Detaches the behaviors from the <see cref="AssociatedObject"/>.
    /// </summary>
    public void Detach()
    {
        OnDetaching();
        _dataContextDisposable?.Dispose();
        AssociatedObject = null;
    }

    /// <summary>
    /// Called after the behavior is attached to the <see cref="AssociatedObject"/>.
    /// </summary>
    /// <remarks>
    /// Override this to hook up functionality to the <see cref="AssociatedObject"/>
    /// </remarks>
    protected virtual void OnAttached()
    {
    }

    /// <summary>
    /// Called when the behavior is being detached from its <see cref="AssociatedObject"/>.
    /// </summary>
    /// <remarks>
    /// Override this to unhook functionality from the <see cref="AssociatedObject"/>
    /// </remarks>
    protected virtual void OnDetaching()
    {
    }

    void IBehaviorEventsHandler.AttachedToVisualTreeEventHandler()
    {
        AttachBehaviorToLogicalTree();

        OnAttachedToVisualTree();
    }

    void IBehaviorEventsHandler.DetachedFromVisualTreeEventHandler()
    {
        DetachBehaviorFromLogicalTree();

        OnDetachedFromVisualTree();
    }

    void IBehaviorEventsHandler.AttachedToLogicalTreeEventHandler()
    {
        AttachBehaviorToLogicalTree();

        OnAttachedToLogicalTree();
    }

    void IBehaviorEventsHandler.DetachedFromLogicalTreeEventHandler()
    {
        DetachBehaviorFromLogicalTree();

        OnDetachedFromLogicalTree();
    }

    void IBehaviorEventsHandler.LoadedEventHandler() => OnLoaded();

    void IBehaviorEventsHandler.UnloadedEventHandler() => OnUnloaded();

    /// <summary>
    /// Called after the <see cref="AssociatedObject"/> is attached to the visual tree.
    /// </summary>
    /// <remarks>
    /// Invoked only when the <see cref="AssociatedObject"/> is of type <see cref="Control"/>.
    /// </remarks>
    protected virtual void OnAttachedToVisualTree()
    {
    }

    /// <summary>
    /// Called when the <see cref="AssociatedObject"/> is being detached from the visual tree.
    /// </summary>
    /// <remarks>
    /// Invoked only when the <see cref="AssociatedObject"/> is of type <see cref="Control"/>.
    /// </remarks>
    protected virtual void OnDetachedFromVisualTree()
    {
    }

    /// <summary>
    /// Called after the <see cref="AssociatedObject"/> is attached to the logical tree.
    /// </summary>
    /// <remarks>
    /// Invoked only when the <see cref="AssociatedObject"/> is of type <see cref="Control"/>.
    /// </remarks>
    protected virtual void OnAttachedToLogicalTree()
    {
    }

    /// <summary>
    /// Called when the <see cref="AssociatedObject"/> is being detached from the logical tree.
    /// </summary>
    /// <remarks>
    /// Invoked only when the <see cref="AssociatedObject"/> is of type <see cref="Control"/>.
    /// </remarks>
    protected virtual void OnDetachedFromLogicalTree()
    {
    }

    /// <summary>
    /// Called after the <see cref="AssociatedObject"/> is loaded.
    /// </summary>
    /// <remarks>
    /// Invoked only when the <see cref="AssociatedObject"/> is of type <see cref="Control"/>.
    /// </remarks>
    protected virtual void OnLoaded()
    {
    }

    /// <summary>
    /// Called when the <see cref="AssociatedObject"/> is unloaded.
    /// </summary>
    /// <remarks>
    /// Invoked only when the <see cref="AssociatedObject"/> is of type <see cref="Control"/>.
    /// </remarks>
    protected virtual void OnUnloaded()
    {
    }

    internal virtual void AttachBehaviorToLogicalTree()
    {
        if (AssociatedObject is not StyledElement styledElement || styledElement.Parent is null)
        {
            return;
        }

        // Required for $parent binding in XAML
        ((ISetLogicalParent)this).SetParent(null);
        ((ISetLogicalParent)this).SetParent(styledElement);

        // Required for TemplateBinding in XAML
        if (styledElement.TemplatedParent is { } templatedParent)
        {
            TemplatedParentHelper.SetTemplatedParent(this, templatedParent);
        }
    }

    internal virtual void DetachBehaviorFromLogicalTree()
    {
        ((ISetLogicalParent)this).SetParent(null);

        if (AssociatedObject is StyledElement { TemplatedParent: not null })
        {
            TemplatedParentHelper.SetTemplatedParent(this, null);
        }
    }

    private IDisposable? SynchronizeDataContext(AvaloniaObject associatedObject)
    {
        if (associatedObject is StyledElement styledElement)
        {
            // Required for data context binding in XAML
            return styledElement
                .GetObservable(DataContextProperty)
                .Subscribe(new AnonymousObserver<object?>(x =>
                {
                    SetCurrentValue(DataContextProperty, x);
                }));
        }

        return default;
    }
}
