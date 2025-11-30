#if RUSTY_DEBUG
using Rusty.Serialization.Core.Contexts;
using System.Collections.Generic;

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

            var Int = GetBatch<int>();
            Int.Add(new RoundTripTest<int>(0, "(System.Int32)0", Context, PrettyPrint));
            Add(Int);
        }

        /* Private methods. */
        private static UnitTestBatch<RoundTripTest<T>> GetBatch<T>() => new();
    }
}
#endif