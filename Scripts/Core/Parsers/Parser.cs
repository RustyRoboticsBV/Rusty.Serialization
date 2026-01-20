using Rusty.Serialization.Core.Lexing;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Parsing
{
    /// <summary>
    /// A base class for a token parser.
    /// </summary>
    public abstract class Parser
    {
        /* Public methods. */
        /// <summary>
        /// Parse a text as a node tree.
        /// </summary>
        public abstract NodeTree Parse(TextSpan span, Lexer lexer);
    }
}