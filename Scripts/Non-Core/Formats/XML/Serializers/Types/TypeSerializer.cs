using System;
using System.Xml;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.XML
{
    /// <summary>
    /// An XML type serializer.
    /// </summary>
    public class TypeSerializer : Serializer<TypeNode>
    {
        /* Public methods. */
        public override string Serialize(TypeNode node, ISerializerScheme scheme)
        {
            string name = node.Name.Trim();
            Validate(name);

            if (node.Value == null)
                throw new InvalidOperationException("name was null.");
            return XmlUtility.Pack(scheme.Serialize(node.Value), "type", ("name", node.Name));
        }

        public override TypeNode Parse(string text, ISerializerScheme scheme)
        {
            // Remove whitespaces.
            string trimmed = text?.Trim();

            try
            {
                // Empty strings are not allowed.
                if (string.IsNullOrEmpty(trimmed))
                    throw new ArgumentException("Empty string.");

                // Unpack XML.
                XmlDocument doc = new();
                doc.LoadXml(trimmed);

                // Get name.
                string name = doc.ChildNodes[0].Attributes[0].Value;
                Validate(name);

                // Get contents.
                string value = doc.ChildNodes[0].InnerXml;
                INode valueNode = null;
                if (value.Length > 0)
                    valueNode = scheme.Parse(value);

                // Return type node.
                return new(name, valueNode);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not parse string '{text}' as a type:\n{ex.Message}");
            }
        }

        /* Private methods. */
        private static void Validate(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (c < '!' && c > '~' || c == '(' || c == ')')
                    throw new ArgumentException($"Disallowed character '{c}' in type '{name}'.");
            }
        }
    }
}