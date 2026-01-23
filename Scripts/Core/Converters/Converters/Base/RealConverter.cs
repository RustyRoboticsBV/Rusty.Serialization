using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A real converter.
    /// </summary>
    public abstract class RealConverter<T> : Converter<T, FloatNode>, IConverter
    {
        /* Protected properties. */
        protected abstract T NaN { get; }
        protected abstract T PositiveInfinity { get; }
        protected abstract T NegativeInfinity { get; }

        /* Public methods. */
        void IConverter.CollectTypes(INode node, CollectTypesContext context)
        {
            if (!CanHandleNode(node))
                NodeError(node);
            if (node is NanNode || node is InfinityNode)
                return;
            CollectTypes((FloatNode)node, context);
        }

        INode IConverter.CreateNode(object obj, CreateNodeContext context)
        {
            T target = (T)obj;
            if (IsNaN(ref target))
                return new NanNode();
            else if (IsPositiveInfinity(ref target))
                return new InfinityNode(true);
            else if (IsNegativeInfinity(ref target))
                return new InfinityNode(false);
            else
                return CreateNode(target, context);
        }

        object IConverter.CreateObject(INode node, CreateObjectContext context)
        {
            if (!CanHandleNode(node))
                NodeError(node);
            if (node is FloatNode @float)
                return CreateObject(@float, context);
            if (node is NanNode)
                return NaN;
            if (node is InfinityNode infinity)
            {
                if (infinity.Positive)
                    return PositiveInfinity;
                else
                    return NegativeInfinity;
            }
            throw new ArgumentException("Invalid node type.");
        }

        /* Protected methods. */
        protected override bool CanHandleNode(INode node)
        {
            return base.CanHandleNode(node) || node is FloatNode || node is InfinityNode || node is NanNode;
        }

        protected abstract bool IsNaN(ref T value);
        protected abstract bool IsPositiveInfinity(ref T value);
        protected abstract bool IsNegativeInfinity(ref T value);
    }
}