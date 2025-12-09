#if GODOT
using Godot;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.NodePath converter.
    /// </summary>
    public sealed class NodePathConverter : ReferenceConverter<NodePath, StringNode>
    {
        /* Protected methods. */
        protected override StringNode CreateNode(NodePath obj, IConverterScheme scheme, SymbolTable table) => new(obj);
        protected override NodePath DeconvertRef(StringNode node, IConverterScheme scheme, ParsingTable table) => node.Value;
    }
}
#endif