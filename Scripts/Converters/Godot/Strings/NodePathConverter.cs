#if GODOT
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A Godot.NodePath converter.
    /// </summary>
    public sealed class NodePathConverter : ReferenceConverter<NodePath, StringNode>
    {
        /* Protected methods. */
        protected override StringNode Convert(NodePath obj, Context context) => new(obj);
        protected override NodePath Deconvert(StringNode node, Context context) => node.Value;
    }
}
#endif