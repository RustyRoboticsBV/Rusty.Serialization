#if GODOT
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rusty.Serialization.Gd
{
    internal static class BuiltInResourceTypes
    {
        /* Private properties. */
        private static readonly Assembly EngineAssembly = typeof(Node).Assembly;

        /* Public methods. */
        public static IEnumerable<Type> GetBuiltInResourceTypes()
        {
            foreach (var type in EngineAssembly.GetTypes())
            {
                if (!typeof(Resource).IsAssignableFrom(type))
                    continue;

                if (type.IsAbstract)
                    continue;

                yield return type;
            }
        }
    }
}
#endif