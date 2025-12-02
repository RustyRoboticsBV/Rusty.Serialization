using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML string serializer.
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
                return XmlUtility.Pack($"#{r}{g}{b}", "color");

            // Return as #RGBA in other cases.
            else
                return XmlUtility.Pack($"#{r}{g}{b}{a}", "color");
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

                // Unpack XML.
                string contents = XmlUtility.Unpack(trimmed, "color");

                // Enforce leading hex sign.
                if (!contents.StartsWith('#'))
                    throw new ArgumentException("Missing hex sign.");

                // Allow CSS convention.
                if (contents.Length == 4)
                {
                    contents = contents[0]
                        + new string(contents[1], 2)
                        + new string(contents[2], 2)
                        + new string(contents[3], 2);
                }
                else if (contents.Length == 5)
                {
                    contents = contents[0]
                        + new string(contents[1], 2)
                        + new string(contents[2], 2)
                        + new string(contents[3], 2)
                        + new string(contents[4], 2);
                }

                // Split into substrs.
                if (contents.Length != 7 && contents.Length != 9)
                    throw new Exception("Bad length. Use #RGB, #RGBA, #RRGGBB or #RRGGBBAA.");
                string r = contents.Substring(1, 2);
                string g = contents.Substring(3, 2);
                string b = contents.Substring(5, 2);
                string a = contents.Length == 9 ? contents.Substring(7, 2) : "FF";

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