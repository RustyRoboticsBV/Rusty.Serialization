#if RUSTY_DEBUG
using System;
using Rusty.Serialization;
using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Testing;

IContext context = new DefaultContext();
string serialized = context.Serialize(new Test<char>());
Console.WriteLine(serialized);

UnitTests.RunSerializeTests();
UnitTests.RunParserTests();
#endif