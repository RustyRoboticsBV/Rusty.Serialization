namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for unsigned integers.
/// </summary>
public struct Uint(uint value) : ISerializer<uint>
{
    /* Public properties. */
    public readonly string TypeCode => "ui";
    public uint Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator uint(Uint value) => value.Value;
    public static implicit operator Uint(uint value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        text = StringParser.Parse(text);
        if (uint.TryParse(text, out uint result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0u;
    }
}