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
        protected override StringNode ConvertRef(StringName obj, IConverterScheme scheme) => new(obj);
        protected override StringName DeconvertRef(StringNode node, IConverterScheme scheme) => node.Value;
    }
}
#endif