namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for strings.
/// </summary>
public struct String(string value) : ISerializer<string>
{
    /* Public properties. */
    public readonly string TypeCode => "s";
    public string Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator string(String value) => value.Value;
    public static implicit operator String(string value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => StringWriter.Serialize(Value);

    public void Deserialize(string text)
    {
        Value = StringParser.Parse(text) ?? "";
    }

    public void Clear()
    {
        Value = "";
    }
}