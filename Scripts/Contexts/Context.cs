#if GODOT && !UNITY_5_OR_NEWER
#define GODOT_CONTEXT

#elif !GODOT && UNITY_5_OR_NEWER
#define UNITY_CONTEXT
#endif

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using Rusty.Serialization.Converters.System;
using Rusty.Serialization.Core.Contexts;

namespace Rusty.Serialization
{
    /// <summary>
    /// A default serialization context that contains serializers for various .NET types.
    /// If used in a Godot context, it also adds serializers for various Godot types.
    /// </summary>
    public class Serializer : Context
    {
        /* Fields. */
        private static Serializer @default = new();

        /* Public properties. */
        /// <summary>
        /// A global, default serialization context.
        /// </summary>
        public static Serializer Default => @default;

        /* Constructors. */
        public Serializer() : base()
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

#if GODOT_CONTEXT
            Add<Godot.StringName, Converters.Gd.StringNameConverter>("GDsname");
            Add<Godot.NodePath, Converters.Gd.NodePathConverter>("GDnpath");
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

#if GODOT_CONTEXT
            Add<Godot.Color, Converters.Gd.ColorConverter>("GDcol");
#endif

#if UNITY_CONTEXT
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

            Add(typeof(Point), typeof(Converters.System.PointConverter), "p2i");
            Add(typeof(PointF), typeof(PointFConverter), "p2f");
            Add(typeof(Size), typeof(Converters.System.SizeConverter), "s2i");
            Add(typeof(SizeF), typeof(Converters.System.SizeFConverter), "s2f");
            Add(typeof(Rectangle), typeof(Converters.System.RectangleConverter), "r2i");
            Add(typeof(RectangleF), typeof(RectangleFConverter), "r2f");

            Add(typeof(Complex), typeof(ComplexConverter), "cpx");
            Add(typeof(Vector2), typeof(Vector2Converter), "v2f");
            Add(typeof(Vector3), typeof(Vector3Converter), "v3f");
            Add(typeof(Vector4), typeof(Vector4Converter), "v4f");
            Add(typeof(Quaternion), typeof(QuaternionConverter), "quat");
            Add(typeof(Plane), typeof(PlaneConverter), "pln");
            Add(typeof(Matrix3x2), typeof(Matrix3x2Converter), "m3x2");
            Add(typeof(Matrix4x4), typeof(Matrix4x4Converter), "m4x4");

#if GODOT_CONTEXT
            Add(typeof(Godot.Collections.Array), typeof(Converters.Gd.ArrayConverter), "GDarr");
            Add(typeof(Godot.Collections.Array<>), typeof(Converters.Gd.ArrayConverter<>), "GDarrT");
            Add<Godot.Vector2, Converters.Gd.Vector2Converter>("GDv2f");
            Add<Godot.Vector3, Converters.Gd.Vector3Converter>("GDv3f");
            Add<Godot.Vector4, Converters.Gd.Vector4Converter>("GDv4f");
            Add<Godot.Vector2I, Converters.Gd.Vector2IConverter>("GDv2i");
            Add<Godot.Vector3I, Converters.Gd.Vector3IConverter>("GDv3i");
            Add<Godot.Vector4I, Converters.Gd.Vector4IConverter>("GDv4i");
            Add<Godot.Quaternion, Converters.Gd.QuaternionConverter>("GDquat");
            Add<Godot.Plane, Converters.Gd.PlaneConverter>("GDpln");
            Add<Godot.Rect2, Converters.Gd.Rect2Converter>("GDr2f");
            Add<Godot.Rect2I, Converters.Gd.Rect2IConverter>("GDr2i");
            Add<Godot.Aabb, Converters.Gd.AabbConverter>("GDaabb");
            Add<Godot.Transform2D, Converters.Gd.Transform2DConverter>("GDm2x2");
            Add<Godot.Basis, Converters.Gd.BasisConverter>("GDm3x3");
            Add<Godot.Transform3D, Converters.Gd.Transform3DConverter>("GDm3x4");
            Add<Godot.Projection, Converters.Gd.ProjectionConverter>("GDm4x4");
#endif

            // Dictionary types.
            Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>), "dict");
            Add(typeof(KeyValuePair<,>), typeof(KeyValuePairConverter<,>), "kvp");

#if GODOT_CONTEXT
            Add(typeof(Godot.Collections.Dictionary), typeof(Converters.Gd.DictionaryConverter), "GDdict");
            Add(typeof(Godot.Collections.Dictionary<,>), typeof(Converters.Gd.DictionaryConverter<,>), "GDdictT");
#endif

            // Object types.
#if GODOT_CONTEXT
            Add(typeof(Godot.Variant), typeof(Converters.Gd.VariantConverter), "GDvar");
            Add(typeof(Godot.Resource), typeof(Converters.Gd.ResourceConverter), "GDres");
#endif
        }
    }
}
