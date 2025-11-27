#if GODOT
using Godot;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A Godot.StringName converter.
    /// </summary>
    public sealed class StringNameConverter : ReferenceConverter<StringName, StringNode>
    {
        /* Protected methods. */
        protected override StringNode Convert(StringName obj, Context context) => new(obj);
        protected override StringName Deconvert(StringNode node, Context context) => node.Value;
    }
}
#endif