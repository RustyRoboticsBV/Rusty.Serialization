#if UNITY_5_OR_NEWER
using System;
using UnityEngine;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Color serializer.
    /// </summary>
    public readonly struct ColorSerializer : ISerializer<Color>
    {
        /* Public methods. */
        public INode Serialize(Color value, Registry context)
        {
            byte r = (byte)Mathf.RoundToInt(value.r * 255f);
            byte g = (byte)Mathf.RoundToInt(value.g * 255f);
            byte b = (byte)Mathf.RoundToInt(value.b * 255f);
            byte a = (byte)Mathf.RoundToInt(value.a * 255f);
            return new ColorNode(r, g, b, a);
        }

        public Color Deserialize(INode node, Registry context)
        {
            if (node is ColorNode color)
            {
                return new Color(
                    color.R / 255f,
                    color.G / 255f,
                    color.B / 255f,
                    color.A / 255f
                );
            }

            throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
        }
    }
}
#endif