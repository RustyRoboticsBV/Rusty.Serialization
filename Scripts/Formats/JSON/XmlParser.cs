using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.XML
{
    /// <summary>
    /// A base class for XML token parsers.
    /// </summary>
    public class XmlParser : Parser<XmlLexer>
    {
        /* Fields. */
        private static readonly Dictionary<string, UnicodePair> simpleEscapes = new Dictionary<string, UnicodePair>
        {
            { "lt", '<' },
            { "gt", '>' },
            { "amp", '&' },
            { "quot", '"' },
            { "apos", '\'' }
        };

        /* Public methods. */
        public override NodeTree Parse(TextSpan text, XmlLexer lexer)
        {
            INode root = null;
            while (lexer.GetNextToken(text, out Token token))
            {
                if (root != null)
                    throw new FormatException($"Token found after root value: {token.ToString()}.");

                root = ParseToken(text, token, lexer);
            }

            // Ensure legal root value.
            INode check = root;
            if (check is IdNode id)
                check = id.Value;
            if (check is TypeNode type)
                check = type.Value;
            if (check is RefNode)
                throw new FormatException("Root values may not be references.");
            if (check == null)
                throw new FormatException("No root value.");

            // Create tree.
            return new NodeTree(root);
        }

        /* Protected methods. */
        protected static INode ParseToken(TextSpan text, Token token, XmlLexer lexer)
        {
            // Must start with <.
            if (token != "<")
                TokenError(token, "Missing < symbol.");



            // Illegal tokens.
            TokenError(token, $"Unexpected token.");
            return null;
        }

        /* Private methods. */
        /// <summary>
        /// Parse an escape sequence (and get the original sequence's length).
        /// </summary>
        private static bool ExtractEscape(TextSpan span, int index, out UnicodePair chr, out int sequenceLength)
        {
            // Must be followed by another character.
            if (index + 1 >= span.Length)
                throw new FormatException($"Unclosed escape sequence at {new string(span)}.");

            char next = span[index + 1];

            if (next == '&')
            {
                int escapeEnd = span.FirstIndexOf(index = 1, ';');
                if (escapeEnd == -1)
                    throw new FormatException($"Unclosed escape sequence at {new string(span)}.");

                int codeLength = escapeEnd - (index + 1);
                TextSpan code = span.Slice(index + 1, codeLength);

                // Simple escape codes.
                if (simpleEscapes.TryGetValue(code.ToString(), out UnicodePair simpleEscape))
                {
                    chr = simpleEscape;
                    sequenceLength = codeLength + 2;
                    return true;
                }

                // Unicode (hex).
                else if (code.StartsWith("x"))
                { }

                // Unicode (decimal).
                else { }
            }

            throw new FormatException($"Bad escape sequence at {new string(span)}.");
        }
    }
}