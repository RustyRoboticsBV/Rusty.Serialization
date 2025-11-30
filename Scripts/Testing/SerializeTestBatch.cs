#if RUSTY_DEBUG
using Rusty.Serialization.Core.Contexts;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A batch of serializer/deserializer tests.
    /// </summary>
    public class SerializeTestBatch<T> : UnitTestBatch<SerializeTest<T>>
    {
        /* Public properties. */
        public IContext Context { get; set; }
        public bool PrettyPrint { get; set; }

        /* Constructors. */
        public SerializeTestBatch(IContext context, bool prettyPrint) : base()
        {
            Context = context;
            PrettyPrint = prettyPrint;
        }

        /* Public methods. */
        public void Add(T input, UnitTestInput<string> expectedOutput)
        {
            Add(new SerializeTest<T>(input, expectedOutput, Context, PrettyPrint));
        }
    }
}
#endif