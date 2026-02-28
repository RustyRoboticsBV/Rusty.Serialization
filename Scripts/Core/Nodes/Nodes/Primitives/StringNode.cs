using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A string serializer node.
    /// </summary>
    public class StringNode : ValueNode<string>
    {
        /* Constructors. */
        public StringNode() : base() { }

        public StringNode(string value) : base(value){}

        /* Public methods. */
        public override string ToString()
        {
            return "string: " + (Value ?? "(null)");
        }

        /* Casting operators. */
        public static implicit operator NullNode(StringNode node)
        {
            if (node.Value.ToLower() == "null")
                return new NullNode();
            throw new InvalidCastException(node.ToString());
        }

        public static implicit operator BoolNode(StringNode node)
        {
            return new BoolNode(BoolValue.Parse(node.Value));
        }

        public static implicit operator IntNode(StringNode node)
        {
            return new IntNode(IntValue.Parse(node.Value));
        }

        public static implicit operator FloatNode(StringNode node)
        {
            return new FloatNode(FloatValue.Parse(node.Value));
        }

        public static implicit operator InfinityNode(StringNode node)
        {
            string str = node.Value.ToLower();
            if (str == "Infinity")
                return new InfinityNode(true);
            if (str == "-Infinity")
                return new InfinityNode(false);
            throw new InvalidCastException(node.ToString());
        }

        public static implicit operator NanNode(StringNode node)
        {
            if (node.Value.ToLower() == "nan")
                return new NanNode();
            throw new InvalidCastException(node.ToString());
        }

        public static implicit operator CharNode(StringNode node)
        {
            return new CharNode(new UnicodePair(node.Value));
        }

        public static implicit operator ColorNode(StringNode node)
        {
            return new ColorNode(ColorValue.Parse(node.Value));
        }

        public static implicit operator UidNode(StringNode node)
        {
            return new UidNode(System.Guid.Parse(node.Value)); // Replace with UidValue.Parse.
        }

        public static implicit operator TimestampNode(StringNode node)
        {
            return new TimestampNode(TimestampValue.Parse(node.Value));
        }

        public static implicit operator DurationNode(StringNode node)
        {
            return new DurationNode(DurationValue.Parse(node.Value));
        }

        public static implicit operator BytesNode(StringNode node)
        {
            return new BytesNode(BytesValue.Parse(node.Value));
        }

        public static implicit operator SymbolNode(StringNode node)
        {
            return new SymbolNode(node.Value);
        }

        public static implicit operator RefNode(StringNode node)
        {
            return new RefNode(node.Value);
        }
    }
}