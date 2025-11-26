using Rusty.Serialization;
using Rusty.Serialization.Nodes;
using Rusty.Serialization.Testing;
using System;

Context context = new();
object obj = new Test<double>();
Console.WriteLine(context.Serialize(obj));
Console.ReadLine();

//obj = context.Deserialize(obj);

UnitTests.RunParserTests(true);
UnitTests.RunSerializeTests();
