using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A nullable type converter.
    /// </summary>
    public sealed class NullableConverter<TargetT> : Converter<TargetT?>
        where TargetT : struct
    {
        /* Public methods */
        public override INode Convert(TargetT? obj, IConverterScheme scheme)
        {
            if (obj == null)
                return new NullNode();
            else
                return ConvertNested(obj.Value.GetType(), obj.Value, scheme);
        }

        public override TargetT? Deconvert(INode node, IConverterScheme scheme)
        {
            if (node is NullNode)
                return null;
            else if (node is RefNode @ref)
                return default; // TODO: implement
            else if (node is IdNode id)
                return Deconvert(id.Value, scheme);
            else if (node is TypeNode type)
                return Deconvert(type.Value, scheme);
            else
                return DeconvertNested<TargetT>(node, scheme);
            throw new Exception($"Cannot interpret nodes of type '{node.GetType()}'.");
        }
    }
}