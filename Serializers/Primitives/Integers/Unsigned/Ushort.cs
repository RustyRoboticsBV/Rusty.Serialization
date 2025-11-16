namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for unsigned, short integers.
/// </summary>
public struct Ushort(ushort value) : ISerializer<ushort>
{
    /* Public properties. */
    public readonly string TypeCode => "ul";
    public ushort Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator ushort(Ushort value) => value.Value;
    public static implicit operator Ushort(ushort value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        text = StringParser.Parse(text);
        if (ushort.TryParse(text, out ushort result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0;
    }
}