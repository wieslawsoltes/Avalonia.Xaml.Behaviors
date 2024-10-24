﻿using Avalonia.Controls;
using System.Diagnostics;
using System.Globalization;
using System;
using System.Reactive.Disposables;
using Avalonia.Reactive;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom.Core;

/// <summary>
/// A base class for behaviors that require <see cref="StyledElement"/> as base, usually for those having properties that are bound to <see cref="StyledElement.DataContext"/>.
/// </summary>
public abstract class StyledElementBehavior : StyledElement, IBehavior
{
    private IDisposable? _dataContextSyncer;

    /// <summary>
    /// Identifies the <seealso cref="IsEnabled"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<Behavior, bool>(nameof(IsEnabled), defaultValue: true);

    /// <summary>
    /// Gets the <see cref="AvaloniaObject"/> to which the behavior is attached.
    /// </summary>
    public AvaloniaObject? AssociatedObject { get; private set; }

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

        _dataContextSyncer = SyncDataContextFrom(associatedObject);

        OnAttached();
    }

    /// <summary>
    /// Detaches the behaviors from the <see cref="AssociatedObject"/>.
    /// </summary>
    public void Detach()
    {
        OnDetaching();
        AssociatedObject = null;
        _dataContextSyncer?.Dispose();
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

    internal void AttachedToVisualTree() => OnAttachedToVisualTree();

    internal void DetachedFromVisualTree() => OnDetachedFromVisualTree();

    internal void AttachedToLogicalTree() => OnAttachedToLogicalTree();

    internal void DetachedFromLogicalTree() => OnDetachedFromLogicalTree();

    internal void Loaded() => OnLoaded();

    internal void Unloaded() => OnUnloaded();

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

    private IDisposable SyncDataContextFrom(AvaloniaObject associatedObject)
    {
        if (associatedObject is StyledElement styledElement)
        {
            return styledElement
                .GetObservable(DataContextProperty)
                .Subscribe(new AnonymousObserver<object?>(o => DataContext = o));
        }

        return Disposable.Empty;
    }
}
