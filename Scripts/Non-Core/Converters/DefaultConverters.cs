using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using Rusty.Serialization.Converters.System;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A default converter scheme, which contains support for all built-in C# types, various .NET types from the System
    /// namespace, and various Godot or Unity Engine types when compiled in one of those two contexts.
    /// </summary>
    public class DefaultConverters : ConverterScheme
    {
        public DefaultConverters()
        {
            // Int types.
            Add<BigInteger, BigIntegerConverter>("ibig");
            Add<Index, IndexConverter>("idx");

            // Real types.
            Add<Half, HalfConverter>("f16");

            // String types.
            Add<StringBuilder, StringBuilderConverter>("sb");
            Add<Uri, UriConverter>("uri");
            Add<Version, VersionConverter>("ver");

#if GODOT
            Add<Godot.StringName, Gd.StringNameConverter>("GDsname");
            Add<Godot.NodePath, Gd.NodePathConverter>("GDnpath");
#endif

            // Time types.
            Add<TimeSpan, TimeSpanConverter>("ts");
            Add<DateTime, DateTimeConverter>("dt");

#if NET6_0_OR_GREATER
            Add<DateOnly, DateOnlyConverter>("date");
            Add<TimeOnly, TimeOnlyConverter>("time");
#endif

            // Binary types.
            Add<Guid, GuidConverter>("guid");

            // Color types.
            Add<Color, Converters.System.ColorConverter>("col");

#if GODOT
            Add<Godot.Color, Gd.ColorConverter>("GDcol");
#endif

#if UNITY_5_OR_NEWER
            Add<UnityEngine.Color, Converters.Unity.ColorConverter>("UNcol");
            Add<UnityEngine.Color32, Converters.Unity.ColorConverter>("UNcol32");
#endif

            // List types.
            Add<Range, RangeConverter>("rng");

            Add(typeof(List<>), typeof(ListConverter<>), "list");
            Add(typeof(LinkedList<>), typeof(LinkedListConverter<>), "lnls");
            Add(typeof(HashSet<>), typeof(HashSetConverter<>), "hset");
            Add(typeof(Stack<>), typeof(StackConverter<>), "stck");
            Add(typeof(Queue<>), typeof(QueueConverter<>), "queu");

            Add(typeof(Point), typeof(System.PointConverter), "p2i");
            Add(typeof(PointF), typeof(PointFConverter), "p2f");
            Add(typeof(Size), typeof(System.SizeConverter), "s2i");
            Add(typeof(SizeF), typeof(System.SizeFConverter), "s2f");
            Add(typeof(Rectangle), typeof(System.RectangleConverter), "r2i");
            Add(typeof(RectangleF), typeof(RectangleFConverter), "r2f");

            Add(typeof(Complex), typeof(ComplexConverter), "cpx");
            Add(typeof(Vector2), typeof(Vector2Converter), "v2f");
            Add(typeof(Vector3), typeof(Vector3Converter), "v3f");
            Add(typeof(Vector4), typeof(Vector4Converter), "v4f");
            Add(typeof(Quaternion), typeof(QuaternionConverter), "quat");
            Add(typeof(Plane), typeof(PlaneConverter), "pln");
            Add(typeof(Matrix3x2), typeof(Matrix3x2Converter), "m3x2");
            Add(typeof(Matrix4x4), typeof(Matrix4x4Converter), "m4x4");

#if GODOT
            Add(typeof(Godot.Collections.Array), typeof(Gd.ArrayConverter), "GDarr");
            Add(typeof(Godot.Collections.Array<>), typeof(Gd.ArrayConverter<>), "GDarrT");
            Add<Godot.Vector2, Gd.Vector2Converter>("GDv2f");
            Add<Godot.Vector3, Gd.Vector3Converter>("GDv3f");
            Add<Godot.Vector4, Gd.Vector4Converter>("GDv4f");
            Add<Godot.Vector2I, Gd.Vector2IConverter>("GDv2i");
            Add<Godot.Vector3I, Gd.Vector3IConverter>("GDv3i");
            Add<Godot.Vector4I, Gd.Vector4IConverter>("GDv4i");
            Add<Godot.Quaternion, Gd.QuaternionConverter>("GDquat");
            Add<Godot.Plane, Gd.PlaneConverter>("GDpln");
            Add<Godot.Rect2, Gd.Rect2Converter>("GDr2f");
            Add<Godot.Rect2I, Gd.Rect2IConverter>("GDr2i");
            Add<Godot.Aabb, Gd.AabbConverter>("GDaabb");
            Add<Godot.Transform2D, Gd.Transform2DConverter>("GDm2x2");
            Add<Godot.Basis, Gd.BasisConverter>("GDm3x3");
            Add<Godot.Transform3D, Gd.Transform3DConverter>("GDm3x4");
            Add<Godot.Projection, Gd.ProjectionConverter>("GDm4x4");
#endif

            // Dictionary types.
            Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>), "dict");
            Add(typeof(KeyValuePair<,>), typeof(KeyValuePairConverter<,>), "kvp");

#if GODOT
            Add(typeof(Godot.Collections.Dictionary), typeof(Gd.DictionaryConverter), "GDdict");
            Add(typeof(Godot.Collections.Dictionary<,>), typeof(Gd.DictionaryConverter<,>), "GDdictT");
#endif

            // Object types.
#if GODOT
            Add(typeof(Godot.Variant), typeof(Gd.VariantConverter), "GDvar");
            Add(typeof(Godot.Resource), typeof(Gd.ResourceConverter), "GDres");
#endif
        }
    }
}