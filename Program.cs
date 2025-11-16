using Rusty.Serialization.Nodes;
using System;

Integer integer = 10;
Console.WriteLine(integer.Serialize());

//Float real = -1234567890123456789.01234;
Float real = 0.000000000000000000000000000000000000001;
Console.WriteLine(real.Serialize());
Console.WriteLine(Float.Deserialize("010.01000").Serialize());

Character character = 'A';
Console.WriteLine(character.Serialize());

List list = new INode[] { (Integer)0, (Character)'A', (Float)0.125790 };
List nestedList = new INode[] { list, Color.Deserialize("#AABBCC"), (Rusty.Serialization.Nodes.String)"\"Text\".", (Rusty.Serialization.Nodes.Boolean)true };
Console.WriteLine(nestedList);
Console.WriteLine(list.Serialize());
Console.WriteLine(List.Deserialize(nestedList.Serialize()));