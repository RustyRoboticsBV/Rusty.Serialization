namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for integers.
/// </summary>
public struct Int(int value) : ISerializer<int>
{
    /* Public properties. */
    public readonly string TypeCode => "i";
    public int Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator int(Int value) => value.Value;
    public static implicit operator Int(int value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        text = StringParser.Parse(text);
        if (int.TryParse(text, out int result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0;
    }
}
