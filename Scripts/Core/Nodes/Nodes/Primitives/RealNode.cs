using System;
using System.Globalization;

namespace Rusty.Serialization.Nodes
{
    /// <summary>
    /// A real number serializer node.
    /// </summary>
    public readonly struct RealNode : INode
    {
        /* Fields. */
        private readonly decimal value;

        /* Public properties. */
        public readonly decimal Value => value;

        /* Constructors. */
        public RealNode(decimal value)
        {
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return "real: " + value;
        }

        public readonly string Serialize()
        {
            // Handle 0.
            if (value == 0)
                return ".";

            // Parse raw decimal.
            string text = value.ToString(CultureInfo.InvariantCulture);

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

        public static RealNode Parse(string text)
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
                    return new(0);

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
                return new(decimal.Parse(trimmed, CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a float:\n{ex.Message}");
            }
        }
    }
}