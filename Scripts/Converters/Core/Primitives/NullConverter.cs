using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A null converter.
    /// </summary>
    public sealed class NullConverter : ReferenceConverter<object, NullNode>
    {
        /* Protected methods. */
        protected override NullNode Convert(object obj, Context context) => new();
        protected override object Deconvert(NullNode node, Context context) => null;
    }
}