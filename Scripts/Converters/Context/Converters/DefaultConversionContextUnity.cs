#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Unity;

namespace Rusty.Serialization
{
    public partial class DefaultConverters : Converters
    {
        /// <summary>
        /// Add the Unity type converters.
        /// </summary>
        private void AddUnity()
        {
            // Structs.
            ConverterRegistry.Add<LayerMask, LayerMaskConverter>();

            ConverterRegistry.Add<Color, ColorConverter>();
            ConverterRegistry.Add<Color32, Color32Converter>();

            ConverterRegistry.Add<RangeInt, RangeIntConverter>();
            ConverterRegistry.Add<Vector2, Vector2Converter>();
            ConverterRegistry.Add<Vector3, Vector3Converter>();
            ConverterRegistry.Add<Vector4, Vector4Converter>();
            ConverterRegistry.Add<Vector2Int, Vector2IntConverter>();
            ConverterRegistry.Add<Vector3Int, Vector3IntConverter>();
            ConverterRegistry.Add<Quaternion, QuaternionConverter>();
            ConverterRegistry.Add<Plane, PlaneConverter>();
            ConverterRegistry.Add<Rect, RectConverter>();
            ConverterRegistry.Add<RectInt, RectIntConverter>();
            ConverterRegistry.Add<Bounds, BoundsConverter>();
            ConverterRegistry.Add<BoundsInt, BoundsIntConverter>();
            ConverterRegistry.Add<Matrix4x4, Matrix4x4Converter>();
            ConverterRegistry.Add<BoundingSphere, BoundingSphereConverter>();
            ConverterRegistry.Add<Ray, RayConverter>();
            ConverterRegistry.Add<Ray2D, Ray2DConverter>();
            ConverterRegistry.Add<Keyframe, ClassConverter<Keyframe>>();
            ConverterRegistry.Add<AnimationCurve, ClassConverter<AnimationCurve>>();
        }
    }
}
#endif