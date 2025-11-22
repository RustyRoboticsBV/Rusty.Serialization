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

        /* Private methods. */
        private static void Report<T>(string str, Result<T> result, Result<T> expected, string nodeType, Exception ex)
        {
            string prefix = "";
            string text = "";
            if (!result.thrown)
            {
                if (!expected.thrown)
                {
                    if (result.value == null && expected.value == null || result.value.Equals(expected.value))
                    {
                        prefix = "SUCCESS";
                        text = $"{nodeType} accepts {Format(str)} with correct output {ToSequence(result.ToString())}";
                    }
                    else
                    {
                        prefix = "FAILED";
                        text = $"{nodeType} accepts {Format(str)} with wrong output {ToSequence(result.ToString())} - it should be {ToSequence(expected.ToString())}";
                    }
                }
                else
                {
                    prefix = "FAILED";
                    text = $"{nodeType} accepts {Format(str)} with output {ToSequence(result.ToString())} - it should throw";
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
                    text = $"{nodeType} throws on {Format(str)} - we expected output {ToSequence(expected.ToString())}{verbose}";
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

        private static string ToSequence(string str)
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
    }
}
#endif