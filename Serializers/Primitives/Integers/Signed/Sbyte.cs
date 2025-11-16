namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for signed bytes.
/// </summary>
public struct Sbyte(sbyte value) : ISerializer<sbyte>
{
    /* Public properties. */
    public readonly string TypeCode => "sb";
    public sbyte Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator sbyte(Sbyte value) => value.Value;
    public static implicit operator Sbyte(sbyte value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        text = StringParser.Parse(text);
        if (sbyte.TryParse(text, out sbyte result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0b0;
    }
}