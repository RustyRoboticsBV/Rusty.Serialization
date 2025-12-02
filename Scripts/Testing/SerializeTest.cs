#if RUSTY_DEBUG
using Rusty.Serialization.Core.Contexts;
using System;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A serializing unit test.
    /// </summary>
    public class SerializeTest<TypeT> : UnitTest<TypeT, string>
    {
        /* Public properties. */
        public IContext Context { get; set; }
        public bool PrettyPrint { get; set; }

        /* Constructors. */
        public SerializeTest(TypeT input, UnitTestInput<string> expectedOutput, IContext context, bool prettyPrint)
            : base(input, expectedOutput)
        {
            Context = context;
            PrettyPrint = prettyPrint;
        }

        /* Public methods. */
        public override string ToString()
        {
            switch (Result)
            {
                case UnitTestResult.Uninitialized:
                    return $"INVALID: Call the {nameof(Run)} method.";
                case UnitTestResult.CorrectResult:
                    return $"SUCCESS: Correctly serialized '{Input}' to \"{ActualOutput}\".";
                case UnitTestResult.WrongResult:
                    return $"FAILURE: Wrongly serialized '{Input}' to \"{ActualOutput}\" - expected \"{ExpectedOutput}\".";
                case UnitTestResult.CorrectException:
                    return $"SUCCESS: Correctly threw exception for '{Input}'.";
                case UnitTestResult.WrongException:
                    return $"FAILURE: Wrongly threw exception for '{Input}' - expected output \"{ExpectedOutput}\".";
                case UnitTestResult.TypeMismatch:
                    return $"INVALID: Serialized '{Input}' to \"{ActualOutput}\" - expected \"{ExpectedOutput}\". This probably means your test is wrong.";
                default:
                    return "";
            }
        }

        /* Protected methods. */
        protected override UnitTestInput<string> Evaluate()
        {
            UnitTestInput<string> serialized;
            try
            {
                serialized = Context.Serialize(Input, PrettyPrint);
            }
            catch (Exception ex)
            {
                serialized = Throw.Exception;
                // TODO: better exception reporting.
                System.Console.WriteLine(ex);
            }
            return serialized;
        }
    }
}
#endif