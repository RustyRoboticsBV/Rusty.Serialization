#if GODOT
using Godot;
using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Resource converter.
    /// </summary>
    public sealed class ResourceConverter : ReferenceConverter<Resource, StringNode>
    {
        /* Protected methods. */
        protected override StringNode ConvertRef(Resource obj, IConverterScheme scheme)
        {
            return new(obj.ResourcePath);
        }

        protected override Resource DeconvertRef(StringNode node, IConverterScheme scheme)
        {
            return ResourceLoader.Load(node.Value);
        }
    }
}
#endif