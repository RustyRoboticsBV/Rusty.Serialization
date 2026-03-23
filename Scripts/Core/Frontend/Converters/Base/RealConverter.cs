using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A real converter.
    /// </summary>
    public abstract class RealConverter<T> : Converter<T, INode>
    {
        /* Protected properties. */
        protected abstract T NaN { get; }
        protected abstract T PositiveInfinity { get; }
        protected abstract T NegativeInfinity { get; }
        protected abstract T Pi { get; }
        protected abstract T E { get; }

        /* Constructors. */
        public RealConverter()
        {
            AllowedNodeTypes = new Type[]
            {
                typeof(FloatNode),
                typeof(InfinityNode),
                typeof(NanNode),
                typeof(SymbolNode)
            };
        }

        /* Public methods. */
        protected override INode CreateNode(T obj, CreateNodeContext context)
        {
            T target = (T)obj;
            if (IsNaN(ref target))
                return new NanNode();
            else if (IsPositiveInfinity(ref target))
                return new InfinityNode(true);
            else if (IsNegativeInfinity(ref target))
                return new InfinityNode(false);
            else if (IsPi(ref target))
                return new SymbolNode("pi");
            else if (IsE(ref target))
                return new SymbolNode("e");
            else
                return CreateNode(target);
        }

        protected override T CreateObject(INode node, CreateObjectContext context)
        {
            if (node is FloatNode @float)
                return CreateObject(@float);
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

        protected abstract FloatNode CreateNode(T obj);
        protected abstract T CreateObject(FloatNode node);

        /* Protected methods. */
        protected abstract bool IsNaN(ref T value);
        protected abstract bool IsPositiveInfinity(ref T value);
        protected abstract bool IsNegativeInfinity(ref T value);
        protected abstract bool IsPi(ref T value);
        protected abstract bool IsE(ref T value);
    }
}