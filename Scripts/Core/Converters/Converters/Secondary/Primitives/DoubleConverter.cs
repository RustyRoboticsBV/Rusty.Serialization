using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A double converter.
    /// </summary>
    public sealed class DoubleConverter : Converter<double, RealNode>, IConverter
    {
        /* Public methods. */
        void IConverter.CollectTypes(INode node, CollectTypesContext context)
        {
            if (node is NanNode || node is InfinityNode)
                return;
            CollectTypes((RealNode)node, context);
        }

        INode IConverter.CreateNode(object obj, CreateNodeContext context)
        {
            double target = (double)obj;
            if (double.IsNaN(target))
                return new NanNode();
            else if (double.IsPositiveInfinity(target))
                return new InfinityNode(true);
            else if (double.IsNegativeInfinity(target))
                return new InfinityNode(false);
            else
                return CreateNode(target, context);
        }

        object IConverter.CreateObject(INode node, CreateObjectContext context)
        {
            if (node is RealNode real)
                return CreateObject(real, context);
            if (node is NanNode)
                return double.NaN;
            if (node is InfinityNode infinity)
            {
                if (infinity.Positive)
                    return double.PositiveInfinity;
                else
                    return double.NegativeInfinity;
            }
            throw new ArgumentException("Invalid node type.");
        }

        /* Protected methods. */
        protected override RealNode CreateNode(double obj, CreateNodeContext context) => new(obj);
        protected override double CreateObject(RealNode node, CreateObjectContext context) => double.Parse(node.Value);
    }
}