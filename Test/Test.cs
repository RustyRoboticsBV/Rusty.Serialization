using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A test class.
/// </summary>
public sealed class Test
{
    // Primitives.
    public bool @bool = true;
    public sbyte @sbyte = -1;
    public byte @byte = 1;
    public short @short = -123;
    public ushort @ushort = 123;
    public int @int = -12345;
    public uint @uint = 67890;
    public float @float = 0.12345f;
    public double @double = 12345.67890;
    public decimal @decimal = 3.14m;
    public char @char = '"';
    public string @string = "abc";
#if GODOT
    public Godot.Color color = Godot.Colors.Red;
#elif UNITY_5_OR_NEWER
    public UnityEngine.Color color = Color.red;
    public UnityEngine.Color32 color32 = Color.red;
#endif

    // Lists.
    public int[] array = [1, 2, 3, 4, 5];
    public List<float> list = [1.1f, 2.2f, 3.3f, 4.4f, 5.5f];
#if GODOT
    public Godot.Vector2 vector2 = new(1.1f, 2.2f);
    public Godot.Vector3 vector3 = new(1.1f, 2.2f, 3.3f);
    public Godot.Vector4 vector4 = new(1.1f, 2.2f, 3.3f, 4.4f);
    public Godot.Vector2I vector2i = new(1, 2);
    public Godot.Vector3I vector3i = new(1, 2, 3);
    public Godot.Vector4I vector4i = new(1, 2, 3, 4);
    public Godot.Quaternion quaternion = Godot.Quaternion.Identity;
    public Godot.Plane plane = Godot.Plane.PlaneXY;
    public Godot.Rect2 rect2 = new(1, 2, 3, 4);
    public Godot.Rect2I rect2i = new(1, 2, 3, 4);
    public Godot.Aabb aabb = new(1, 2, 3, 4, 5, 6);
    public Godot.Transform2D transform2d = new(1, 2, 3, 4, 5, 6);
    public Godot.Basis basis = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
    public Godot.Transform3D transform3d = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
    public Godot.Projection projection = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
#endif

    // Dictionaries.
    public Dictionary<char, string> dictionary = new Dictionary<char, string>()
    {
        { 'a', "AAA" },
        { 'b', "BBB" },
        { 'c', "CCC" }
    };

    // Objects.
    public class Class
    {
        public int a = 0;
    }
    public Class @class = new();

    public struct Struct
    {
        public int a;
    }
    public Struct @struct = new();
}