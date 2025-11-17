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

var obj = ObjectConversionUtility.Convert(new Test(), "test", "a", "b", "c", "d", "e", "f", "g", "h");
Console.WriteLine(obj);
Console.WriteLine(obj.Serialize());

class Test
{
    public bool a = true;
    public int b = 10;
    public float c = 1.234f;
    public char d = '"';
    public string e = "Text";
    public Godot.Color f = Godot.Colors.Red;
    public int[] g = [1, 2, 3,];
    public System.Collections.Generic.Dictionary<char, string> h = new System.Collections.Generic.Dictionary<char, string> {
        { 'a', "AAA" },
        { 'b', "BBB" },
        { 'c', "CCC" }
    };
}