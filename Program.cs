using Rusty.Serialization;
using Rusty.Serialization.Nodes;
using System;

Test test = new();
test.@bool = false;
test.@int = 12345;
test.@float = 987.321f;
test.@char = 'C';
test.@string = "A\"B\"C";
test.array = [5, 7, 9, 11];

Registry registry = new();
ObjectSerializer<Test> serializer = new("test", null);
string serialized = serializer.Serialize(test, registry).Serialize();
Test deserialized = serializer.Deserialize(ObjectNode.Deserialize(serialized), registry);
string reserialized = serializer.Serialize(deserialized, registry).Serialize();
Console.WriteLine(serialized);
Console.WriteLine(reserialized);