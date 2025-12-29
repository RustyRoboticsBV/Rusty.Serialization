#if GODOT
using Godot;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Gd;
using System;

namespace Rusty.Serialization
{
    public partial class DefaultConversionContext : ConversionContext
    {
        private void AddGodot()
        {
            // Built-in resources.
            var builtInResources = BuiltInResourceTypes.GetBuiltInResourceTypes();
            foreach (Type builtInResource in builtInResources)
            {
                Converters.Add(builtInResource, typeof(ResourcePathConverter));
            }
        }
    }
}
#endif