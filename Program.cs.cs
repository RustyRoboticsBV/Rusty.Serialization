#if RUSTY_DEBUG
using System;
using System.Text;
using Rusty.Serialization;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Testing;

//TypeNameParser.Test();
string nstr = "System.Collections.Generic.Dictionary`2[System.Int32,System.Char][,][]";
TypeName tn = new(nstr, null);
Type ttype = tn.ToType();
Console.WriteLine(nstr);
Console.WriteLine(tn);
Console.WriteLine(ttype);
Console.ReadLine();

DefaultContext context = new DefaultContext();

var obj = new Test<char>();
string serialized = context.Serialize(obj);
Console.WriteLine(serialized);

Console.WriteLine();
obj = context.Deserialize<Test<char>>(serialized);
string reserialized = context.Serialize(obj);
Console.WriteLine(reserialized);

Console.WriteLine();
Console.WriteLine("Test");
Console.WriteLine(context.Deserialize<StringBuilder>("(str)  \"abc\""));

Console.WriteLine();
Console.WriteLine("Lossless: " + (serialized == reserialized));

Console.ReadLine();

UnitTests.RunSerializeTests();
UnitTests.RunParserTests();
#endif