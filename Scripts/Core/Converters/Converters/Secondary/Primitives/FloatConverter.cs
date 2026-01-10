using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A float converter.
    /// </summary>
    public sealed class FloatConverter : Converter<float, RealNode>, IConverter
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
            float target = (float)obj;
            if (float.IsNaN(target))
                return new NanNode();
            else if (float.IsPositiveInfinity(target))
                return new InfinityNode(true);
            else if (float.IsNegativeInfinity(target))
                return new InfinityNode(false);
            else
                return CreateNode(target, context);
        }

        object IConverter.CreateObject(INode node, CreateObjectContext context)
        {
            if (node is RealNode real)
                return CreateObject(real, context);
            if (node is NanNode)
                return float.NaN;
            if (node is InfinityNode infinity)
            {
                if (infinity.Positive)
                    return float.PositiveInfinity;
                else
                    return float.NegativeInfinity;
            }
            throw new ArgumentException("Invalid node type.");
        }

        /* Protected methods. */
        protected override RealNode CreateNode(float obj, CreateNodeContext context) => new(obj);
        protected override float CreateObject(RealNode node, CreateObjectContext context) => float.Parse(node.Value);
    }
}