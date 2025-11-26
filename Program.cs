using Rusty.Serialization;
using Rusty.Serialization.Nodes;
using Rusty.Serialization.Test;
using System;

Context context = new();
System.Console.WriteLine(context.GetTypeName(typeof(System.Collections.Generic.LinkedList<DateTime>)));
string serialized = context.Serialize(new System.Collections.Generic.Dictionary<object, object>()
{
    { new System.Collections.Generic.List<object>() { "abc", 'd', 123 }, 12.5 },
    { false, new System.Collections.Generic.LinkedList<DateTime>([new DateTime(1,2,3)]) },
    { "ABC", new Struct() }
});
System.Console.WriteLine(serialized);
System.Console.WriteLine(ParseUtility.ParseValue(serialized));
Console.ReadLine();

UnitTests.RunParserTests(true);
UnitTests.RunSerializeTests();


public struct Struct
{
    public int A = 0;
    public string B = "BBB";
    public object C = 'C';

    public Struct() { }
}