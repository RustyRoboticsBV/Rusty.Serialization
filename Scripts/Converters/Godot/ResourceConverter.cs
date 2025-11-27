#if GODOT
using Godot;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters.Gd;

/// <summary>
/// A Godot.Resource converter.
/// </summary>
public sealed class ResourceConverter : ReferenceConverter<Resource, StringNode>
{
    /* Protected methods. */
    protected override StringNode Convert(Resource obj, Context context)
    {
        return new(obj.ResourcePath);
    }

    protected override Resource Deconvert(StringNode node, Context context)
    {
        return ResourceLoader.Load(node.Value);
    }
}
#endif