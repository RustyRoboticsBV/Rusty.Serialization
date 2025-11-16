#if GODOT
namespace Rusty.Serialization;

/// <summary>
/// A serializer/deserializer for colors.
/// </summary>
public struct Color(Godot.Color value) : ISerializer<Godot.Color>
{
    /* Public properties. */
    public readonly string TypeCode => "#";
    public Godot.Color Value { readonly get; set; } = value;

    /* Casting operators. */
    public static implicit operator Godot.Color(Color value) => value.Value;
    public static implicit operator Color(Godot.Color value) => new(value);

    /* Public methods. */
    public readonly string Serialize() => $"#{Value.ToHtml(Value.A < 1f)}";

    public void Deserialize(string text)
    {
        try
        {
            Value = Godot.Color.FromHtml(text);
        }
        catch
        {
            Clear();
        }
    }

    public void Clear()
    {
        Value = new(0, 0, 0, 0);
    }
}
#endif