#if RUSTY_DEBUG
using Rusty.Serialization.Core.Contexts;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A round-trip serialization unit test:
    /// Serialize -> Deserialize -> (Re)Serialize -> Compare.
    /// </summary>
    public class RoundTripTest<TypeT> : UnitTest<TypeT, bool>
    {
        public IContext Context { get; set; }
        public bool PrettyPrint { get; set; }
        public UnitTestInput<string> ExpectedSerializeOutput { get; private set; }

        public SerializeTest<TypeT> Serialize { get; private set; }
        public DeserializeTest<TypeT> Deserialize { get; private set; }
        public SerializeTest<TypeT> Reserialize { get; private set; }

        public string Log { get; private set; } = "";

        public RoundTripTest(TypeT input, UnitTestInput<string> expectedSerializeOutput, IContext context, bool prettyPrint)
            : base(input, true)
        {
            Context = context;
            PrettyPrint = prettyPrint;
            ExpectedSerializeOutput = expectedSerializeOutput;
        }

        public override string ToString()
        {
            if (Failed)
                return $"FAILURE:{typeof(TypeT).FullName} = '{Input}'\n" + Log;
            else
                return $"SUCCESS: {typeof(TypeT).FullName} = '{Input}'\n" + Log;
        }

        /* Protected methods. */
        protected override UnitTestInput<bool> Evaluate()
        {
            Log = "";

            // STEP 1: Serialize.
            Serialize = new(Input, ExpectedSerializeOutput, Context, PrettyPrint);
            Serialize.Run();

            Log += "- " + Serialize.ToString();
            if (Serialize.Failed)
                return false;

            // STEP 2: Deserialize.
            Deserialize = new(Serialize.ActualOutput, new(Input), Context);
            Deserialize.Run();

            Log += "\n- " + Deserialize.ToString();
            if (Deserialize.Failed)
                return false;

            // STEP 3: Reserialize.
            Reserialize = new(Deserialize.ActualOutput, ExpectedSerializeOutput, Context, PrettyPrint);
            Reserialize.Run();

            Log += "\n- " + Reserialize.ToString();
            if (Reserialize.Failed)
                return false;

            // STEP 4: Bool test comparing serialized and reserialized.
            bool equal = Serialize.ActualOutput == Reserialize.ActualOutput;
            return equal;
        }
    }
}
#endif
