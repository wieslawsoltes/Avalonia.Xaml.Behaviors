using System;
using System.Collections.Generic;

namespace BehaviorsTestApplication.Extensions;

public enum Spacing
{
    None,
    Tiny,
    Small,
    Medium,
    Large,
    XLarge
}

public class SpacingExtension
{
    private static readonly Dictionary<Spacing, double> Instance = new()
    {
        [Spacing.None] = 0,
        [Spacing.Tiny] = 4,
        [Spacing.Small] = 8,
        [Spacing.Medium] = 16,
        [Spacing.Large] = 24,
        [Spacing.XLarge] = 32
    };

    public Spacing Key { get; set; }

    public SpacingExtension()
    {
    }

    public SpacingExtension(Spacing key)
    {
        Key = key;
    }

    public double ProvideValue(IServiceProvider serviceProvider) => Instance[Key];
}
