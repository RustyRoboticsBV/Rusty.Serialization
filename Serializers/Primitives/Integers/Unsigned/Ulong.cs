namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for unsigned, long integers.
/// </summary>
public struct Ulong(ulong value) : ISerializer<ulong>
{
    /* Public properties. */
    public readonly string TypeCode => "ul";
    public ulong Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator ulong(Ulong value) => value.Value;
    public static implicit operator Ulong(ulong value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        text = StringParser.Parse(text);
        if (ulong.TryParse(text, out ulong result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0UL;
    }
}