#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using System.Collections.Generic;
using Rusty.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public List<int> intList = new();
}

/// <summary>
/// A test monobehavior.
/// </summary>
public class SerializeTest : MonoBehaviour
{
    private enum PerformanceTest { Off, Serialize, Parse, RoundTrip }

    [SerializeField] public Format Format = Format.Cscd;
    [SerializeField] public bool PrettyPrint;
    [SerializeField] PerformanceTest TestPerformance;
    [SerializeField] public StructTest Object;
    [SerializeField, Multiline(100)] public string Serialized;
}

#if UNITY_EDITOR
/// <summary>
/// The editor for SerializeTest.
/// </summary>
[CustomEditor(typeof(SerializeTest))]
public class SerializerTestEditor : Editor
{
    /* Fields. */
    private DefaultContext cscd = new(Format.Cscd);
    private DefaultContext json = null;// new(Format.Json);
    private DefaultContext xml = null;// new(Format.Xml);

    /* Unity events. */
    public override void OnInspectorGUI()
    {
        SerializeTest t = (SerializeTest)target;

        DefaultContext context = GetContext(t.Format);

        // Draw buttons.
        if (GUILayout.Button("Serialize"))
        {
            string serialized = context.Serialize(t.Object, t.PrettyPrint);
            t.Serialized = serialized;
        }
        if (GUILayout.Button("Deserialize"))
        {
            if (t.Serialized == "")
                Debug.LogError("Serialized text field is empty!! This is not a serializer bug!");
            else
            {
                var obj = context.Parse<StructTest>(t.Serialized);
                t.Object = obj;
            }
        }
        if (GUILayout.Button("Clear Object"))
            t.Object = new();
        if (GUILayout.Button("Clear Text"))
            t.Serialized = "";
        GUILayout.Space(10);

        // Draw normal inspector.
        DrawDefaultInspector();
    }

    private DefaultContext GetContext(Format format)
    {
        switch (format)
        {
            case Format.Cscd:
                return cscd;
            case Format.Json:
                return json;
            case Format.Xml:
                return xml;
            default:
                throw new Exception();
        }
    }
}
#endif
#endif