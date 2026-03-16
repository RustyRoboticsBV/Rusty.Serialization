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
        public static explicit operator NullNode(StringNode node)
        {
            if (node.Value.ToLower() == "null")
                return new NullNode();
            throw new InvalidCastException(node.ToString());
        }

        public static explicit operator BoolNode(StringNode node)
        {
            return new BoolNode(BoolValue.Parse(node.Value));
        }

        public static explicit operator BitmaskNode(StringNode node)
        {
            return new BitmaskNode(BitmaskValue.Parse(node.Value));
        }

        public static explicit operator IntNode(StringNode node)
        {
            return new IntNode(IntValue.Parse(node.Value));
        }

        public static explicit operator FloatNode(StringNode node)
        {
            return new FloatNode(FloatValue.Parse(node.Value));
        }

        public static explicit operator InfinityNode(StringNode node)
        {
            string str = node.Value.ToLower();
            if (str == "infinity")
                return new InfinityNode(true);
            if (str == "-infinity")
                return new InfinityNode(false);
            throw new InvalidCastException(node.ToString());
        }

        public static explicit operator NanNode(StringNode node)
        {
            if (node.Value.ToLower() == "nan")
                return new NanNode();
            throw new InvalidCastException(node.ToString());
        }

        public static explicit operator CharNode(StringNode node)
        {
            return new CharNode(new UnicodePair(node.Value));
        }

        public static explicit operator DecimalNode(StringNode node)
        {
            return new DecimalNode(DecimalValue.Parse(node.Value));
        }

        public static explicit operator ColorNode(StringNode node)
        {
            return new ColorNode(ColorValue.Parse(node.Value));
        }

        public static explicit operator UidNode(StringNode node)
        {
            return new UidNode(UidValue.Parse(node.Value));
        }

        public static explicit operator TimestampNode(StringNode node)
        {
            return new TimestampNode(TimestampValue.Parse(node.Value));
        }

        public static explicit operator DurationNode(StringNode node)
        {
            return new DurationNode(DurationValue.Parse(node.Value));
        }

        public static explicit operator BytesNode(StringNode node)
        {
            return new BytesNode(BytesValue.Parse(node.Value));
        }

        public static explicit operator SymbolNode(StringNode node)
        {
            return new SymbolNode(node.Value);
        }

        public static explicit operator RefNode(StringNode node)
        {
            return new RefNode(node.Value);
        }
    }
}