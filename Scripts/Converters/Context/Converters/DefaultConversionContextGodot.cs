#if GODOT
using Godot;
using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Gd;

namespace Rusty.Serialization
{
    public partial class DefaultConversionContext : ConversionContext
    {
        /// <summary>
        /// Add the Godot type converters.
        /// </summary>
        private void AddGodot()
        {
            // Structs.
            Converters.Add<StringName, StringNameConverter>();
            Converters.Add<NodePath, NodePathConverter>();

            Converters.Add<Color, ColorConverter>();

            Converters.Add<Vector2, Vector2Converter>();
            Converters.Add<Vector3, Vector3Converter>();
            Converters.Add<Vector4, Vector4Converter>();
            Converters.Add<Vector2I, Vector2IConverter>();
            Converters.Add<Vector3I, Vector3IConverter>();
            Converters.Add<Vector4I, Vector4IConverter>();
            Converters.Add<Quaternion, QuaternionConverter>();
            Converters.Add<Plane, PlaneConverter>();
            Converters.Add<Rect2, Rect2Converter>();
            Converters.Add<Rect2I, Rect2IConverter>();
            Converters.Add<Aabb, AabbConverter>();
            Converters.Add<Transform2D, Transform2DConverter>();
            Converters.Add<Basis, BasisConverter>();
            Converters.Add<Transform3D, Transform3DConverter>();
            Converters.Add<Projection, Transform2DConverter>();

            // Built-in resources.
            var builtInResources = BuiltInResourceTypes.GetBuiltInResourceTypes();
            foreach (Type builtInResource in builtInResources)
            {
                Converters.Add(builtInResource, typeof(ResourcePathConverter));
            }

            // User-defined resources.
            Converters.Add<Resource, ClassConverter<Resource>>();
        }
    }
}
#endif