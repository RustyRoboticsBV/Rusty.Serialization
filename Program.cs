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
    public bool a;

    public sbyte b1;
    public short b2;
    public int b3;
    public long b4;
    public byte b5;
    public ushort b6;
    public uint b7;
    public ulong b8;

    public float c1;
    public double c2;
    public decimal c3;

    public char e;

    public string f1 = "ABC";
    public string f2 = "DEF";

    public System.Drawing.Color g = System.Drawing.Color.Red;

    public DateTime h = new(1997, 2, 13, 13, 30, 10);

    public byte[] i = [0x01, 0x23, 0x45, 0xAB];

    public int[] j1 = [1, 2, 3];
    public System.Collections.Generic.List<int> j2 = [4, 5, 6];

    public A()
    {
        f2 = f1;
    }
}