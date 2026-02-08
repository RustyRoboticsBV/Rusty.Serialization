#if GODOT
using Godot;
using Rusty.Serialization.Core.Conversion;
using System.Collections.Generic;
using System.Reflection;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot resource converter.
    /// </summary>
    public class ResourceConverter<T> : ClassConverter<T>
        where T : Resource
    {
        /* Protected properties. */
        protected override HashSet<MemberInfo> IgnoredMembers => new HashSet<MemberInfo>
        {
            typeof(Resource).GetProperty("ResourceLocalToScene"),
            typeof(Resource).GetProperty("ResourcePath"),
            typeof(Resource).GetProperty("ResourceName"),
            typeof(Resource).GetProperty("ResourceSceneUniqueId")
        };
    }
}
#endif