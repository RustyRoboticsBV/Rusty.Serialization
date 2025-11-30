#if RUSTY_DEBUG
using Rusty.Serialization.Core.Contexts;
using System.Collections.Generic;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A batch of serializer tests.
    /// </summary>
    public class AllTests
    {
        public IContext Context { get; private set; }
        public bool PrettyPrint { get; private set; }

        /* Private properties. */
        private List<UnitTestBatch> Batches { get; } = new();

        /* Constructors. */
        public AllTests(IContext context, bool prettyPrint)
        {
            Context = context;
            PrettyPrint = prettyPrint;
        }

        public void Add(UnitTestBatch test)
        {
            Batches.Add(test);
        }

        public void Run()
        {
            RoundTripTest<int> int1 = new(0, "(System.Int32)0", Context, PrettyPrint);
            int1.Run();
            System.Console.WriteLine(int1);
        }
    }
}
#endif