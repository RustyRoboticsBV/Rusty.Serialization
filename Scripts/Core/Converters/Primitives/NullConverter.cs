using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
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