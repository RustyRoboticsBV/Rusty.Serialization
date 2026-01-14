using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD real serializer.
    /// </summary>
    public class FloatSerializer : Serializer<FloatNode>
    {
        /* Public methods. */
        public override string Serialize(FloatNode node, ISerializerScheme scheme)
        {
            // Parse raw decimal.
            string text = node.Value.ToString();

            // Handle 0.
            if (text == "0")
                return ".";

            // Make sure a decimal point exists.
            if (!text.Contains('.'))
                text += ".";

            // Remove leading zeroes.
            while (text.StartsWith("-0"))
            {
                text = '-' + text.Substring(2);
            }
            while (text.StartsWith('0'))
            {
                text = text.Substring(1);
            }

            // Remove trailing zeroes.
            while (text.EndsWith('0'))
            {
                text = text.Substring(0, text.Length - 1);
            }

            return text;
        }

        public override FloatNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Handle . and -. strings.
                if (trimmed == "." || trimmed == "-.")
                    return new(0f);

                // Check syntax.
                bool foundDot = false;
                for (int i = 0; i < trimmed.Length; i++)
                {
                    if (trimmed[i] == '.')
                    {
                        if (!foundDot)
                            foundDot = true;
                        else
                            throw new ArgumentException("Multiple decimal points.");
                    }
                    else if (!((i == 0 && trimmed[i] == '-') || (trimmed[i] >= '0' && trimmed[i] <= '9')))
                        throw new ArgumentException($"Illegal character '{trimmed[i]}' at {i}.");
                }
                if (!foundDot)
                    throw new ArgumentException("Missing decimal dot.");

                // Parse.
                return new(trimmed);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a float:\n{ex.Message}");
            }
        }
    }
}