using Rusty.Serialization.Testing;
using Rusty.Serialization;
using System;

Test<int> test = new();
test.@bool = false;
test.@int = 12345;
test.@float = 987.321f;
test.@char = 'C';
test.@string = "A\"B\"C";
test.array = [5, 7, 9, 11];
test.dictionary = new() { { 'C', "CCC" }, { 'D', "DDD" } };
test.@enum = Test<int>.Enum.B;

SerializerContext context = new();
/*Console.WriteLine(context.Serialize(5));
Console.WriteLine(context.Serialize(7.77));
Console.WriteLine(context.Serialize(new int[] { 1, 2, 3 }));
Console.WriteLine(context.Serialize(new System.Collections.Generic.List<int> { 1, 2, 3 }));*/
Console.WriteLine("Serialized:");
Console.WriteLine(context.Serialize(test));
Console.WriteLine(new TypeName(typeof(int[])));
Console.WriteLine(new TypeName(typeof(Test<Test<char>[][,]>)) + 'a');

/*string serialized = context.Serialize(test);
Test<int> deserialized = context.Deserialize<Test<int>>(serialized);
string reserialized = context.Serialize(deserialized);

Console.WriteLine("Serialized");
Console.WriteLine(serialized);

Console.WriteLine();
Console.WriteLine("Reserialized");
Console.WriteLine(reserialized);

Console.WriteLine();
Console.WriteLine("Are the objects equal: " + (serialized == reserialized));*/

#if DEBUG
Console.WriteLine();
context.Registry.PrintSerializers();
#endif