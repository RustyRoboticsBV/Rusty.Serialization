using Rusty.Serialization;
using System;

Registry registry = new();
ObjectSerializer<TestClass> serializer = new("test", null);
Console.WriteLine(serializer.Serialize(new TestClass(), registry).Serialize());