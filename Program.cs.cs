#if RUSTY_DEBUG
using System;
using Rusty.Serialization;
using Rusty.Serialization.Testing;

string serialized = Serializer.Default.Serialize(new Test<char>());
Console.WriteLine(serialized);

UnitTests.RunSerializeTests();
UnitTests.RunParserTests();
#endif