namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for decimals.
/// </summary>
public struct Decimal(decimal value) : ISerializer<decimal>
{
    /* Public properties. */
    public readonly string TypeCode => "dec";
    public decimal Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator decimal(Decimal value) => value.Value;
    public static implicit operator Decimal(decimal value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => Value.ToString();

    public void Deserialize(string text)
    {
        if (decimal.TryParse(text, out decimal result))
            Value = result;
    }

    public void Clear()
    {
        Value = 0;
    }
}