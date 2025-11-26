using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Converters;

/// <summary>
/// An type alias registry.
/// </summary>
public class AliasRegistry
{
    /* Private properties. */
    private Dictionary<Type, string> typeToAlias = new();
    private Dictionary<string, Type> aliasToType = new();

    /* Public methods. */
    /// <summary>
    /// Register a converter type for some target type.
    /// </summary>
    public void Add<T>(string alias)
    {
        Add(typeof(T), alias);
    }

    /// <summary>
    /// Register a type with some alias.
    /// </summary>
    public void Add(Type type, string alias)
    {
        // Don't allow duplicate aliasses.
        if (aliasToType.ContainsKey(alias) && aliasToType[alias] != type)
        {
            throw new ArgumentException($"Cannot add type '{alias}' for type '{type}', as it's already used by type "
                + $"'{aliasToType[alias]}'");
        }

        // Remove the type's old alias (if it existed).
        if (typeToAlias.ContainsKey(type))
            Remove(type);

        // Add the type-alias pair.
        typeToAlias[type] = alias;
        aliasToType[alias] = type;
    }

    /// <summary>
    /// Remove a type and its alias.
    /// </summary>
    public void Remove(Type type)
    {
        string alias = typeToAlias[type];
        typeToAlias.Remove(type);
        aliasToType.Remove(alias);
    }

    /// <summary>
    /// Check if an alias exists in the registry.
    /// </summary>
    public bool Has(string alias)
    {
        return aliasToType.ContainsKey(alias);
    }

    /// <summary>
    /// Check if a type exists in the registry.
    /// </summary>
    public bool Has(Type type)
    {
        return typeToAlias.ContainsKey(type);
    }

    /// <summary>
    /// Get the type of some alias.
    /// </summary>
    public Type Get(string alias)
    {
        return aliasToType[alias];
    }

    /// <summary>
    /// Get the alias of some type.
    /// </summary>
    public string Get(Type type)
    {
        return typeToAlias[type];
    }
}