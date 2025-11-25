using Rusty.Serialization;
using Rusty.Serialization.Nodes;
using Rusty.Serialization.Test;

Context context = new();
string serialized = context.Serialize(new System.Collections.Generic.Dictionary<object, object>()
{
    { new System.Collections.Generic.List<object>() { "abc", 'd', 123 }, 12.5 },
    { false, new System.Collections.Generic.LinkedList<System.DateTime>([new System.DateTime(1,2,3)]) }
});
System.Console.WriteLine(serialized);
System.Console.WriteLine(ParseUtility.ParseValue("{[\"abc\",'d',123]:12.5,false:[Y1M2D3]}"));

UnitTests.RunParserTests(true);
UnitTests.RunSerializeTests();
