using Rusty.Serialization;
using Rusty.Serialization.GodotEngine;
using System;

Registry context = new();

Console.WriteLine(new BoolSerializer().Serialize(true, context).Serialize());
Console.WriteLine(new IntSerializer().Serialize(123, context).Serialize());
Console.WriteLine(new FloatSerializer().Serialize(123.456f, context).Serialize());
Console.WriteLine(new CharSerializer().Serialize('a', context).Serialize());
Console.WriteLine(new StringSerializer().Serialize("ABC DEF", context).Serialize());
#if GODOT
Console.WriteLine(new ColorSerializer().Serialize(Godot.Colors.Red, context).Serialize());
#endif
Console.WriteLine(new StringSerializer().Serialize(null, context).Serialize());
Console.WriteLine(new Rusty.Serialization.ArraySerializer<int>().Serialize([1, 2, 3], context).Serialize());


Console.WriteLine("a");
Registry registry = new();
ObjectSerializer<TestClass> serializer = new("test", null);
Console.WriteLine("b");
Console.WriteLine(serializer.Serialize(new TestClass(), registry).Serialize());