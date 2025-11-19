using Rusty.Serialization;
using Rusty.Serialization.Nodes;
using System;

Registry registry = new();
ObjectSerializer<TestClass> serializer = new("test", null);
string serialized = serializer.Serialize(new TestClass(), registry).Serialize();
TestClass deserialized = serializer.Deserialize(ObjectNode.Deserialize(serialized), registry);
string reserialized = serializer.Serialize(deserialized, registry).Serialize();
Console.WriteLine(serialized);
Console.WriteLine(reserialized);