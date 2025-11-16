#if GODOT
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for colors.
/// </summary>
public struct Dictionary<T, U>(IDictionary<T, U> value) : ISerializer<IDictionary<T, U>>
    where T : ISerializer, new()
    where U : ISerializer, new()
{
    /* Public properties. */
    public readonly string TypeCode => '{' + $"{new T().TypeCode},{new U().TypeCode}" + '}';
    public IDictionary<T, U> Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator System.Collections.Generic.Dictionary<T, U>(Dictionary<T, U> value)
    {
        return (System.Collections.Generic.Dictionary<T, U>)value.Value;
    }

    public static implicit operator Dictionary<T, U>(System.Collections.Generic.Dictionary<T, U> value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => DictionaryWriter.Serialize(Value);

    public void Deserialize(string text)
    {
        Clear();

        List<(string, string)> dict = DictionaryParser.Parse(text);
        foreach ((string, string) pair in dict)
        {
            T key = new();
            key.Deserialize(pair.Item1);

            U value = new();
            value.Deserialize(pair.Item2);

            Value.Add(key, value);
        }
    }

    public void Clear()
    {
        Value = null;
    }
}
#endif