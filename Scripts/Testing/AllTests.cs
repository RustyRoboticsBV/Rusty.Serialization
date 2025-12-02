#if RUSTY_DEBUG
using System;
using Rusty.Serialization.Core.Contexts;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A batch of serializer tests.
    /// </summary>
    public class AllTests : UnitTestBatch<UnitTestBatch>
    {
        /* Public properties. */
        public IContext Context { get; private set; }
        public bool PrettyPrint { get; private set; }

        /* Constructors. */
        public AllTests(IContext context, bool prettyPrint)
        {
            Context = context;
            PrettyPrint = prettyPrint;

            // Booleans.
            AddTest<bool>("System.Boolean",
                (true, "true"),
                (false, "false")
            );

            // Integers.
            AddTest<sbyte>("System.SByte",
                (0, "0"),
                (123, "123"),
                (-123, "-123"),
                (sbyte.MaxValue, sbyte.MaxValue.ToString()),
                (sbyte.MinValue, sbyte.MinValue.ToString())
            );
            AddTest<short>("System.Int16",
                (0, "0"),
                (123, "123"),
                (-456, "-456"),
                (short.MaxValue, short.MaxValue.ToString()),
                (short.MinValue, short.MinValue.ToString())
            );
            AddTest<int>("System.Int32",
                (0, "0"),
                (12345, "12345"),
                (-67890, "-67890"),
                (int.MaxValue, int.MaxValue.ToString()),
                (int.MinValue, int.MinValue.ToString())
            );
            AddTest<long>("System.Int64",
                (0, "0"),
                (12345, "12345"),
                (-67890, "-67890"),
                (long.MaxValue, long.MaxValue.ToString()),
                (long.MinValue, long.MinValue.ToString())
            );

            // Reals.
            AddTest<float>("System.Single",
                (0f, "."),
                (123f, "123."),
                (.5f, ".5"),
                (123.5f, "123.5"),
                (float.MaxValue, PeterO.Numbers.EDecimal.FromSingle(float.MaxValue).ToString() + '.'),
                (float.MinValue, PeterO.Numbers.EDecimal.FromSingle(float.MinValue).ToString() + '.')
            );
            AddTest<double>("System.Double",
                (0.0, "."),
                (123, "123."),
                (.5, ".5"),
                (123.5, "123.5"),
                (double.MaxValue, PeterO.Numbers.EDecimal.FromDouble(double.MaxValue).ToString() + '.'),
                (double.MinValue, PeterO.Numbers.EDecimal.FromDouble(double.MinValue).ToString() + '.')
            );
            AddTest<decimal>("System.Decimal",
                (0m, "."),
                (123m, "123."),
                (.5m, ".5"),
                (123.5m, "123.5"),
                (decimal.MaxValue, PeterO.Numbers.EDecimal.FromDecimal(decimal.MaxValue).ToString() + '.'),
                (decimal.MinValue, PeterO.Numbers.EDecimal.FromDecimal(decimal.MinValue).ToString() + '.')
            );

            // String.
            AddTest<string>("System.String",
                ("", "\"\""),
                ("abcdefg\t'\n\"", "\"abcdefg\\t'\\n\\\"\"")
            );

            // Time.
            AddTest<DateTime>("System.DateTime",
                (new DateTime(1994, 2, 13), "Y1994M2D13"),
                (new DateTime(1994, 2, 13, 14, 10, 3), "Y1994M2D13h14m10s3"),
                (new DateTime(1994, 2, 13, 0, 0, 0, 30), "Y1994M2D13f30")
            );
        }

        /* Public methods. */
        public override string ToString()
        {
            if (Successes == Total)
                return base.ToString() + "\n\nSUCCESS: All tests succeeded.";
            else
                return base.ToString() + "\n\nFAILURE: Some tests failed.";
        }

        /* Private methods. */
        private UnitTestBatch<RoundTripTest<T>> GetTest<T>(string typeName, params SerializeTestInput<T>[] values)
        {
            UnitTestBatch<RoundTripTest<T>> batch = new();
            foreach (var value in values)
            {
                string expected = $"({typeName}){value.ExpectedOutput}";
                batch.Add(new RoundTripTest<T>(value.InputValue, expected, Context, PrettyPrint));
            }
            return batch;
        }

        private void AddTest<T>(string typeName, params SerializeTestInput<T>[] values)
        {
            Add(GetTest(typeName, values));
        }
    }
}
#endif