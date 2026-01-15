using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON type serializer.
    /// </summary>
    public class TypeSerializer : Serializer<TypeNode>
    {
        /* Public methods. */
        public override string Serialize(TypeNode node, ISerializerScheme scheme)
        {
            string name = node.Name.Trim();
            Validate(name);

            if (node.Value == null)
                throw new InvalidOperationException("node value was null.");
            return '{' + $"\"type\":\"{name}\",\"value\":{scheme.Serialize(node.Value, scheme.PrettyPrint)}" + '}';
        }

        public override TypeNode Parse(string text, ISerializerScheme scheme)
        {
            return (TypeNode)scheme.ParseAsNode(text);
        }

        /* Private methods. */
        private static void Validate(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (c < '!' && c > '~' || c == '(' || c == ')')
                    throw new ArgumentException($"Illegal character '{c}' in type '{name}'.");
            }
        }
    }
}