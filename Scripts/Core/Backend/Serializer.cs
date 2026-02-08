using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Codecs
{
    /// <summary>
    /// A base class for serializers.
    /// </summary>
    public abstract class Serializer
    {
        /* Protected properties. */
        protected static StringBuilderBag StringBuilders { get; } = new();

        /* Public methods. */
        public abstract string Serialize(NodeTree node, Settings settings);
    }
}