namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for floats.
/// </summary>
public struct Float(float value) : ISerializer<float>
{
    /* Public properties. */
    public readonly string TypeCode => "f";
    public float Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator float(Float value) => value.Value;
    public static implicit operator Float(float value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        if (float.TryParse(text, out float result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0f;
    }
}