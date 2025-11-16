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
Console.WriteLine(Character.Deserialize("''").Serialize());