namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for characters.
/// </summary>
public struct Char(char value) : ISerializer<char>
{
    /* Public properties. */
    public readonly string TypeCode => "b";
    public char Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator char(Char value) => value.Value;
    public static implicit operator Char(char value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => StringWriter.Serialize(Value.ToString());

    public void Deserialize(string text)
    {
        // CSV-unpack the input string.
        text = StringParser.Parse(text);

        // Handle empty strings.
        if (string.IsNullOrEmpty(text))
            Clear();
        else
            Value = text[0];
    }

    public void Clear()
    {
        Value = '\0';
    }
}
