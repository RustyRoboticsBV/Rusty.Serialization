using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// An array serializer.
/// </summary>
public struct Array<T>(T[] value) : ISerializer<T[]>
    where T : ISerializer, new()
{
    /* Public properties. */
    public string TypeCode => $"{new T().TypeCode}[]";
    public T[] Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator T[](Array<T> value) => value.Value;
    public static implicit operator Array<T>(T[] value) => new(value);
    public static implicit operator List<T>(Array<T> value) => new(value.Value);
    public static implicit operator Array<T>(List<T> value) => value.ToArray();

    /* Public methods. */
    public readonly string Serialize() => ListWriter.Serialize(Value);

    public void Deserialize(string text)
    {
        Clear();

        // Get cells.
        List<string> parts = ListParser.Parse(text);

        // Parse cells.
        T[] result = new T[parts.Count];
        for (int i = 0; i < parts.Count; i++)
        {
            result[i] = new T();
            result[i].Deserialize(parts[i]);
        }

        // Store parsed result.
        Value = result;
    }

    public void Clear()
    {
        Value = [];
    }
}
