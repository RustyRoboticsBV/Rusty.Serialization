using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A null converter.
    /// </summary>
    public sealed class NullConverter : ReferenceConverter<object, INode>
    {
        /* Protected methods. */
        protected override INode ConvertRef(object obj, IConverterScheme scheme, NodeTree tree) => new NullNode();
        protected override object DeconvertRef(INode node, IConverterScheme scheme, NodeTree tree) => null;
    }
}