#if GODOT
using Godot;
using Rusty.Serialization.Core.Converters;
using System.Collections.Generic;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Resource converter.
    /// </summary>
    public sealed class ResourceConverter<T> : ClassConverter<T>
        where T : Resource, new()
    {
        /* Protected methods. */
        protected override HashSet<string> IgnoredMembers => [
            "ResourceLocalToScene",
            "ResourcePath",
            "ResourceName",
            "ResourceSceneUniqueId"
        ];
    }
}
#endif