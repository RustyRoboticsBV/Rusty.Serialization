#if GODOT
using Godot;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot node path converter.
    /// </summary>
    public class NodePathConverter : Converter<NodePath, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode(NodePath obj, CreateNodeContext context)
        {
            return new StringNode(obj);
        }

        protected override NodePath CreateObject(StringNode node, CreateObjectContext context)
        {
            return new NodePath(node.Value);
        }
    }
}
#endif