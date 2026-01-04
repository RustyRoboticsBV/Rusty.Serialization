#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Unity;

namespace Rusty.Serialization
{
    public partial class DefaultConversionContext : ConversionContext
    {
        private void AddUnity()
        {
            // Structs.
            Converters.Add<LayerMask, LayerMaskConverter>();

            Converters.Add<Color, ColorConverter>();
            Converters.Add<Color32, Color32Converter>();

            Converters.Add<RangeInt, RangeIntConverter>();
            Converters.Add<Vector2, Vector2Converter>();
            Converters.Add<Vector3, Vector3Converter>();
            Converters.Add<Vector4, Vector4Converter>();
            Converters.Add<Vector2Int, Vector2IntConverter>();
            Converters.Add<Vector3Int, Vector3IntConverter>();
            Converters.Add<Quaternion, QuaternionConverter>();
            Converters.Add<Plane, PlaneConverter>();
            Converters.Add<Rect, RectConverter>();
            Converters.Add<RectInt, RectIntConverter>();
            Converters.Add<Bounds, BoundsConverter>();
            Converters.Add<BoundsInt, BoundsIntConverter>();
            Converters.Add<Matrix4x4, Matrix4x4Converter>();
            Converters.Add<BoundingSphere, BoundingSphereConverter>();
            Converters.Add<Ray, RayConverter>();
            Converters.Add<Ray2D, Ray2DConverter>();
            Converters.Add<Keyframe, ClassConverter<Keyframe>>();
            Converters.Add<AnimationCurve, ClassConverter<AnimationCurve>>();
        }
    }
}
#endif