using Rusty.Serialization;
using Rusty.Serialization.Nodes;
using Rusty.Serialization.Test;
using Rusty.Serialization.Testing;
using System;

UnitTests.RunParserTests();


return;
Console.WriteLine(BoolNode.Parse("true"));
Console.WriteLine(IntNode.Parse("-0"));
Console.WriteLine(IntNode.Parse("1000"));
Console.WriteLine(RealNode.Parse("1000.0"));
Console.WriteLine(RealNode.Parse(".5"));
Console.WriteLine(RealNode.Parse("1."));
Console.WriteLine(RealNode.Parse("."));
Console.WriteLine(CharNode.Parse("'A'"));
Console.WriteLine(CharNode.Parse("'''"));
Console.WriteLine(CharNode.Parse("'\"'"));
Console.WriteLine(CharNode.Parse("'\\''"));
Console.WriteLine(CharNode.Parse("'\\\"'"));
Console.WriteLine(CharNode.Parse("'\\t'"));
Console.WriteLine(CharNode.Parse("'\\n'"));
Console.WriteLine(CharNode.Parse("'\\0'"));
Console.WriteLine(CharNode.Parse("'\\[21FF]'"));
Console.WriteLine(CharNode.Parse("'\n'"));
Console.WriteLine("\u21ff");
return;

// Create test class object.
Test<int> test = new();
test.@bool = false;
test.@int = 12345;
test.@float = 987.321f;
test.@char = 'C';
test.@string = "A\"B\"C";
test.array = [5, 7, 9, 11];
test.dictionary = new() { { 'C', "CCC" }, { 'D', "DDD" } };
test.@enum = Test<int>.Enum.B;
test.@class = new Test<int>.Disaster<string, decimal>() { thing = new(), a = 10 };

// Print test class.
SerializerContext context = new();

Console.WriteLine("Serialized");
string serialized = context.Serialize(test);
Console.WriteLine(serialized);

Console.WriteLine();
Console.WriteLine("Reserialized");
Test<int> deserialized = context.Deserialize<Test<int>>(serialized);
string reserialized = context.Serialize(deserialized);
Console.WriteLine(reserialized);

Console.WriteLine();
Console.WriteLine("Are the objects equal: " + (serialized == reserialized));

#if DEBUG
Console.WriteLine();
context.Registry.PrintSerializers();
#endif