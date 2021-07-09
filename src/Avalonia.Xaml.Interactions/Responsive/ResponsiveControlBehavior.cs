﻿using System;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactions.Core;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Responsive
{
    /// <summary>
    /// Observes attached control Bounds property changes and if triggered sets or removes style classes.
    /// </summary>
    public sealed class ResponsiveControlBehavior : Behavior<Control>
    {
        private IDisposable? _disposable;
        private AvaloniaList<ResponsiveTrigger>? _triggers;

        /// <summary>
        /// Identifies the <seealso cref="Control"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<Control?> ControlProperty =
            AvaloniaProperty.Register<ResponsiveControlBehavior, Control?>(nameof(Control));

        /// <summary>
        /// Identifies the <seealso cref="Triggers"/> avalonia property.
        /// </summary>
        public static readonly DirectProperty<ResponsiveControlBehavior, AvaloniaList<ResponsiveTrigger>?> TriggersProperty = 
            AvaloniaProperty.RegisterDirect<ResponsiveControlBehavior, AvaloniaList<ResponsiveTrigger>?>(nameof(Triggers), t => t._triggers);

        /// <summary>
        /// Gets or sets the target control that class name that should be added or removed when triggered. This is a avalonia property.
        /// </summary>
        public Control? Control
        {
            get => GetValue(ControlProperty);
            set => SetValue(ControlProperty, value);
        }

        /// <summary>
        /// Gets responsive triggers collection. This is a avalonia property.
        /// </summary>
        [Content]
        public AvaloniaList<ResponsiveTrigger>? Triggers => _triggers ??= new AvaloniaList<ResponsiveTrigger>();

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is { })
            {
                AssociatedObject.AttachedToVisualTree += AttachedToVisualTree;
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject is { })
            {
                AssociatedObject.AttachedToVisualTree -= AttachedToVisualTree;
            }

            _disposable?.Dispose();
        }

        private void AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            var target = GetValue(ControlProperty) is { } ? Control : AssociatedObject;
            var triggers = Triggers;

            if (target is not null && triggers is not null)
            {
                _disposable = ObserveBounds(target, triggers);
            }
        }

        private static IDisposable? ObserveBounds(Control target, AvaloniaList<ResponsiveTrigger> triggers)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));

            var data = target.GetObservable(Visual.BoundsProperty);
            return data.Subscribe(bounds =>
            {
                foreach (var trigger in triggers)
                {
                    var minValue = trigger.MinValue;
                    var maxValue = trigger.MaxValue;

                    var property = trigger.Property switch
                    {
                        ResponsiveBoundsProperty.Width => bounds.Width,
                        ResponsiveBoundsProperty.Height => bounds.Height,
                        _ => throw new Exception("Invalid Bounds property.")
                    };

                    var enabled = 
                        GetResult(trigger.MinOperator, property, minValue)
                        && GetResult(trigger.MaxOperator, property, maxValue);

                    var className = trigger.ClassName;

                    if (enabled)
                    {
                        if (!string.IsNullOrEmpty(className) && !target.Classes.Contains(className))
                        {
                            target.Classes.Add(className);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(className) && target.Classes.Contains(className))
                        {
                            target.Classes.Remove(className);
                        }
                    }
                }
            });
        }

        private static bool GetResult(ComparisonConditionType comparisonConditionType, double property, double value)
        {
            return comparisonConditionType switch
            {
                ComparisonConditionType.Equal => property == value,
                ComparisonConditionType.NotEqual => property != value,
                ComparisonConditionType.LessThan => property < value,
                ComparisonConditionType.LessThanOrEqual => property <= value,
                ComparisonConditionType.GreaterThan => property > value,
                ComparisonConditionType.GreaterThanOrEqual => property >= value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}