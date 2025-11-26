#if DEBUG
using Rusty.Serialization.Nodes;
using System;

namespace Rusty.Serialization.Test
{
    /// <summary>
    /// Unit test utility for checking if a parse function is working correctly.
    /// </summary>
    public static class ParseTester
    {
        /* Public types. */
        public struct ParseResultThrow { }

        public class Result<T>
        {
            public bool thrown;
            public object value;

            public Result(object value, bool thrown = false)
            {
                this.thrown = thrown;
                this.value = value;
            }

            public static implicit operator Result<T>(T value) => new(value);
            public static implicit operator Result<T>(ParseResultThrow value) => new(default, true);

            public override string ToString()
            {
                if (value == null)
                    return "null";
                return value?.ToString();
            }
        }

        /* Public properties. */
        public static bool Verbose { get; set; } = false;
        public static bool Numeric { get; set; } = false;
        public static int Total => Successes + Failures;
        public static int Successes { get; set; }
        public static int Failures { get; set; }
        public static ParseResultThrow Throw => new();

        /* Public methods. */
        public static void TestBool(string str, Result<bool> expected)
        {
            try
            {
                var node = BoolNode.Parse(str);
                Report(str, new(node.Value), expected, nameof(BoolNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(BoolNode), ex);
            }
        }

        public static void TestInt(string str, Result<decimal> expected)
        {
            try
            {
                var node = IntNode.Parse(str);
                Report(str, new(node.Value), expected, nameof(IntNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(IntNode), ex);
            }
        }

        public static void TestReal(string str, Result<decimal> expected)
        {
            try
            {
                var node = RealNode.Parse(str);
                Report(str, new(node.Value), expected, nameof(RealNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(RealNode), ex);
            }
        }

        public static void TestChar(string str, Result<char> expected)
        {
            try
            {
                var node = CharNode.Parse(str);
                Report(str, new(node.Value), expected, nameof(CharNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(CharNode), ex);
            }
        }

        public static void TestString(string str, Result<string> expected)
        {
            try
            {
                var node = StringNode.Parse(str);
                Report(str, new(node.Value), expected, nameof(StringNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(StringNode), ex);
            }
        }

        public static void TestColor(string str, Result<(byte, byte, byte, byte)> expected)
        {
            try
            {
                var node = ColorNode.Parse(str);
                Report(str, new((node.R, node.G, node.B, node.A)), expected, nameof(ColorNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(ColorNode), ex);
            }
        }

        public static void TestTimestamp(string str, Result<TimestampNode.Timestamp> expected)
        {
            try
            {
                var node = TimestampNode.Parse(str);
                Report(str, new(node.Value), expected, nameof(TimestampNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(TimestampNode), ex);
            }
        }

        public static void TestBinary(string str, Result<byte[]> expected)
        {
            try
            {
                var node = BinaryNode.Parse(str);
                Report(str, node.Value, expected, nameof(BinaryNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(BinaryNode), ex);
            }
        }

        public static void TestNull(string str, Result<object> expected)
        {
            try
            {
                var node = NullNode.Parse(str);
                Report(str, new(null), expected, nameof(NullNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(NullNode), ex);
            }
        }

        public static void TestType(string str, Result<TypeName> expected)
        {
            try
            {
                var node = TypeNode.Parse(str);
                Report(str, new(node.Name), expected, nameof(TypeNode), null);
            }
            catch (Exception ex)
            {
                Report(str, Throw, expected, nameof(TypeNode), ex);
            }
        }

        /* Private methods. */
        private static void Report<T>(string str, Result<T> result, Result<T> expected, string nodeType, Exception ex)
        {
            string prefix = "";
            string text = "";
            if (!result.thrown)
            {
                if (!expected.thrown)
                {
                    if (result.value == null && expected.value == null || Equals(result.value, expected.value))
                    {
                        prefix = "SUCCESS";
                        text = $"{nodeType} accepts {Format(str)} with correct output {Numberfy(Stringify(result.value))}";
                    }
                    else
                    {
                        prefix = "FAILED";
                        text = $"{nodeType} accepts {Format(str)} with wrong output {Numberfy(Stringify(result.value))} - it should be {Numberfy(Stringify(expected.value))}";
                    }
                }
                else
                {
                    prefix = "FAILED";
                    text = $"{nodeType} accepts {Format(str)} with output {Numberfy(Stringify(result.value))} - it should throw";
                }
            }
            else
            {
                string verbose = "";
                if (Verbose)
                    verbose = ":\n" + ex.Message;

                if (expected.thrown)
                {
                    prefix = "SUCCESS";
                    text = $"{nodeType} correctly throws on {Format(str)}{verbose}";
                }
                else
                {
                    prefix = "FAILED";
                    text = $"{nodeType} throws on {Format(str)} - we expected output {Numberfy(Stringify(expected.value))}{verbose}";
                }
            }

            if (prefix == "SUCCESS")
                Successes++;
            else
                Failures++;
                Console.WriteLine(prefix + ": " + text.Replace("\n", "\n" + new string(' ', prefix.Length + 2)));
        }

        private static string Format(string str)
        {
            if (str == null)
                return "NULL";
            else if (str.StartsWith('\'') || str.EndsWith('\''))
                return $"\"{str}\"";
            else
                return $"'{str}'";
        }

        private static string Numberfy(string str)
        {
            if (!Numeric)
                return str;
            string result = "";
            for (int i = 0; i < str.Length; i++)
            {
                result += ' ' + ((int)str[i]).ToString();
            }
            return result;
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

        private static bool Equals<T>(T a, T b)
        {
            if (a is Array aa && b is Array ba)
                return ArrayEquals(aa, ba);

            return a.Equals(b);
        }

        private static bool ArrayEquals(Array a, Array b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (!Equals(a.GetValue(i), b.GetValue(i)))
                    return false;
            }
            return true;
        }
    }
}
#endif