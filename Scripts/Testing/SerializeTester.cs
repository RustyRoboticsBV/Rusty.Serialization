#if RUSTY_DEBUG
using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// Unit test utility for checking if a serializing function is working correctly.
    /// </summary>
    public static class SerializeTester
    {
        /* Public methods. */
        public static void TestBool(bool obj, string expectedValue)
        {
            BoolNode node = new(obj);
            string result = node.Serialize();
            Report(obj, result, expectedValue);
        }

        public static void TestInt(decimal obj, string expectedValue)
        {
            IntNode node = new(obj);
            string result = node.Serialize();
            Report(obj, result, expectedValue);
        }

        public static void TestReal(decimal obj, string expectedValue)
        {
            RealNode node = new(obj);
            string result = node.Serialize();
            Report(obj, result, expectedValue);
        }

        public static void TestChar(char obj, string expectedValue)
        {
            CharNode node = new(obj);
            string result = node.Serialize();
            Report(obj, result, expectedValue);
        }

        public static void TestString(string obj, string expectedValue)
        {
            StringNode node = new(obj);
            string result = node.Serialize();
            Report(obj, result, expectedValue);
        }

        public static void TestColor((byte, byte, byte, byte) obj, string expectedValue)
        {
            ColorNode node = new(obj.Item1, obj.Item2, obj.Item3, obj.Item4);
            string result = node.Serialize();
            Report(obj, result, expectedValue);
        }

        public static void TestBinary(byte[] obj, string expectedValue)
        {
            BinaryNode node = new(obj);
            string result = node.Serialize();
            Report(obj, result, expectedValue);
        }

        public static void TestNull(string expectedValue)
        {
            NullNode node = new();
            string result = node.Serialize();
            Report(null, result, expectedValue);
        }

        /* Private methods. */
        private static void Report(object obj, string value, string expected)
        {
            if (value == expected)
                Console.WriteLine($"SUCCESS: Correctly serialized \"{Stringify(obj)}\" to \"{value}\"");
            else
                Console.WriteLine($"FAILED: Serialized \"{Stringify(obj)}\" to \"{value}\" - but expected '{expected}'");
        }

        private static string Stringify(object obj)
        {
            if (obj == null)
                return "null";
            if (obj is Array array)
            {
                string str = "";
                foreach (object element in array)
                {
                    if (str != "")
                        str += ",";
                    str += element.ToString();
                }
                return '[' + str + ']';
            }

            return obj.ToString();
        }
    }
}
#endif