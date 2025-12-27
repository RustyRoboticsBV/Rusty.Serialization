using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A context for object-node conversions.
    /// </summary>
    public class ConversionContext
    {
        /* Public properties. */
        public ConverterRegistry Converters { get; private set; } = new();

        public SymbolTable SymbolTable { get; private set; } = new();
        public CreateNodeContext CreateNodeContext { get; private set; }
        public AssignNodeContext AssignNodeContext { get; private set; }

        public NodeTypeTable NodeTypeTable { get; private set; } = new();
        public ParsingTable ParsingTable { get; private set; } = new();
        public CollectTypesContext CollectTypesContext { get; private set; }
        public CreateObjectContext CreateObjectContext { get; private set; }
        public AssignObjectContext AssignObjectContext { get; private set; }

        /* Constructors. */
        public ConversionContext()
        {
            // Create sub-contexts.
            CreateNodeContext = new(this);
            AssignNodeContext = new(this);
            CollectTypesContext = new(this);
            CreateObjectContext = new(this);
            AssignObjectContext = new(this);

            // Register built-in converters.
            Converters.Add<bool, BoolConverter>();
            Converters.Add<sbyte, SbyteConverter>();
            Converters.Add<short, ShortConverter>();
            Converters.Add<int, IntConverter>();
            Converters.Add<long, LongConverter>();
            Converters.Add<byte, ByteConverter>();
            Converters.Add<ushort, UshortConverter>();
            Converters.Add<uint, UintConverter>();
            Converters.Add<ulong, UlongConverter>();
            Converters.Add<float, FloatConverter>();
            Converters.Add<double, DoubleConverter>();
            Converters.Add<decimal, DecimalConverter>();
            Converters.Add<char, CharConverter>();
            Converters.Add<string, StringConverter>();
            Converters.Add<byte[], ByteArrayConverter>();
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
            // Clear previous resources.
            SymbolTable.Clear();

            // Create node hierarchy.
            INode root = CreateNodeContext.CreateNode(obj);

            // Wrap the root node in a type node.
            INode rootParent = (INode)root.Parent;
            TypeNode typeNode = new(new TypeName(obj != null ? obj.GetType() : type), root);
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
        public T Deconvert<T>(NodeTree tree) => (T)Deconvert(typeof(T), tree);

        /// <summary>
        /// Deconvert a node tree into an object.
        /// </summary>
        public object Deconvert(Type type, NodeTree tree)
        {
            // Clear previous resources.
            NodeTypeTable.Clear();
            ParsingTable.Clear();

            // Collect the type of each node.
            CollectTypesContext.CollectTypes(tree.Root, type);
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