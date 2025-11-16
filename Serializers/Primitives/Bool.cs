namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for booleans.
/// </summary>
public struct Bool(bool value) : ISerializer<bool>
{
    /* Public properties. */
    public readonly string TypeCode => "b";
    public bool Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator bool(Bool value) => value.Value;
    public static implicit operator Bool(bool value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        if (bool.TryParse(text, out bool result))
            Value = result;
    }

    public void Clear()
    {
        Value = false;
    }
}
