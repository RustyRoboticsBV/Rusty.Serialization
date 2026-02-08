using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A context for object-node conversions.
    /// </summary>
    public class Converters
    {
        /* Public properties. */
        public ConverterRegistry ConverterRegistry { get; private set; } = new();

        public SymbolTable SymbolTable { get; private set; } = new();
        public CreateNodeContext CreateNodeContext { get; private set; }
        public AssignNodeContext AssignNodeContext { get; private set; }

        public NodeTypeTable NodeTypeTable { get; private set; } = new();
        public ParsingTable ParsingTable { get; private set; } = new();
        public CollectTypesContext CollectTypesContext { get; private set; }
        public CreateObjectContext CreateObjectContext { get; private set; }
        public AssignObjectContext AssignObjectContext { get; private set; }

        /* Constructors. */
        public Converters()
        {
            // Create sub-contexts.
            CreateNodeContext = new(this);
            AssignNodeContext = new(this);
            CollectTypesContext = new(this);
            CreateObjectContext = new(this);
            AssignObjectContext = new(this);

            // Register C# keyword type converters.
            ConverterRegistry.Add<bool, BoolConverter>();
            ConverterRegistry.Add<sbyte, SbyteConverter>();
            ConverterRegistry.Add<short, ShortConverter>();
            ConverterRegistry.Add<int, IntConverter>();
            ConverterRegistry.Add<long, LongConverter>();
            ConverterRegistry.Add<byte, ByteConverter>();
            ConverterRegistry.Add<ushort, UshortConverter>();
            ConverterRegistry.Add<uint, UintConverter>();
            ConverterRegistry.Add<ulong, UlongConverter>();
            ConverterRegistry.Add<float, FloatConverter>();
            ConverterRegistry.Add<double, DoubleConverter>();
            ConverterRegistry.Add<decimal, DecimalConverter>();
            ConverterRegistry.Add<char, CharConverter>();
            ConverterRegistry.Add<string, StringConverter>();
            ConverterRegistry.Add<byte[], ByteArrayConverter>();
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
            // Clear previous resources.
            SymbolTable.Clear();

            // Create node hierarchy.
            INode root = CreateNodeContext.CreateNode(obj);

            // Wrap the root node in a type node.
            INode rootParent = (INode)root.Parent;
            TypeNode typeNode = new TypeNode(new TypeName(typeof(T)), root);
            if (rootParent is IContainerNode container)
                container.ReplaceChild(root, typeNode);

            // Find root node.
            while (root.Parent != null)
            {
                root = (INode)root.Parent;
            }

            // Create node tree.
            return new(root);
        }

        /// <summary>
        /// Convert an object into a node tree.
        /// </summary>
        public NodeTree Convert(object obj, Type type)
        {
            // Clear previous resources.
            SymbolTable.Clear();

            // Create node hierarchy.
            INode root = CreateNodeContext.CreateNode(obj);

            // Wrap the root node in a type node.
            INode rootParent = (INode)root.Parent;
            TypeNode typeNode = new TypeNode(new TypeName(obj != null ? obj.GetType() : type), root);
            if (rootParent is IContainerNode container)
                container.ReplaceChild(root, typeNode);

            // Find root node.
            while (root.Parent != null)
            {
                root = (INode)root.Parent;
            }

            // Create node tree.
            return new(root);
        }

        /// <summary>
        /// Deconvert a node tree into an object.
        /// </summary>
        public T Deconvert<T>(NodeTree tree)
        {
            // Clear previous resources.
            NodeTypeTable.Clear();
            ParsingTable.Clear();

            // Collect the type of each node.
            CollectTypesContext.CollectTypesAndReferences(tree.Root, typeof(T));
            NodeTypeTable.ResolveRefs();

            // Deconvert.
            return CreateObjectContext.CreateObject<T>(tree.Root);
        }

        /// <summary>
        /// Deconvert a node tree into an object.
        /// </summary>
        public object Deconvert(Type type, NodeTree tree)
        {
            // Clear previous resources.
            NodeTypeTable.Clear();
            ParsingTable.Clear();

            // Collect the type of each node.
            CollectTypesContext.CollectTypesAndReferences(tree.Root, type);
            NodeTypeTable.ResolveRefs();

            // Deconvert.
            return CreateObjectContext.CreateObject(type, tree.Root);
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
                throw new ArgumentException("Cannot parse node tree due to ambiguous root type:\n" + tree);
        }
    }
}