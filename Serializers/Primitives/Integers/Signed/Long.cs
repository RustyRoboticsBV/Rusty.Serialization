namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for long integers.
/// </summary>
public struct Long(long value) : ISerializer<long>
{
    /* Public properties. */
    public readonly string TypeCode => "li";
    public long Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator long(Long value) => value.Value;
    public static implicit operator Long(long value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        text = StringParser.Parse(text);
        if (long.TryParse(text, out long result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0L;
    }
}