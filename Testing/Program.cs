using System;
using System.Diagnostics;
using System.IO;
using Rusty.Serialization;
using Rusty.Serialization.Core.Nodes;

string text = File.ReadAllText("BigTest.cscd");
UCS ucs = new UCS();

Console.WriteLine("Hi! Press enter to test.");
Console.ReadLine();

// Parse.
Stopwatch stopwatch = Stopwatch.StartNew();
NodeTree tree = ucs.Codec.Parse(text);
stopwatch.Stop();

Console.WriteLine(tree);
Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");

// Repeat.
Console.WriteLine("Enter number of repititions.");
int number = int.Parse(Console.ReadLine());

for (int i = 0; i < number; i++)
{
    stopwatch = Stopwatch.StartNew();
    ucs.Codec.Parse(text);
    stopwatch.Stop();

    Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
}