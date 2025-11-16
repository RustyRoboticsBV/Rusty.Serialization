namespace Rusty.Serialization;

public interface ISerializer
{
    /* Public properties. */
    /// <summary>
    /// The type code of this serializer type.
    /// </summary>
    public string TypeCode { get; }
    /// <summary>
    /// The value of this object.
    /// </summary>
    public object Value { get; }

    /* Public methods. */
    /// <summary>
    /// Serialize this object.
    /// </summary>
    public string Serialize();

    /// <summary>
    /// Deserialize a string and overwrite this object's values.
    /// </summary>
    public void Deserialize(string text);

    /// <summary>
    /// Reset all values to their defaults.
    /// </summary>
    public void Clear();
}


public interface ISerializer<T> : ISerializer
{
    /* Public properties. */
    /// <summary>
    /// The value of this object.
    /// </summary>
    public new T Value { get; set; }
    object ISerializer.Value => Value;
}