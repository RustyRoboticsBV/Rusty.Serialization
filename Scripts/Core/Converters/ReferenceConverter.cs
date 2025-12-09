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
            // Handle null.
            if (obj == null)
                return new NullNode();

            // If the object is a present in the symbol table...
            if (table.HasObject(obj))
            {
                // If there was no ID for the object yet, create one and wrap the original node.
                if (!table.HasIdFor(obj))
                {
                    ulong newId = table.CreateId(obj);
                    WrapInId(table.GetNode(obj), newId);
                }

                // Return a reference to the object.
                string idName = table.GetId(obj).ToString();
                return new RefNode(idName);
            }

            // Else, register in symbol table.
            else
                table.Add(obj);

            // Create node.
            NodeT node = CreateNode(obj, scheme, table);
            table.SetNode(obj, node);
            AssignNode(ref node, obj, scheme, table);

            return node;
        }

        public sealed override TargetT Deconvert(INode node, IConverterScheme scheme, ParsingTable table)
        {
            if (node is NullNode)
                return null;
            else if (node is RefNode refNode)
            {
                if (table.HasParsed(refNode.ID))
                    return (TargetT)table.GetParsed(refNode.ID);
                else
                {
                    // TODO: THIS DOES NOT WORK FOR FORWARD AND CYCLIC REFERENCES!!!
                    System.Console.WriteLine(node);
                    return default;
                    throw new ArgumentException("Forward and cyclic references not yet supported.");
                }
            }
            else if (node is TypeNode type)
                return DeconvertNested<TargetT>(type.Value, scheme, table);
            else if (node is IdNode idNode)
            {
                if (table.HasParsed(idNode.Name))
                    return (TargetT)table.GetParsed(idNode.Name);
                else
                {
                    TargetT obj = Deconvert(idNode.Value, scheme, table);
                    table.SetParsed(idNode.Name, obj);
                    return obj;
                }
            }
            else if (node is NodeT typed)
            {
                TargetT obj = CreateObject(typed, scheme, table);
                AssignObject(obj, typed, scheme, table);
                return obj;
            }
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

        /// <summary>
        /// Create a source object from a node and return it.
        /// </summary>
        protected abstract TargetT CreateObject(NodeT node, IConverterScheme scheme, ParsingTable table);
        /// <summary>
        /// Set the values of a created source object according to the state of some INode.
        /// You can use this to separate the creation of the object from the assignment of its values.
        /// </summary>
        protected virtual void AssignObject(TargetT obj, NodeT node, IConverterScheme scheme, ParsingTable table) { }

        /* Private methods. */
        private static IdNode WrapInId(INode node, ulong id)
        {
            string name = id.ToString();
            if (node.Parent != null && node.Parent is ICollectionNode collection)
            {
                IdNode idNode = new(name, null);
                collection.WrapChild(node, idNode);
                return idNode;
            }
            else
                return new(name, node);
        }
    }
}