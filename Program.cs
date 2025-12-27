using Rusty.Serialization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Testing;
using System;

DefaultConversionContext context = new();
A a = new();
ObjectDumper.Print(a);

NodeTree tree = context.Convert(a);
Console.WriteLine();
Console.WriteLine(tree);

a = context.Deconvert<A>(tree);
Console.WriteLine();
ObjectDumper.Print(a);

tree = context.Convert(a);
Console.WriteLine();
Console.WriteLine(tree);


class A
{
    public bool a = true;

    public sbyte b1 = 1;
    public short b2 = 2;
    public int b3 = 3;
    public long b4 = 4;
    public byte b5 = 5;
    public ushort b6 = 6;
    public uint b7 = 7 ;
    public ulong b8 = 8;

    public float c1 = 1.1f;
    public double c2 = 2.2;
    public decimal c3 = 3.3m;

    public char e = 'a';

    public string f1 = "ABC";
    public string f2 = "DEF";

    public System.Drawing.Color g = System.Drawing.Color.Red;

    public DateTime h = new(1997, 2, 13, 13, 30, 10);

    public byte[] i = [0x01, 0x23, 0x45, 0xAB];

    public int[] j1 = [1, 2, 3];
    public (int, float, (char, string)) j2 = (0, 1.1f, ('a', "ABC"));
    public System.Tuple<int, float, System.Tuple<char, string>> j3 = new(0, 1.1f, new('a', "ABC"));
    public System.Collections.Generic.List<int> j4 = [4, 5, 6];

    public System.Collections.Generic.Dictionary<string, int> k = new System.Collections.Generic.Dictionary<string, int>()
    {
        { "ABC", 123 },
        { "DEF", 456 },
        { "GHI", 789 }
    };

    public A l;

    public A()
    {
        f2 = f1;
        l = this;
    }
}