using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A node object pool.
    /// </summary>
    public static class NodePool
    {
        /* Fields. */
        private static Dictionary<Type, INodeBag> NodeBags { get; } = new();

        /* Constructors. */
        static NodePool()
        {
            NodeBags.Add(typeof(AddressNode), new NodeBag<AddressNode>());
            NodeBags.Add(typeof(TypeNode), new NodeBag<TypeNode>());
            NodeBags.Add(typeof(ScopeNode), new NodeBag<ScopeNode>());
            NodeBags.Add(typeof(OffsetNode), new NodeBag<OffsetNode>());
            NodeBags.Add(typeof(NullNode), new NodeBag<NullNode>());
            NodeBags.Add(typeof(BoolNode), new NodeBag<BoolNode>());
            NodeBags.Add(typeof(IntNode), new NodeBag<IntNode>());
            NodeBags.Add(typeof(FloatNode), new NodeBag<FloatNode>());
            NodeBags.Add(typeof(InfinityNode), new NodeBag<InfinityNode>());
            NodeBags.Add(typeof(NanNode), new NodeBag<NanNode>());
            NodeBags.Add(typeof(CharNode), new NodeBag<CharNode>());
            NodeBags.Add(typeof(StringNode), new NodeBag<StringNode>());
            NodeBags.Add(typeof(DecimalNode), new NodeBag<DecimalNode>());
            NodeBags.Add(typeof(ColorNode), new NodeBag<ColorNode>());
            NodeBags.Add(typeof(UidNode), new NodeBag<UidNode>());
            NodeBags.Add(typeof(TimestampNode), new NodeBag<TimestampNode>());
            NodeBags.Add(typeof(DurationNode), new NodeBag<DurationNode>());
            NodeBags.Add(typeof(BytesNode), new NodeBag<BytesNode>());
            NodeBags.Add(typeof(SymbolNode), new NodeBag<SymbolNode>());
            NodeBags.Add(typeof(RefNode), new NodeBag<RefNode>());
            NodeBags.Add(typeof(ListNode), new NodeBag<ListNode>());
            NodeBags.Add(typeof(DictNode), new NodeBag<DictNode>());
            NodeBags.Add(typeof(ObjectNode), new NodeBag<ObjectNode>());
        }

        /* Public methods. */
        /// <summary>
        /// Rent a node from the bag.
        /// </summary>
        public static NodeT Rent<NodeT>()
            where NodeT : INode
        {
            Type type = typeof(NodeT);
            return (NodeT)NodeBags[type].Rent();
        }

        /// <summary>
        /// Return a node.
        /// </summary>
        public static void Return(INode node)
        {
            if (node == null)
                return;

            Type type = node.GetType();
            NodeBags[type].Return(node);
        }

        /// <summary>
        /// Delete all free nodes. Rented nodes are not deleted.
        /// </summary>
        public static void Clear()
        {
            foreach (INodeBag bag in NodeBags.Values)
            {
                bag.Clear();
            }
        }

        /// <summary>
        /// Rent and initialize an address node.
        /// </summary>
        public static AddressNode RentAddress(string name, INode child)
        {
            AddressNode address = Rent<AddressNode>();
            address.Name = name;
            address.Value = child;
            if (child != null)
                child.Parent = address;
            return address;
        }

        /// <summary>
        /// Rent and initialize a type node.
        /// </summary>
        public static TypeNode RentType(string name, INode child)
        {
            if (child == null)
                throw new Exception();
            TypeNode type = Rent<TypeNode>();
            type.Name = name;
            type.Value = child;
            if (child != null)
                child.Parent = type;
            return type;
        }

        /// <summary>
        /// Rent and initialize a type node.
        /// </summary>
        public static ScopeNode RentScope(string name, SymbolNode child)
        {
            ScopeNode scope = Rent<ScopeNode>();
            scope.Name = name;
            scope.Value = child;
            if (child != null)
                child.Parent = scope;
            return scope;
        }

        /// <summary>
        /// Rent and initialize a type node.
        /// </summary>
        public static OffsetNode RentOffset(OffsetValue value, TimestampNode child)
        {
            OffsetNode offset = Rent<OffsetNode>();
            offset.Offset = value;
            offset.Time = child;
            if (child != null)
                child.Parent = offset;
            return offset;
        }

        /// <summary>
        /// Rent and initialize a null node.
        /// </summary>
        public static NullNode RentNull()
        {
            return Rent<NullNode>();
        }

        /// <summary>
        /// Rent and initialize a bool node.
        /// </summary>
        public static BoolNode RentBool(BoolValue value)
        {
            BoolNode @bool = Rent<BoolNode>();
            @bool.Value = value;
            return @bool;
        }

        /// <summary>
        /// Rent and initialize a int node.
        /// </summary>
        public static IntNode RentInt(IntValue value)
        {
            IntNode @int = Rent<IntNode>();
            @int.Value = value;
            return @int;
        }

        /// <summary>
        /// Rent and initialize a float node.
        /// </summary>
        public static FloatNode RentFloat(FloatValue value)
        {
            FloatNode @float = Rent<FloatNode>();
            @float.Value = value;
            return @float;
        }

        /// <summary>
        /// Rent and initialize an infinity node.
        /// </summary>
        public static InfinityNode RentInfinity(bool positive)
        {
            InfinityNode infinity = Rent<InfinityNode>();
            infinity.Positive = positive;
            return infinity;
        }

        /// <summary>
        /// Rent and initialize a nan node.
        /// </summary>
        public static NanNode RentNan()
        {
            return Rent<NanNode>();
        }

        /// <summary>
        /// Rent and initialize an char node.
        /// </summary>
        public static CharNode RentChar(UnicodePair value)
        {
            CharNode @char = Rent<CharNode>();
            @char.Value = value;
            return @char;
        }

        /// <summary>
        /// Rent and initialize an string node.
        /// </summary>
        public static StringNode RentString(string value)
        {
            StringNode @string = Rent<StringNode>();
            @string.Value = value;
            return @string;
        }

        /// <summary>
        /// Rent and initialize a decimal node.
        /// </summary>
        public static DecimalNode RentDecimal(DecimalValue value)
        {
            DecimalNode @decimal = Rent<DecimalNode>();
            @decimal.Value = value;
            return @decimal;
        }

        /// <summary>
        /// Rent and initialize a color node.
        /// </summary>
        public static ColorNode RentColor(ColorValue value)
        {
            ColorNode color = Rent<ColorNode>();
            color.Value = value;
            return color;
        }

        /// <summary>
        /// Rent and initialize a uid node.
        /// </summary>
        public static UidNode RentUid(Guid value)
        {
            UidNode uid = Rent<UidNode>();
            uid.Value = value;
            return uid;
        }

        /// <summary>
        /// Rent and initialize a timestamp node.
        /// </summary>
        public static TimestampNode RentTimestamp(TimestampValue value)
        {
            TimestampNode timestamp = Rent<TimestampNode>();
            timestamp.Value = value;
            return timestamp;
        }

        /// <summary>
        /// Rent and initialize a duration node.
        /// </summary>
        public static DurationNode RentDuration(DurationValue value)
        {
            DurationNode duration = Rent<DurationNode>();
            duration.Value = value;
            return duration;
        }

        /// <summary>
        /// Rent and initialize a bytes node.
        /// </summary>
        public static BytesNode RentBytes(BytesValue value)
        {
            BytesNode bytes = Rent<BytesNode>();
            bytes.Value = value;
            return bytes;
        }

        /// <summary>
        /// Rent and initialize a symbol node.
        /// </summary>
        public static SymbolNode RentSymbol(string name)
        {
            SymbolNode symbol = Rent<SymbolNode>();
            symbol.Name = name;
            return symbol;
        }

        /// <summary>
        /// Rent and initialize a ref node.
        /// </summary>
        public static RefNode RentRef(string address)
        {
            RefNode @ref = Rent<RefNode>();
            @ref.Address = address;
            return @ref;
        }
    }
}