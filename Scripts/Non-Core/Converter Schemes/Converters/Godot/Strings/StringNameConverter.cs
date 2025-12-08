#if GODOT
using Godot;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.StringName converter.
    /// </summary>
    public sealed class StringNameConverter : ReferenceConverter<StringName, StringNode>
    {
        /* Protected methods. */
        protected override StringNode CreateNode(StringName obj, IConverterScheme scheme, SymbolTable table) => new(obj);
        protected override StringName DeconvertRef(StringNode node, IConverterScheme scheme, NodeTree tree) => node.Value;
    }
}
#endif