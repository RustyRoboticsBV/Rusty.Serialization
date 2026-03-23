using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for float converters with support for infinities, nan, and the symbols pi and e.
    /// </summary>
    public abstract class RealConverter<T> : TypedConverter<T, INode>
    {
        /* Fields. */
        private static Type[] allowedNodeTypes = new Type[]
        {
            typeof(FloatNode),
            typeof(InfinityNode),
            typeof(NanNode),
            typeof(SymbolNode)
        };

        /* Protected properties. */
        protected override Type[] AllowedNodeTypes => allowedNodeTypes;

        protected abstract T NaN { get; }
        protected abstract T PositiveInfinity { get; }
        protected abstract T NegativeInfinity { get; }
        protected abstract T Pi { get; }
        protected abstract T E { get; }

        /* Public methods. */
        protected override INode CreateNode2(T obj, CreateNodeContext context)
        {
            if (IsNaN(ref obj))
                return new NanNode();
            else if (IsPositiveInfinity(ref obj))
                return new InfinityNode(true);
            else if (IsNegativeInfinity(ref obj))
                return new InfinityNode(false);
            else if (IsPi(ref obj))
                return new SymbolNode("pi");
            else if (IsE(ref obj))
                return new SymbolNode("e");
            else
                return new FloatNode(ToFloat(obj));
        }

        protected override T CreateObject2(INode node, CreateObjectContext context)
        {
            if (node is FloatNode @float)
                return FromFloat(@float.Value);
            if (node is NanNode)
                return NaN;
            if (node is InfinityNode infinity)
            {
                if (infinity.Value.positive)
                    return PositiveInfinity;
                else
                    return NegativeInfinity;
            }
            if (node is SymbolNode symbol)
            {
                if (symbol.Name == "pi")
                    return Pi;
                else if (symbol.Name == "e")
                    return E;
                else
                    throw new ArgumentException($"Unknown symbol '{symbol.Name}'");
            }
            throw new ArgumentException($"Invalid node type:\n'{node}");
        }

        /// <summary>
        /// Get the node value representation of the object.
        /// </summary>
        protected abstract FloatValue ToFloat(T obj);
        /// <summary>
        /// Parse a node value into an object.
        /// </summary>
        protected abstract T FromFloat(FloatValue value);

        /* Protected methods. */
        protected abstract bool IsNaN(ref T value);
        protected abstract bool IsPositiveInfinity(ref T value);
        protected abstract bool IsNegativeInfinity(ref T value);
        protected abstract bool IsPi(ref T value);
        protected abstract bool IsE(ref T value);
    }
}