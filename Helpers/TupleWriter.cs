namespace Rusty.Serialization;

/// <summary>
/// A helper class for serializing tuples.
/// </summary>
internal static class TupleWriter
{
    /// <summary>
    /// Serialize a tuple.
    /// </summary>
    public static string Serialize<T, U>((T, U) tuple)
        where T : ISerializer
        where U : ISerializer
    {
        return $"({tuple.Item1.Serialize()},{tuple.Item2.Serialize()})";
    }
}