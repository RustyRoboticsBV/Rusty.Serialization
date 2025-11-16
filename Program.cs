using Rusty.Serialization;
using System;

Vector4 f4 = "[1,2,3,4]";
Console.WriteLine(f4.Value);

Projection pro = "[[1,2,3,4],[1,2,3,4],[1,2,3,4],[1,2,3,4]]";
Console.WriteLine(pro.Value);
