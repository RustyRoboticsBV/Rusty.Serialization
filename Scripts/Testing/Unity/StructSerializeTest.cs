#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Rusty.Serialization.Testing
{
    [Serializable]
    public class StructTest
    {
        public bool @bool;

        public char chr = ' ';
        public string str = "";

        public enum Cheese { Leerdammer, BlueViking, Camembert, Tenenkaas }
        public Cheese cheese;

        public LayerMask layerMask;

        [Serializable]
        public class Scalars
        {
            public sbyte i8;
            public short i16;
            public int i32;
            public long i64;
            public byte u8;
            public ushort u16;
            public uint u32;
            public ulong u64;
            public float f32;
            public double f64;
            public byte[] bytes;

            public float nan = float.NaN;
            public double inf = double.PositiveInfinity;
            public double ninf = double.NegativeInfinity;

            public decimal dec = 1.00m;
        }
        public Scalars scalars = new();

        [Serializable]
        public class Vectors
        {
            public Vector2 vec2;
            public Vector3 vec3;
            public Vector4 vec4;
            public Vector2Int vec2i;
            public Vector3Int vec3i;
            public Quaternion q = Quaternion.identity;
            public Rect rect;
            public RectInt recti;
            public Bounds bounds;
            public BoundsInt boundsInt;
            public Matrix4x4 matrix;
        }
        public Vectors vectors = new();

        [Serializable]
        public class Colors
        {
            public Color color = new(0, 0, 0, 1);
            public Color32 color32 = new(0, 0, 0, 255);
        }
        public Colors color = new();

        public DateTime time = new DateTime(1994, 2, 13, 10, 5, 3, 77) + TimeSpan.FromTicks(1);

        public List<int> intList = new();
        public Dictionary<string, object> dict = new()
        {
            { "a", 0 },
            { "b", new DateTime(2000, 1, 2, 0, 0, 0) },
            { "c", Color.cyan }
        };
    }

    /// <summary>
    /// A struct serialization test monobehavior.
    /// </summary>
    public class StructSerializeTest : SerializeTest<StructTest> { }

#if UNITY_EDITOR
    /// <summary>
    /// The editor for StructSerializeTest.
    /// </summary>
    [CustomEditor(typeof(StructSerializeTest))]
    public class SerializerTestEditor : SerializerTestEditor<StructSerializeTest, StructTest> { }
}
#endif
#endif