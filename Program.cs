using Rusty.Serialization;
using Rusty.Serialization.Converters;
using Rusty.Serialization.Test;
using Rusty.Serialization.Testing;
using System;
using System.Collections;

DateTime dt = new(2005, 2, 29);
System.Console.WriteLine(dt);
UnitTests.RunParserTests(true);
UnitTests.RunSerializeTests();
return;

Context context = new();
context.AddConverter<bool, BoolConverter>();
context.AddConverter<int, IntConverter>();
context.AddConverter<float, FloatConverter>();
context.AddConverter<char, CharConverter>();
context.AddConverter<string, StringConverter>();
context.AddConverter<byte[], ByteArrayConverter>();
context.AddConverter(typeof(System.ValueTuple<>), typeof(TupleConverter<>));
Console.WriteLine(context.Serialize(new byte[] { 1, 2, 3 }));
Console.WriteLine(context.Serialize(new int[] { 1, 2, 3 }));
Console.WriteLine(context.Serialize(new object[][] { [1, 'a', "ABC"], [], [4, 5.5f, new byte[] { 0x12, 0x34 }] }));
Console.WriteLine(context.Serialize((5, 'C')));
Console.WriteLine(context.Serialize(new System.Collections.Generic.List<int> { 1, 2, 3 }));
Console.WriteLine(context.Serialize(System.ConsoleKey.P));
return;
