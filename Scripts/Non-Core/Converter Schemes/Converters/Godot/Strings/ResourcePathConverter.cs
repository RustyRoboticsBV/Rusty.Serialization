#if GODOT
using Godot;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Resource converter.
    /// </summary>
    public sealed class ResourcePathConverter<T> : ReferenceConverter<T, StringNode>
        where T : Resource
    {
        /* Protected methods. */
        protected override StringNode ConvertRef(T obj, IConverterScheme scheme, SymbolTable table)
        {
            return new(obj.ResourcePath);
        }

        protected override T DeconvertRef(StringNode node, IConverterScheme scheme, NodeTree tree)
        {
            return (T)ResourceLoader.Load(node.Value);
        }
    }
}
#endif