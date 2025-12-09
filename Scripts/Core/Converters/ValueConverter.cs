using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic value type converter.
    /// </summary>
    public abstract class ValueConverter<TargetT, NodeT> : Converter<TargetT>
        where TargetT : struct
        where NodeT : INode
    {
        /* Public methods */
        public override INode Convert(TargetT obj, IConverterScheme scheme, SymbolTable table) => ConvertValue(obj, scheme, table);

        public override TargetT Deconvert(INode node, IConverterScheme scheme, ParsingTable table)
        {
            if (node is TypeNode type)
                return DeconvertNested<TargetT>(type.Value, scheme, table);
            if (node is NodeT typed)
                return DeconvertValue(typed, scheme, table);
            throw new Exception($"{GetType().Name} cannot interpret nodes of type '{node.GetType()}'.");
        }

        /* Protected methods. */
        /// <summary>
        /// Convert a value into a node.
        /// </summary>
        protected abstract NodeT ConvertValue(TargetT obj, IConverterScheme scheme, SymbolTable table);
        /// <summary>
        /// Deconvert a node into a value.
        /// </summary>
        protected abstract TargetT DeconvertValue(NodeT node, IConverterScheme scheme, ParsingTable table);
    }
}