using Rusty.Serialization;
using Rusty.Serialization.Nodes;
using Rusty.Serialization.Test;
using Rusty.Serialization.Testing;
using System;

FullTypeName ftn1 = new(typeof(Test<char>));
Console.WriteLine((TypeName)ftn1);
FullTypeName ftn2 = new(typeof(Test<char>.Ouchie.Disaster<uint, System.Collections.Generic.List<string>>));
Console.WriteLine((TypeName)ftn2);
Console.ReadLine();

Context context = new();
string serialized = context.Serialize(new System.Collections.Generic.Dictionary<object, object>()
{
    { new System.Collections.Generic.List<object>() { "abc", 'd', 123 }, 12.5 },
    { false, new System.Collections.Generic.LinkedList<System.DateTime>([new System.DateTime(1,2,3)]) },
    { "ABC", new Struct() }
});
System.Console.WriteLine(serialized);
System.Console.WriteLine(ParseUtility.ParseValue(serialized));

UnitTests.RunParserTests(true);
UnitTests.RunSerializeTests();


public struct Struct
{
    public int A = 0;
    public string B = "BBB";
    public object C = 'C';

    public Struct() { }
}