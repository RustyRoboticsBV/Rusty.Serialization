using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.CSCD
{
    /// <summary>
    /// A CSCD char serializer.
    /// </summary>
    public class CharSerializer : Serializer<CharNode>
    {
        /* Public methods. */
        public override string Serialize(CharNode node, ISerializerScheme scheme)
        {
            string str = "";
            if (node.Value == '\t')
                str = "\\t";
            else if (node.Value == '\n')
                str = "\\n";
            else if (!CharUtility.Check(node.Value))
                str = "\\" + UnicodeUtility.Serialize(node.Value) + "\\";
            else
                str = ((char)node.Value).ToString(CultureInfo.InvariantCulture);
            return $"'" + str + "'";
        }

        public override CharNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Enforce quotes.
                if (!(trimmed.StartsWith('\'') && trimmed.EndsWith('\'')))
                    throw new ArgumentException("Missing quotes.");

                // Extract contents.
                string contents = trimmed.Substring(1, trimmed.Length - 2);

                // Convert from the CSCD string.
                string packed = StringUtility.Parse(contents, '\'');

                // Don't allow empty characters.
                if (packed.Length == 0)
                    throw new ArgumentException("Empty character.");

                // Don't allow strings longer than 1.
                if (packed.Length > 1)
                    throw new ArgumentException("Too many characters.");

                // Return finished node.
                return new CharNode(packed[0]);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a character:\n{ex.Message}");
            }
        }
    }
}