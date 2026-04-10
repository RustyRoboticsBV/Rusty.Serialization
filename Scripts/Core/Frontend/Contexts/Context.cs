namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base-class for conversion method contexts.
    /// </summary>
    public abstract class Context
    {
        /* Protected properties. */
        /// <summary>
        /// The object codec.
        /// </summary>
        protected ObjectCodec Codec { get; private set; }

        /* Constructors. */
        public Context(ObjectCodec codec)
        {
            Codec = codec;
        }
    }
}