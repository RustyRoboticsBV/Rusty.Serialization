#if RUSTY_DEBUG
namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// Throw exception test result singleton.
    /// </summary>
    public class Throw
    {
        /* Fields. */
        private static Throw exception = new();

        /* Public properties. */
        public static Throw Exception => exception;

        /* Constructors. */
        private Throw() { }
    }
}
#endif