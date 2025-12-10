using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD string serializer.
    /// </summary>
    public class ColorSerializer : Serializer<ColorNode>
    {
        /* Public methods. */
        public override string Serialize(ColorNode node, ISerializerScheme scheme)
        {
            // Serialize components as 2-digit hexadecimal strings.
            string r = node.R.ToString("X2");
            string g = node.G.ToString("X2");
            string b = node.B.ToString("X2");
            string a = node.A.ToString("X2");

            // Truncate to CSS style if possible.
            if (r[0] == r[1] && g[0] == g[1] && b[0] == b[1] && a[0] == a[1])
            {
                r = r[0].ToString();
                g = g[0].ToString();
                b = b[0].ToString();
                a = a[0].ToString();
            }

            // Return as #RGB if a is 255.
            if (node.A == 255)
                return $"#{r}{g}{b}";

            // Return as #RGBA in other cases.
            else
                return $"#{r}{g}{b}{a}";
        }

        public override ColorNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Enforce leading hex sign.
                if (!trimmed.StartsWith('#'))
                    throw new ArgumentException("Missing hex sign.");

                // Allow CSS convention.
                if (trimmed.Length == 4)
                {
                    trimmed = trimmed[0]
                        + new string(trimmed[1], 2)
                        + new string(trimmed[2], 2)
                        + new string(trimmed[3], 2);
                }
                else if (trimmed.Length == 5)
                {
                    trimmed = trimmed[0]
                        + new string(trimmed[1], 2)
                        + new string(trimmed[2], 2)
                        + new string(trimmed[3], 2)
                        + new string(trimmed[4], 2);
                }

                // Split into substrs.
                if (trimmed.Length != 7 && trimmed.Length != 9)
                    throw new Exception("Bad length. Use #RGB, #RGBA, #RRGGBB or #RRGGBBAA.");
                string r = trimmed.Substring(1, 2);
                string g = trimmed.Substring(3, 2);
                string b = trimmed.Substring(5, 2);
                string a = trimmed.Length == 9 ? trimmed.Substring(7, 2) : "FF";

                // Parse substrs.
                try
                {
                    return new(
                        byte.Parse(r, NumberStyles.HexNumber),
                        byte.Parse(g, NumberStyles.HexNumber),
                        byte.Parse(b, NumberStyles.HexNumber),
                        byte.Parse(a, NumberStyles.HexNumber)
                    );
                }
                catch
                {
                    throw new ArgumentException("Not a hexadecimal number.");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a color:\n{ex.Message}");
            }
        }
    }
}