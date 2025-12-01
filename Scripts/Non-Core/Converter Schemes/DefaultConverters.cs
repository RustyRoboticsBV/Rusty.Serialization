using System;
using System.Collections;
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
            // Null types.
            Add<DBNull, DBNullConverter>("db0");

            // Int types.
            Add<BigInteger, BigIntegerConverter>("ibig");
            Add<Index, IndexConverter>("idx");

            // Real types.
#if NET5_0_OR_GREATER
            Add<Half, HalfConverter>("f16");
#endif
            // Char types.
#if NET5_0_OR_GREATER
            Add<Rune, RuneConverter>("rune");
#endif

            // String types.
            Add<Type, TypeConverter>("type");
            Add<Uri, UriConverter>("uri");
            Add<Version, VersionConverter>("ver");

            Add<StringBuilder, StringBuilderConverter>("sb");
            Add<Encoding, EncodingConverter>("enc");

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

            Add<BitArray, BitArrayConverter>("barr");

            // Color types.
            Add<Color, System.ColorConverter>("col");

#if GODOT
            Add<Godot.Color, Gd.ColorConverter>("GDcol");
#endif

#if UNITY_5_3_OR_NEWER
            Add<UnityEngine.Color, Unity.ColorConverter>("UNcol");
            Add<UnityEngine.Color32, Unity.Color32Converter>("UNcol32");
#endif

            // List types.
            Add<Range, RangeConverter>("rng");
            Add<DateTimeOffset, DateTimeOffsetConverter>("dto");

            Add(typeof(List<>), typeof(ListConverter<>), "list");
            Add(typeof(LinkedList<>), typeof(LinkedListConverter<>), "lnls");
            Add(typeof(HashSet<>), typeof(HashSetConverter<>), "hset");
            Add(typeof(Stack<>), typeof(StackConverter<>), "stck");
            Add(typeof(Queue<>), typeof(QueueConverter<>), "queu");
            Add(typeof(SortedSet<>), typeof(SortedSetConverter<>), "sset");

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

#if UNITY_5_3_OR_NEWER
            Add<UnityEngine.LayerMask, Unity.LayerMaskConverter>("UNlmsk");
            Add<UnityEngine.RangeInt, Unity.RangeIntConverter>("UNrngi");
            Add<UnityEngine.Vector2, Unity.Vector2Converter>("UNv2f");
            Add<UnityEngine.Vector3, Unity.Vector3Converter>("UNv3f");
            Add<UnityEngine.Vector4, Unity.Vector4Converter>("UNv4f");
            Add<UnityEngine.Vector2Int, Unity.Vector2IntConverter>("UNv2i");
            Add<UnityEngine.Vector3Int, Unity.Vector3IntConverter>("UNv3i");
            Add<UnityEngine.Quaternion, Unity.QuaternionConverter>("UNquat");
            Add<UnityEngine.Plane, Unity.PlaneConverter>("UNpln");
            Add<UnityEngine.Rect, Unity.RectConverter>("UNr2f");
            Add<UnityEngine.RectInt, Unity.RectIntConverter>("UNr2i");
            Add<UnityEngine.Bounds, Unity.BoundsConverter>("UNr3f");
            Add<UnityEngine.BoundsInt, Unity.BoundsIntConverter>("UNr3i");
            Add<UnityEngine.Matrix4x4, Unity.Matrix4x4Converter>("UNm4x4");
            Add<UnityEngine.FrustumPlanes, Unity.FrustumPlanesConverter>("UNfrpl");
            Add<UnityEngine.BoundingSphere, Unity.BoundingSphereConverter>("UNsphr");
            Add<UnityEngine.Ray, Unity.RayConverter>("UNray3");
            Add<UnityEngine.Ray2D, Unity.Ray2DConverter>("UNray2");
#endif

            // Dictionary types.
            Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>), "dict");
            Add(typeof(SortedList<,>), typeof(SortedListConverter<,>), "slst");
            Add(typeof(SortedDictionary<,>), typeof(SortedDictionaryConverter<,>), "sdic");
            Add(typeof(KeyValuePair<,>), typeof(KeyValuePairConverter<,>), "kvp");
