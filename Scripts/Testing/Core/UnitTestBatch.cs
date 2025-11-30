#if RUSTY_DEBUG
using System.Collections.Generic;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A base class for all unit test batches.
    /// </summary>
    public abstract class UnitTestBatch : UnitTest
    {
        /* Public properties. */
        public int Successes { get; protected set; }
        public int Failures { get; protected set; }
        public int Invalid { get; protected set; }
        public int Total => Successes + Failures + Invalid;

        /* Public methods. */
        public override string ToString()
        {
            return "Successes: " + Successes + " out of " + Total
                + "\nFailures: " + Failures + " out of " + Total
                + "\nInvalid: " + Invalid + " out of " + Total;
        }
    }

    /// <summary>
    /// A base class for all unit test batches.
    /// </summary>
    public class UnitTestBatch<TestT> : UnitTestBatch
        where TestT : UnitTest
    {
        /* Private properties. */
        List<TestT> Tests { get; } = new();

        /* Public methods. */
        public void Add(TestT test)
        {
            Tests.Add(test);
        }

        public override void Run()
        {
            Successes = 0;
            Failures = 0;
            Invalid = 0;
            foreach (TestT test in Tests)
            {
                test.Run();
                switch (test.Result)
                {
                    case UnitTestResult.CorrectResult:
                    case UnitTestResult.CorrectException:
                        Successes++;
                        break;
                    case UnitTestResult.WrongResult:
                    case UnitTestResult.WrongException:
                        Failures++;
                        break;
                    case UnitTestResult.Uninitialized:
                    case UnitTestResult.TypeMismatch:
                        Invalid++;
                        break;
                }
            }
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < Tests.Count; i++)
            {
                if (i > 0)
                    str += '\n';
                str += "- " + Tests[i].ToString().Replace("\n", "\n  ");
            }
            str += "\n" + base.ToString();
            return str;
        }
    }
}
#endif