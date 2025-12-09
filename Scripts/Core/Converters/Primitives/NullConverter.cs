using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A null converter.
    /// </summary>
    public sealed class NullConverter : ReferenceConverter<object, INode>
    {
        /* Protected methods. */
        protected override INode CreateNode(object obj, IConverterScheme scheme, SymbolTable table) => new NullNode();
        protected override object CreateObject(INode node, IConverterScheme scheme, NodeTree tree) => null;
    }
}