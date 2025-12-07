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
        protected override StringNode ConvertRef(NodePath obj, IConverterScheme scheme, NodeTree tree) => new(obj);
        protected override NodePath DeconvertRef(StringNode node, IConverterScheme scheme, NodeTree tree) => node.Value;
    }
}
#endif