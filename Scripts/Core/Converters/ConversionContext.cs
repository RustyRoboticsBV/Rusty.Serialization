using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    public class ConversionContext
    {
        /* Public properties. */
        public TypeRegistry ConverterTypes { get; private set; } = new();
        public InstanceRegistry InstanceTypes { get; private set; } = new();
        public SymbolTable SymbolTable { get; private set; } = new();
        public ParsingTable ParsingTable { get; private set; } = new();

        /* Constructors. */
        public ConversionContext()
        {
            ConverterTypes.Add<int, IntConverter>();
            ConverterTypes.Add<string, StringConverter>();
        }

        /* Public methods. */
        /// <summary>
        /// Convert an object into a node tree.
        /// </summary>
        public NodeTree Convert(object obj)
        {
            return Convert(obj, obj.GetType());
        }

        /// <summary>
        /// Convert an object into a node tree.
        /// </summary>
        public NodeTree Convert<T>(T obj)
        {
            return Convert(obj, typeof(T));
        }

        /// <summary>
        /// Convert an object into a node tree.
        /// </summary>
        public NodeTree Convert(object obj, Type type)
        {
            // Create contexts.
            CreateNodeContext createNodeContext = new();
            AssignNodeContext assignNodeContext = new(ConverterTypes, InstanceTypes, SymbolTable, createNodeContext);

            // Create node hierarchy.
            INode root = assignNodeContext.CreateNode(obj);

            // Wrap the root node in a type node.
            INode rootParent = (INode)root.Parent;
            TypeNode typeNode = new(new TypeName(obj.GetType()), root);
            if (rootParent is IContainerNode container)
                container.ReplaceChild(root, typeNode);

            // Find root node.
            while (root.Parent != null)
            {
                root = (INode)root.Parent;
            }

            // Clear symbol table.
            SymbolTable.Clear();

            // Create node tree.
            return new(root);
        }

        /// <summary>
        /// Deconvert a node tree into an object.
        /// </summary>
        public T Deconvert<T>(NodeTree tree) => (T)Deconvert(typeof(T), tree);

        /// <summary>
        /// Deconvert a node tree into an object.
        /// </summary>
        public object Deconvert(Type type, NodeTree tree)
        {
            // Create contexts.
            CreateObjectContext createObjectContext = new(ConverterTypes, InstanceTypes, ParsingTable);
            FixReferencesContext fixReferencesContext = new(ConverterTypes, InstanceTypes, ParsingTable);

            // Deconvert.
            object obj = createObjectContext.CreateObject(type, tree.Root);
            obj = fixReferencesContext.FixReferences(obj, tree.Root);

            // Clear parsing table
            ParsingTable.Clear();

            // Return finished object.
            return obj;
        }

        /// <summary>
        /// Deconvert a node tree into an object.
        /// </summary>
        public object Deconvert(NodeTree tree)
        {
            // ID-type root.
            if (tree.Root is IdNode idNode && idNode.Value is TypeNode idTypeNode)
            {
                Type type = new TypeName(idTypeNode.Name).ToType();
                return Deconvert(type, tree);
            }

            // Type root.
            else if (tree.Root is TypeNode typeNode)
            {
                Type type = new TypeName(typeNode.Name).ToType();
                return Deconvert(type, tree);
            }

            else
                throw new ArgumentException("Cannot parse node trees that don't start with a type.");
        }
    }
}