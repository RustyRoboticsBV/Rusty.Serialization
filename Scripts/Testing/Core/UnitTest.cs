#if RUSTY_DEBUG
namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A base class for all unit tests.
    /// </summary>
    public abstract class UnitTest
    {
        /* Public properties. */
        public UnitTestResult Result { get; private set; } = UnitTestResult.Uninitialized;
        public bool Failed => Result != UnitTestResult.CorrectResult && Result != UnitTestResult.CorrectException;

        /* Public methods. */
        /// <summary>
        /// Run this unit test.
        /// </summary>
        public abstract void Run();

        public override string ToString() => Result.ToString();

        /* Protected methods. */
        /// <summary>
        /// Compares the output of a method with an expected output. Store the result.
        /// </summary>
        protected void CheckResult<T>(UnitTestInput<T> expectedInput, UnitTestInput<T> actualInput)
        {
            if (expectedInput == actualInput)
            {
                if (expectedInput.IsException)
                    Result = UnitTestResult.CorrectException;
                else
                    Result = UnitTestResult.CorrectResult;
            }
            else
            {
                if (actualInput.IsException)
                    Result = UnitTestResult.WrongException;
                else
                {
                    if (expectedInput.Type == actualInput.Type)
                        Result = UnitTestResult.WrongResult;
                    else
                        Result = UnitTestResult.TypeMismatch;
                }
            }
        }
    }

    /// <summary>
    /// A base class for all typed unit tests.
    /// </summary>
    public abstract class UnitTest<InputT, OutputT> : UnitTest
    {
        /* Public properties. */
        public InputT Input { get; private set; }
        public UnitTestInput<OutputT> ExpectedOutput { get; private set; }
        public UnitTestInput<OutputT> ActualOutput { get; protected set; }

        /* Constructors. */
        public UnitTest(InputT input, UnitTestInput<OutputT> expectedOutput)
        {
            Input = input;
            ExpectedOutput = expectedOutput;
        }

        /* Public methods. */
        public override void Run()
        {
            ActualOutput = Evaluate();
            CheckResult(ExpectedOutput, ActualOutput);
        }

        /* Protected methods. */
        /// <summary>
        /// Get the actual output.
        /// </summary>
        protected abstract UnitTestInput<OutputT> Evaluate();
    }
}
#endif