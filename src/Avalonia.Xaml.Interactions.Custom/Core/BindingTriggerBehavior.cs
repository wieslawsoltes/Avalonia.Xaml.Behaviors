﻿using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Data;
using Avalonia.Reactive;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A behavior that performs actions when the bound data meets a specified condition.
/// </summary>
[RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
public class BindingTriggerBehavior : StyledElementTrigger
{
    /// <summary>
    /// Identifies the <seealso cref="Binding"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<IBinding?> BindingProperty =
        AvaloniaProperty.Register<BindingTriggerBehavior, IBinding?>(nameof(Binding));

    /// <summary>
    /// Identifies the <seealso cref="ComparisonCondition"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<ComparisonConditionType> ComparisonConditionProperty =
        AvaloniaProperty.Register<BindingTriggerBehavior, ComparisonConditionType>(nameof(ComparisonCondition));

    /// <summary>
    /// Identifies the <seealso cref="Value"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<BindingTriggerBehavior, object?>(nameof(Value));

    private static readonly StyledProperty<object?> BindingValueProperty =
        AvaloniaProperty.Register<BindingTriggerBehavior, object?>(nameof(BindingValue));

    private IDisposable? _dispose;

    /// <summary>
    /// Gets or sets the bound object that the <see cref="BindingTriggerBehavior"/> will listen to. This is an avalonia property.
    /// </summary>
    [AssignBinding]
    public IBinding? Binding
    {
        get => GetValue(BindingProperty);
        set => SetValue(BindingProperty, value);
    }

    /// <summary>
    /// Gets or sets the type of comparison to be performed between <see cref="BindingTriggerBehavior.Binding"/> and <see cref="BindingTriggerBehavior.Value"/>. This is an avalonia property.
    /// </summary>
    public ComparisonConditionType ComparisonCondition
    {
        get => GetValue(ComparisonConditionProperty);
        set => SetValue(ComparisonConditionProperty, value);
    }

    /// <summary>
    /// Gets or sets the value to be compared with the value of <see cref="BindingTriggerBehavior.Binding"/>. This is an avalonia property.
    /// </summary>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private object? BindingValue
    {
        get => GetValue(BindingValueProperty);
        set => SetValue(BindingValueProperty, value);
    }
    
    static BindingTriggerBehavior()
    {
        ComparisonConditionProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<ComparisonConditionType>>(OnValueChanged));

        ValueProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<object?>>(OnValueChanged));

        BindingValueProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<object?>>(OnValueChanged));
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == BindingProperty)
        {
            _dispose?.Dispose();

            var newValue = change.GetNewValue<IBinding?>();
            if (newValue is not null)
            {
                _dispose = Bind(BindingValueProperty, newValue);
            }
        }
    }

    /// <inheritdoc />
    protected override void OnDetaching()
    {
        base.OnDetaching();

        _dispose?.Dispose();
    }

    private static void OnValueChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not BindingTriggerBehavior behavior || behavior.AssociatedObject is null)
        {
            return;
        }

        if (!behavior.IsEnabled)
        {
            return;
        }

        // NOTE: In UWP version binding null check is not present but Avalonia throws exception as Bindings are null when first initialized.
        var binding = behavior.BindingValue;
        if (binding is not null)
        {
            // Some value has changed--either the binding value, reference value, or the comparison condition. Re-evaluate the equation.
            if (ComparisonConditionTypeHelper.Compare(behavior.BindingValue, behavior.ComparisonCondition,
                    behavior.Value))
            {
                Interaction.ExecuteActions(behavior.AssociatedObject, behavior.Actions, args);
            }
        }
    }
}
