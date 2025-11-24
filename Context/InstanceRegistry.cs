using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Converters;

/// <summary>
/// An target type to IConverter instance registry.
/// </summary>
public class InstanceRegistry
{
    /* Private properties. */
    private Dictionary<Type, IConverter> targetToConverter = new();

    /* Public methods. */
    /// <summary>
    /// Register a converter type for some target type.
    /// </summary>
    public void Add(Type targetType, IConverter converter)
    {
        targetToConverter[targetType] = converter;
    }

    /// <summary>
    /// Get a converter for some type if it exists. Returns null if it doesn't.
    /// </summary>
    public IConverter Get(Type targetType)
    {
        if (targetToConverter.TryGetValue(targetType, out IConverter converter))
            return converter;
        return null;
    }
}