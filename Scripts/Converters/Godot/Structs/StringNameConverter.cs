#if GODOT
using Godot;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot string name converter.
    /// </summary>
    public class StringNameConverter : Converter<StringName, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode(StringName obj, CreateNodeContext context)
        {
            return new StringNode(obj);
        }

        protected override StringName CreateObject(StringNode node, CreateObjectContext context)
        {
            return new StringName(node.Value);
        }
    }
}
#endif