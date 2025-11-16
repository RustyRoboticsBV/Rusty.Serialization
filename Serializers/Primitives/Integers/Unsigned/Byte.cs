namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for unsigned bytes.
/// </summary>
public struct Byte(byte value) : ISerializer<byte>
{
    /* Public properties. */
    public readonly string TypeCode => "by";
    public byte Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator byte(Byte value) => value.Value;
    public static implicit operator Byte(byte value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        text = StringParser.Parse(text);
        if (byte.TryParse(text, out byte result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0b0;
    }
}