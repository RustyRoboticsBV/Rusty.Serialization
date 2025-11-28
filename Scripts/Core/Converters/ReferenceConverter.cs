using System;
using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic reference type converter.
    /// </summary>
    public abstract class ReferenceConverter<TargetT, NodeT> : Converter<TargetT>
        where TargetT : class
        where NodeT : INode
    {
        /* Public methods */
        public override INode Convert(TargetT obj, IConverterScheme scheme) => ConvertRef(obj, scheme);

        public override TargetT Deconvert(INode node, IConverterScheme scheme)
        {
            if (node is NullNode)
                return null;
            if (node is NodeT typed)
                return DeconvertRef(typed, scheme);
            throw new Exception($"Cannot interpret nodes of type '{node.GetType()}'.");
        }

        /* Protected methods. */
        protected abstract NodeT ConvertRef(TargetT obj, IConverterScheme scheme);
        protected abstract TargetT DeconvertRef(NodeT node, IConverterScheme scheme);
    }
}