using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A test class.
/// </summary>
public sealed class TestClass
{
    public bool a = false;
    public sbyte b1 = -1;
    public byte b2 = 1;
    public short b3 = -123;
    public ushort b4 = 123;
    public int b5 = -12345;
    public uint b6 = 67890;
    public float c1 = 0.12345f;
    public double c2 = 12345.67890;
    public decimal c3 = 3.14m;
    public char d = '"';
    public string e = "abc";
#if GODOT
    public Godot.Color f = Godot.Colors.Red;
#elif UNITY_5_OR_NEWER
    public UnityEngine.Color f1 = Color.red;
    public UnityEngine.Color32 f2 = Color.red;
#endif
    public int[] g1 = [1, 2, 3, 4, 5];
    public List<float> g2 = [1.1f, 2.2f, 3.3f, 4.4f, 5.5f];
#if GODOT
    public Godot.Vector2 g31 = new(1.1f, 2.2f);
    public Godot.Vector3 g32 = new(1.1f, 2.2f, 3.3f);
    public Godot.Vector4 g33 = new(1.1f, 2.2f, 3.3f, 4.4f);
    public Godot.Vector2I g41 = new(1, 2);
    public Godot.Vector3I g42 = new(1, 2, 3);
    public Godot.Vector4I g43 = new(1, 2, 3, 4);
    public Godot.Quaternion g5 = Godot.Quaternion.Identity;
#endif
    public Dictionary<char, string> h = new Dictionary<char, string>()
    {
        { 'a', "AAA" },
        { 'b', "BBB" },
        { 'c', "CCC" }
    };
}