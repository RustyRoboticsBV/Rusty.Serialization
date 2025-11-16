namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for double-precision floats.
/// </summary>
public struct Double(double value) : ISerializer<double>
{
    /* Public properties. */
    public readonly string TypeCode => "d";
    public double Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator double(Double value) => value.Value;
    public static implicit operator Double(double value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        if (double.TryParse(text, out double result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0.0;
    }
}