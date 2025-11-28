#if GODOT
using Godot;
using Rusty.Serialization.Core.Contexts;
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
        protected override StringNode Convert(NodePath obj, Context context) => new(obj);
        protected override NodePath Deconvert(StringNode node, Context context) => node.Value;
    }
}
#endif