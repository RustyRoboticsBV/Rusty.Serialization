using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A string converter.
    /// </summary>
    public sealed class StringConverter : ReferenceConverter<string, StringNode>
    {
        /* Protected methods. */
        protected override StringNode Convert(string obj, Context context) => new(obj);
        protected override string Deconvert(StringNode node, Context context) => node.Value;
    }
}