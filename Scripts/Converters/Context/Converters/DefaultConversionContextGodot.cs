#if GODOT
using Godot;
using System;
using Rusty.Serialization.Core.Conversion;
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
            ConverterRegistry.Add<StringName, StringNameConverter>();
            ConverterRegistry.Add<NodePath, NodePathConverter>();

            ConverterRegistry.Add<Color, ColorConverter>();

            ConverterRegistry.Add<Vector2, Vector2Converter>();
            ConverterRegistry.Add<Vector3, Vector3Converter>();
            ConverterRegistry.Add<Vector4, Vector4Converter>();
            ConverterRegistry.Add<Vector2I, Vector2IConverter>();
            ConverterRegistry.Add<Vector3I, Vector3IConverter>();
            ConverterRegistry.Add<Vector4I, Vector4IConverter>();
            ConverterRegistry.Add<Quaternion, QuaternionConverter>();
            ConverterRegistry.Add<Plane, PlaneConverter>();
            ConverterRegistry.Add<Rect2, Rect2Converter>();
            ConverterRegistry.Add<Rect2I, Rect2IConverter>();
            ConverterRegistry.Add<Aabb, AabbConverter>();
            ConverterRegistry.Add<Transform2D, Transform2DConverter>();
            ConverterRegistry.Add<Basis, BasisConverter>();
            ConverterRegistry.Add<Transform3D, Transform3DConverter>();
            ConverterRegistry.Add<Projection, Transform2DConverter>();

            // Built-in resources.
            var builtInResources = BuiltInResourceTypes.GetBuiltInResourceTypes();
            foreach (Type builtInResource in builtInResources)
            {
                ConverterRegistry.Add(builtInResource, typeof(ResourcePathConverter));
            }

            // User-defined resources.
            ConverterRegistry.Add<Resource, ClassConverter<Resource>>();
        }
    }
}
#endif