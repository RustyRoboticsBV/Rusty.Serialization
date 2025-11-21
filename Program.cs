using Rusty.Serialization;
using Rusty.Serialization.Testing;
using System;

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

string serialized = context.Serialize(test);

Console.WriteLine("Serialized");
Console.WriteLine(serialized);

Test<int> deserialized = context.Deserialize<Test<int>>(serialized);
string reserialized = context.Serialize(deserialized);
Console.WriteLine();
Console.WriteLine("Reserialized");
Console.WriteLine(reserialized);

Console.WriteLine();
Console.WriteLine("Are the objects equal: " + (serialized == reserialized));

#if DEBUG
Console.WriteLine();
context.Registry.PrintSerializers();
#endif