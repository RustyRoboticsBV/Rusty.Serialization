#if GODOT
using Godot;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot resource path converter.
    /// </summary>
    public class ResourcePathConverter : Converter<Resource, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode(Resource obj, CreateNodeContext context)
        {
            return new StringNode(obj.ResourcePath);
        }

        protected override Resource CreateObject(StringNode node, CreateObjectContext context)
        {
            return ResourceLoader.Load(node.Value);
        }
    }
}
#endif