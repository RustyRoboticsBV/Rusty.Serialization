#if RUSTY_DEBUG
using System;
using Rusty.Serialization.Core.Contexts;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A deserializing unit test.
    /// </summary>
    public class DeserializeTest<TypeT> : UnitTest<string, TypeT>
    {
        /* Public properties. */
        public IContext Context { get; set; }

        /* Constructors. */
        public DeserializeTest(string input, UnitTestInput<TypeT> expectedOutput, IContext context)
            : base(input, expectedOutput)
        {
            Context = context;
        }

        /* Public methods. */
        public override string ToString()
        {
            switch (Result)
            {
                case UnitTestResult.Uninitialized:
                    return $"INVALID: Call the {nameof(Run)} method.";
                case UnitTestResult.CorrectResult:
                    return $"SUCCESS: Correctly parsed \"{Input}\" to '{ActualOutput}'.";
                case UnitTestResult.WrongResult:
                    return $"FAILURE: Wrongly parsed \"{Input}\" to '{ActualOutput}' - expected '{ExpectedOutput}'.";
                case UnitTestResult.CorrectException:
                    return $"SUCCESS: Correctly threw exception for \"{Input}\".";
                case UnitTestResult.WrongException:
                    return $"FAILURE: Wrongly threw exception for \"{Input}\" - expected output '{ExpectedOutput}'.\nException: {ActualOutput.Exception}";
                case UnitTestResult.TypeMismatch:
                    return $"INVALID: Parsed \"{Input}\" to '{ActualOutput}' - expected '{ExpectedOutput}'. This probably means your test is wrong.";
                default:
                    return "";
            }
        }

        /* Protected methods. */
        protected override UnitTestInput<TypeT> Evaluate()
        {
            UnitTestInput<TypeT> parsed;
            try
            {
                parsed = Context.Deserialize<TypeT>(Input);
            }
            catch (Exception ex)
            {
                parsed = new(ex);
            }
            return parsed;
        }
    }
}
#endif