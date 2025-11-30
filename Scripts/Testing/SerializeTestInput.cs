#if RUSTY_DEBUG
namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// An input for a serializer/deserializer test.
    /// </summary>
    public class SerializeTestInput<T>
    {
        /* Public properties. */
        public UnitTestInput<T> InputValue { get; set; }
        public UnitTestInput<string> ExpectedOutput { get; set; }

        /* Constructors. */
        public SerializeTestInput(UnitTestInput<T> inputValue, UnitTestInput<string> expectedOutput)
        {
            InputValue = inputValue;
            ExpectedOutput = expectedOutput;
        }

        /* Conversion operators. */
        public static implicit operator SerializeTestInput<T>((UnitTestInput<T>, UnitTestInput<string>) tuple)
            => new(tuple.Item1, tuple.Item2);
    }
}
#endif