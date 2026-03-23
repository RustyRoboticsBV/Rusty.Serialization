using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Codecs
{
    // TODO: move.
    public class Settings
    {
        public bool PrettyPrint = false;
        public bool IncludeFormatHeader = false;
    }

    /// <summary>
    /// A base class for format codec classes.
    /// </summary>
    public abstract class FormatCodec
    {
        /* Public methods. */
        /// <summary>
        /// Serialize a node tree into a string.
        /// </summary>
        public abstract string Serialize(SyntaxTree tree, Settings settings);

        /// <summary>
        /// Deserialize a string into a node tree.
        /// </summary>
        public abstract SyntaxTree Parse(string serialized);

        /// <summary>
        /// Free owned heap memory. Warning: this may trigger a garbage collection spike!
        /// </summary>
        public virtual void Free() { }
    }
}