using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A codec that converts between object graphs and a node tree.
    /// </summary>
    public abstract class ObjectCodec
    {
        /* Public properties. */
        public ConverterRegistry Converters { get; protected set; }

        /* Public methods. */
        /// <summary>
        /// Convert an object to a node tree.
        /// </summary>
        public abstract SemanticTree Convert<T>(T obj);
        /// <summary>
        /// Deconvert a node tree to an object of some type.
        /// </summary>
        public abstract T Deconvert<T>(SemanticTree tree);
    }
}