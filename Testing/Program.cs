using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
int number = 0;
int.TryParse(Console.ReadLine(), out number);
if (number == 0)
    return;

TimeSpan lowestElapsed = TimeSpan.MaxValue;
TimeSpan highestElapsed = TimeSpan.MinValue;
TimeSpan averageElapsed = new TimeSpan(0);

for (int i = 0; i < number; i++)
{
    long g0Before = GC.CollectionCount(0);
    long g1Before = GC.CollectionCount(1);

    stopwatch = Stopwatch.StartNew();
    ucs.Codec.Parse(text);
    stopwatch.Stop();
    
    if (GC.CollectionCount(0) != g0Before || GC.CollectionCount(1) != g1Before)
    {
        Console.WriteLine("GC at iteration " + i);
    }

    Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
    if (i > number / 10)
    {
        if (stopwatch.Elapsed < lowestElapsed)
            lowestElapsed = stopwatch.Elapsed;
        if (stopwatch.Elapsed > highestElapsed)
            highestElapsed = stopwatch.Elapsed;
        averageElapsed += stopwatch.Elapsed;
    }
}
averageElapsed /= number - number / 10;

Console.WriteLine($"Best Time: {lowestElapsed}");
Console.WriteLine($"Worst Time: {highestElapsed}");
Console.WriteLine($"Average Time: {averageElapsed}");