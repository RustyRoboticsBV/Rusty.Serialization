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
        public sealed override INode Convert(TargetT obj, IConverterScheme scheme, SymbolTable table)
        {
            NodeT node = CreateNode(obj, scheme, table);
            AssignNode(ref node, obj, scheme, table);
            return node;
        }

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
        /// <summary>
        /// Create a node object and return it.
        /// </summary>
        protected abstract NodeT CreateNode(TargetT obj, IConverterScheme scheme, SymbolTable table);
        /// <summary>
        /// Set the values of a blank INode according to the internal state of some object.
        /// You can use this to separate the creation of the node from the assignment of its values.
        /// </summary>
        protected virtual void AssignNode(ref NodeT node, TargetT obj, IConverterScheme scheme, SymbolTable table) { }
        protected abstract TargetT DeconvertRef(NodeT node, IConverterScheme scheme, NodeTree tree);
    }
}