namespace Rusty.Serialization;

/// <summary>
/// A tuple serializer.
/// </summary>
public struct Tuple<T, U>((T, U) value) : ISerializer<(T, U)>
    where T : ISerializer, new()
    where U : ISerializer, new()
{
    /* Public properties. */
    public string TypeCode => $"({new T().TypeCode},{new U().TypeCode})";
    public (T, U) Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator (T, U)(Tuple<T, U> value) => value.Value;
    public static implicit operator Tuple<T, U>((T, U) value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => TupleWriter.Serialize(Value);

    public void Deserialize(string text)
    {
        (string, string) tuple = TupleParser.Parse(text);

        T item1 = new();
        item1.Deserialize(tuple.Item1);

        U item2 = new();
        item2.Deserialize(tuple.Item1);

        Value = (item1, item2);
    }

    public void Clear()
    {
        Value = (new(), new());
    }
}