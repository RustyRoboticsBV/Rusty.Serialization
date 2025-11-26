using System;
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A minimal bi-directional dictionary.
/// </summary>
internal class BiDictionary<KeyT, ValueT>
{
    /* Fields. */
    private readonly Dictionary<KeyT, ValueT> forward = new();
    private readonly Dictionary<ValueT, KeyT> reverse = new();

    /* Public indexers. */
    public ValueT this[KeyT key]
    {
        get => forward[key];
        set => AddOrUpdate(key, value);
    }
    public KeyT this[ValueT val] => reverse[val];

    /* Public methods. */
    public bool HasKey(KeyT key) => forward.ContainsKey(key);
    public bool HasValue(ValueT value) => reverse.ContainsKey(value);

    /* Private methods. */
    private void AddOrUpdate(KeyT key, ValueT value)
    {
        // If key already exists, remove old reverse mapping.
        if (forward.TryGetValue(key, out var existingValue))
        {
            if (EqualityComparer<ValueT>.Default.Equals(existingValue, value))
                return;

            reverse.Remove(existingValue);
        }

        // If value is already associated with a different key, treat as error.
        if (reverse.TryGetValue(value, out var existingKey) &&
            !EqualityComparer<KeyT>.Default.Equals(existingKey, key))
        {
            throw new ArgumentException($"Duplicate name '{value}'.");
        }

        // Set key-value and value-key pairs.
        forward[key] = value;
        reverse[value] = key;
    }
}