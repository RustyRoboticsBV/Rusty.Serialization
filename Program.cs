using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Testing;
using System;

ConversionContext context = new();
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
    public int a;
    public int b { get; set; } = 1;
    public A c { get; set; }
    public string d = null;
    public string e = "ABC";

    public struct NestedStruct
    {
        public A a;
        public A b;
        public string c;
        public string d;
    }
    public NestedStruct f = new();

    public A()
    {
        d = e;
        c = this;
        f.a = this;
        f.b = this;
        f.c = "ABC";
        f.d = "DEF";
    }
}