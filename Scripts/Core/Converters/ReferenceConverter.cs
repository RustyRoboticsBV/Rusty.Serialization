using System;
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
        public override INode Convert(TargetT obj, IConverterScheme scheme, NodeTree tree) => ConvertRef(obj, scheme, tree);

        public override TargetT Deconvert(INode node, IConverterScheme scheme, NodeTree tree)
        {
            if (node is NullNode)
                return null;
            else if (node is RefNode @ref)
            { // TODO: implement.
                if (typeof(TargetT) == typeof(string))
                    return (TargetT)(object)"";
                return default;
            }
            else if (node is TypeNode type)
                return DeconvertNested<TargetT>(type.Value, scheme, tree);
            else if (node is IdNode id)
                return Deconvert(id.Value, scheme, tree);
            else if (node is NodeT typed)
                return DeconvertRef(typed, scheme, tree);
            throw new Exception($"{GetType().Name} cannot interpret node '{node}'.");
        }

        /* Protected methods. */
        protected abstract NodeT ConvertRef(TargetT obj, IConverterScheme scheme, NodeTree tree);
        protected abstract TargetT DeconvertRef(NodeT node, IConverterScheme scheme, NodeTree tree);
    }
}