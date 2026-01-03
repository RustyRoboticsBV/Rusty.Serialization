#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Unity;

namespace Rusty.Serialization
{
    public partial class DefaultConversionContext : ConversionContext
    {
        private void AddUnity()
        {
            // Structs.
            Converters.Add<Vector2, Vector2Converter>();
            Converters.Add<Vector3, Vector3Converter>();
            Converters.Add<Vector4, Vector4Converter>();
        }
    }
}
#endif