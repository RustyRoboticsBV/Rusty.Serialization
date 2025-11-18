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
    public Dictionary<char, string> h = new Dictionary<char, string>()
    {
        { 'a', "AAA" },
        { 'b', "BBB" },
        { 'c', "CCC" }
    };
}