#if NET6_0_OR_GREATER
            Add(typeof(PriorityQueue<,>), typeof(PriorityQueueConverter<,>), "pque");
#endif

#if GODOT
            Add(typeof(Godot.Collections.Dictionary), typeof(Gd.DictionaryConverter), "GDdict");
            Add(typeof(Godot.Collections.Dictionary<,>), typeof(Gd.DictionaryConverter<,>), "GDdictT");
#endif

            // Object types.
#if UNITY_5_3_OR_NEWER
            Add<UnityEngine.Keyframe, StructConverter<UnityEngine.Keyframe>>("Ukeyf");
            Add<UnityEngine.AnimationCurve, ClassConverter<UnityEngine.AnimationCurve>>("Uacurv");
#endif

            // Resource handling.
#if GODOT
            Add(typeof(Godot.Variant), typeof(Gd.VariantConverter), "GDvar");
            Add(typeof(Godot.Resource), typeof(Gd.ResourceConverter<>), "GDres");
            Add(typeof(Godot.PackedScene), typeof(Gd.ResourcePathConverter<Godot.PackedScene>), "GDscn");
            Add(typeof(Godot.Sky), typeof(Gd.ResourcePathConverter<Godot.Sky>), "GDsky");
            Add(typeof(Godot.Environment), typeof(Gd.ResourcePathConverter<Godot.Environment>), "GDenv");
            Add(typeof(Godot.Texture), typeof(Gd.ResourcePathConverter<Godot.Texture>), "GDtex");
            Add(typeof(Godot.Image), typeof(Gd.ResourcePathConverter<Godot.Image>), "GDimg");
            Add(typeof(Godot.SpriteFrames), typeof(Gd.ResourcePathConverter<Godot.SpriteFrames>), "GDspf");
            Add(typeof(Godot.Material), typeof(Gd.ResourcePathConverter<Godot.Material>), "GDmat");
            Add(typeof(Godot.PhysicsMaterial), typeof(Gd.ResourcePathConverter<Godot.PhysicsMaterial>), "GDpmt");
            Add(typeof(Godot.Mesh), typeof(Gd.ResourcePathConverter<Godot.Mesh>), "GDmsh");
            Add(typeof(Godot.MeshLibrary), typeof(Gd.ResourcePathConverter<Godot.MeshLibrary>), "GDmlib");
            Add(typeof(Godot.NavigationMesh), typeof(Gd.ResourcePathConverter<Godot.NavigationMesh>), "GDnav");
            Add(typeof(Godot.Font), typeof(Gd.ResourcePathConverter<Godot.Font>), "GDfnt");
            Add(typeof(Godot.Theme), typeof(Gd.ResourcePathConverter<Godot.Theme>), "GDthm");
            Add(typeof(Godot.Animation), typeof(Gd.ResourcePathConverter<Godot.Animation>), "GDanm");
            Add(typeof(Godot.AnimationLibrary), typeof(Gd.ResourcePathConverter<Godot.AnimationLibrary>), "GDalib");
            Add(typeof(Godot.Skin), typeof(Gd.ResourcePathConverter<Godot.Skin>), "GDskin");
            Add(typeof(Godot.AudioStream), typeof(Gd.ResourcePathConverter<Godot.AudioStream>), "GDaud");
            Add(typeof(Godot.AudioBusLayout), typeof(Gd.ResourcePathConverter<Godot.AudioBusLayout>), "GDbus");
            Add(typeof(Godot.AudioEffect), typeof(Gd.ResourcePathConverter<Godot.AudioEffect>), "GDeff");
            Add(typeof(Godot.TileSet), typeof(Gd.ResourcePathConverter<Godot.TileSet>), "GDtset");
            Add(typeof(Godot.TileSetSource), typeof(Gd.ResourcePathConverter<Godot.TileSetSource>), "GDtsrc");
            Add(typeof(Godot.Script), typeof(Gd.ResourcePathConverter<Godot.Script>), "GDscr");
            Add(typeof(Godot.Shader), typeof(Gd.ResourcePathConverter<Godot.Shader>), "GDshd");
#endif
        }
    }
}