#if RUSTY_DEBUG
using System.Collections.Generic;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A complex class for testing serialization.
    /// </summary>
    public class TestClass<T, U>
    {
        /* Public types. */
        public class NestedClass
        {
            public T field;
        }

        public class InheritedClass : NestedClass
        {
            public U field2;
        }

        public struct NestedStruct
        {
            public T field;
        }

        /* Fields. */
        public bool a = false;

        public sbyte b1 = 1;
        public short b2 = 2;
        public int b3 = 3;
        public long b4 = 4;
        public byte b5 = 5;
        public ushort b6 = 6;
        public uint b7 = 7;
        public ulong b8 = 8;

        public float c1 = 9.0f;
        public double c2 = 10.0;
        public decimal c3 = 11.0m;

        public char d = 'A';

        public string e = "ABC";

        public System.Drawing.Color f = System.Drawing.Color.Red;

        public System.DateTime g = new(1994, 2, 13, 11, 30, 5);

        public byte[] h = new byte[] { 0x12, 0x34, 0xAB };

        public object i = null;

        public int[] j1 = new int[] { 1, 2, 3, 4, 5 };
        public List<int> j2 = new(new int[] { 6, 7, 8, 9, 10 });

        public Dictionary<string, int> k = new()
        {
            { "ABC", 123 },
            { "DEF", 456 },
            { "GHI", 789 }
        };

        public NestedClass l1 = new();
        public NestedClass l2 = null;
        public NestedClass l3 = new InheritedClass();
        public NestedStruct l4 = new();
        public TestClass<T, U> l5 = null;

        /* Constructors. */
        public TestClass()
        {
            l2 = l1;
            //l5 = this; // TODO: find a way to make this work - currently having the root object reference itself will break ID generation.
        }
    }
}
#endif