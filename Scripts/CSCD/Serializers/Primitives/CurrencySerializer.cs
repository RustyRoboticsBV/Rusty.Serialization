using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD currency serializer.
    /// </summary>
    public class CurrencySerializer : Serializer<CurrencyNode>
    {
        /* Public methods. */
        public override string Serialize(CurrencyNode node, ISerializerScheme scheme)
        {
            // Parse raw decimal.
            string text = node.Value.ToString();

            // Handle 0.
            if (text == "0")
                return "$";
            if (text == "-0")
                return "-$";

            // Remove leading zeroes.
            while (text.StartsWith("-0"))
            {
                text = '-' + text.Substring(2);
            }
            while (text.StartsWith('0'))
            {
                text = text.Substring(1);
            }

            return '$' + text;
        }

        public override CurrencyNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Ensure dollar sign.
                if (!text.StartsWith('$') && !text.StartsWith("-$"))
                    throw new FormatException("Missing $");

                // Handle $ and -$ strings.
                if (trimmed == "$")
                    return new CurrencyNode(0m);
                if (trimmed == "-$")
                    return new CurrencyNode(-0m);

                // Check syntax.
                bool foundDot = false;
                int i = 0;
                if (text.StartsWith('$'))
                    i++;
                else if (text.StartsWith("-$"))
                    i += 2;
                for (; i < trimmed.Length; i++)
                {
                    if (trimmed[i] == '.')
                    {
                        if (!foundDot)
                            foundDot = true;
                        else
                            throw new ArgumentException("Multiple decimal points.");
                    }
                    else if (trimmed[i] < '0' || trimmed[i] > '9')
                        throw new ArgumentException($"Illegal character '{trimmed[i]}' at {i}.");
                }

                // Parse.
                return new(trimmed);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a currency:\n{ex.Message}");
            }
        }
    }
}