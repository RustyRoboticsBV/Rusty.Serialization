using Rusty.Serialization;
using System;

Console.WriteLine("Hello, World!");

var parser = new Rusty.Serialization.CSCD.CscdParser();
System.Console.WriteLine(parser.Parse("[b_SVGabQ, @1-2-3_4:5:6;]", new()));