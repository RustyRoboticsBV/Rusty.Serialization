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
        public NodeTree Convert(object obj)
        {
            Type type = obj.GetType();

            // Create context.
            CreateNodeContext createNodeContext = new(ConverterTypes, InstanceTypes, SymbolTable);

            // Create node hierarchy.
            INode root = createNodeContext.CreateNode(obj);

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

        public T Deconvert<T>(NodeTree tree) => (T)Deconvert(typeof(T), tree);

        public object Deconvert(Type type, NodeTree tree)
        {
            // Create context.
            CreateObjectContext createObjectContext = new(ConverterTypes, InstanceTypes, ParsingTable);

            // Deconvert.
            object obj = createObjectContext.CreateObject(type, tree.Root);
            createObjectContext.AssignObject(obj, tree.Root);
            return obj;
        }

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