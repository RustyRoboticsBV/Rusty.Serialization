using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Rusty.Serialization.Core.Nodes
{
    public static class NodeFactory
    {
        /* Fields. */
        private static NodeBag<AddressNode> Address { get; } = new();
        private static NodeBag<TypeNode> Type { get; } = new();
        private static NodeBag<ScopeNode> Scope { get; } = new();
        private static NodeBag<OffsetNode> Offset { get; } = new();
        private static NodeBag<NullNode> Null { get; } = new();
        private static NodeBag<BoolNode> Bool { get; } = new();
        private static NodeBag<IntNode> Int { get; } = new();
        private static NodeBag<FloatNode> Float { get; } = new();
        private static NodeBag<InfinityNode> Infinity { get; } = new();
        private static NodeBag<NanNode> Nan { get; } = new();
        private static NodeBag<CharNode> Char { get; } = new();
        private static NodeBag<StringNode> String { get; } = new();
        private static NodeBag<DecimalNode> Decimal { get; } = new();
        private static NodeBag<ColorNode> Color { get; } = new();
        private static NodeBag<UidNode> Uid { get; } = new();
        private static NodeBag<TimestampNode> Timestamp { get; } = new();
        private static NodeBag<DurationNode> Duration { get; } = new();
        private static NodeBag<BytesNode> Bytes { get; } = new();
        private static NodeBag<SymbolNode> Symbol { get; } = new();
        private static NodeBag<RefNode> Ref { get; } = new();
        private static NodeBag<ListNode> List { get; } = new();
        private static NodeBag<DictNode> Dict { get; } = new();
        private static NodeBag<ObjectNode> Object { get; } = new();

        private static readonly object @lock = new object();

        /* Public methods. */
        /// <summary>
        /// Rent a node from the bag.
        /// </summary>
        public static NodeT Rent<NodeT>()
            where NodeT : INode
        {
            Type type = typeof(NodeT);
            lock (@lock)
            {
                switch (type.Name)
                {
                    case "AddressNode": return (NodeT)(INode)Address.Rent();
                    case "TypeNode": return (NodeT)(INode)Type.Rent();
                    case "ScopeNode": return (NodeT)(INode)Scope.Rent();
                    case "OffsetNode": return (NodeT)(INode)Offset.Rent();
                    case "NullNode": return (NodeT)(INode)Null.Rent();
                    case "BoolNode": return (NodeT)(INode)Bool.Rent();
                    case "IntNode": return (NodeT)(INode)Int.Rent();
                    case "FloatNode": return (NodeT)(INode)Float.Rent();
                    case "InfinityNode": return (NodeT)(INode)Infinity.Rent();
                    case "CharNode": return (NodeT)(INode)Char.Rent();
                    case "StringNode": return (NodeT)(INode)String.Rent();
                    case "DecimalNode": return (NodeT)(INode)Decimal.Rent();
                    case "ColorNode": return (NodeT)(INode)Color.Rent();
                    case "UidNode": return (NodeT)(INode)Uid.Rent();
                    case "TimestampNode": return (NodeT)(INode)Timestamp.Rent();
                    case "DurationNode": return (NodeT)(INode)Duration.Rent();
                    case "BytesNode": return (NodeT)(INode)Bytes.Rent();
                    case "SymbolNode": return (NodeT)(INode)Symbol.Rent();
                    case "RefNode": return (NodeT)(INode)Ref.Rent();
                    case "ListNode": return (NodeT)(INode)List.Rent();
                    case "DictNode": return (NodeT)(INode)Dict.Rent();
                    case "ObjectNode": return (NodeT)(INode)Object.Rent();
                    default: throw new ArgumentException($"Invalid node type '{type}".");
                }
            }
        }

        /// <summary>
        /// Return a node.
        /// </summary>
        public static void Return(INode node)
        {
            lock (@lock)
            {
                switch (node)
                {
                    case AddressNode address: Address.Return(address); break;
                    case TypeNode type: Type.Return(type); break;
                    case ScopeNode scope: Scope.Return(scope); break;
                    case OffsetNode offset: Offset.Return(offset); break;
                    case NullNode @null: Null.Return(@null); break;
                    case BoolNode @bool: Bool.Return(@bool); break;
                    case IntNode @int: Int.Return(@int); break;
                    case FloatNode @float: Float.Return(@float); break;
                    case InfinityNode infinity: Infinity.Return(infinity); break;
                    case NanNode nan: Nan.Return(nan); break;
                    case CharNode @char: Char.Return(@char); break;
                    case StringNode @string: String.Return(@string); break;
                    case DecimalNode @decimal: Decimal.Return(@decimal); break;
                    case ColorNode color: Color.Return(color); break;
                    case UidNode uid: Uid.Return(uid); break;
                    case TimestampNode timestamp: Timestamp.Return(timestamp); break;
                    case DurationNode duration: Duration.Return(duration); break;
                    case BytesNode bytes: Bytes.Return(bytes); break;
                    case SymbolNode symbol: Symbol.Return(symbol); break;
                    case RefNode @ref: Ref.Return(@ref); break;
                    case ListNode list: List.Return(list); break;
                    case DictNode dict: Dict.Return(dict); break;
                    case ObjectNode @object: Object.Return(@object); break;
                    default: throw new ArgumentException($"Invalid node type '{node.GetType()}".");
                }
            }
        }

        /// <summary>
        /// Delete all free nodes. Rented nodes are not deleted.
        /// </summary>
        public static void Clear()
        {
            lock (@lock)
            {
                Address.Clear();
                Type.Clear();
                Scope.Clear();
                Offset.Clear();
                Null.Clear();
                Bool.Clear();
                Int.Clear();
                Float.Clear();
                Infinity.Clear();
                Nan.Clear();
                Char.Clear();
                String.Clear();
                Decimal.Clear();
                Color.Clear();
                Uid.Clear();
                Timestamp.Clear();
                Duration.Clear();
                Bytes.Clear();
                Symbol.Clear();
                Ref.Clear();
                List.Clear();
                Dict.Clear();
                Object.Clear();
            }
        }
    }
}