namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for short integers.
/// </summary>
public struct Short(short value) : ISerializer<short>
{
    /* Public properties. */
    public readonly string TypeCode => "si";
    public short Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator short(Short value) => value.Value;
    public static implicit operator Short(short value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        text = StringParser.Parse(text);
        if (short.TryParse(text, out short result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0;
    }
}