using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A base class for converter schemes.
    /// </summary>
    public abstract class ConverterScheme : IConverterScheme
    {
        /* Private properties. */
        /// <summary>
        /// The registry of known converter types.
        /// </summary>
        private TypeRegistry Types { get; } = new();
        /// <summary>
        /// The registry of known converter instances.
        /// </summary>
        private InstanceRegistry Instances { get; } = new();
        /// <summary>
        /// The registry of known type aliasses.
        /// </summary>
        private AliasRegistry Aliasses { get; } = new();
        /// <summary>
        /// The symbol table, used to track reference types during conversion.
        /// </summary>
        private SymbolTable SymbolTable { get; } = new();
        /// <summary>
        /// The parsing table, used to track reference types during conversion.
        /// </summary>
        private ParsingTable ParsingTable { get; } = new();

        /* Constructors. */
        public ConverterScheme()
        {
            // Bool types.
            Add<bool, BoolConverter>("bl");

            // Int types.
            Add<sbyte, SbyteConverter>("i8");
            Add<short, ShortConverter>("i16");
            Add<int, IntConverter>("i32");
            Add<long, LongConverter>("i64");
            Add<byte, ByteConverter>("u8");
            Add<ushort, UshortConverter>("u16");
            Add<uint, UintConverter>("u32");
            Add<ulong, UlongConverter>("u64");

            Aliasses.Add(typeof(Enum), "enum");

            // Real types.
            Add<float, FloatConverter>("f32");
            Add<double, DoubleConverter>("f64");
            Add<decimal, DecimalConverter>("dec");

            // Char types.
            Add<char, CharConverter>("chr");

            // String types.
            Add<string, StringConverter>("str");

            // Binary types.
            Add<byte[], ByteArrayConverter>("u8[]");

            // Object types.
            Aliasses.Add(typeof(object), "obj");
        }

        /* Public methods. */
        public void Add(Type targetT, Type converterT, string alias = null)
        {
            Types.Add(targetT, converterT);
            if (alias != null)
                Aliasses.Add(targetT, alias);
        }

        public void Add<TargetT, ConverterT>(string alias = null)
            where ConverterT : IConverter<TargetT>
        {
            Add(typeof(TargetT), typeof(ConverterT), alias);
        }

        public IConverter GetConverter(string targetNameOrAlias)
        {
            Type type;
            if (Aliasses.Has(targetNameOrAlias))
                type = Aliasses.Get(targetNameOrAlias);
            else
            {
                TypeName targetName = new(targetNameOrAlias);
                type = targetName.GetType();
            }
            return GetConverter(type);
        }

        public IConverter GetConverter(Type targetType)
        {
            IConverter instance = Instances.Get(targetType);
            if (instance == null)
            {
                instance = Types.Instantiate(targetType);
                Instances.Add(targetType, instance);
            }
            return instance;
        }

        public NodeTree ConvertToTree(object obj)
        {
            // Clear symbol table.
            SymbolTable.Clear();

            // Create node tree.
            NodeTree tree = new();

            INode node = ConvertToNode(obj);
            while (node.Parent != null)
            {
                node = node.Parent;
            }

            tree.SetRoot(node);

            // Wrap tree root in type node.
            // (Unless it's an ID node, in which we case we insert a type node below it).
            Type objType = obj?.GetType();
            string typeName = GetTypeName(objType);
            if (tree.Root is IdNode idNode)
            {
                TypeNode typeNode = new(typeName, idNode.Value);

                idNode.Value = typeNode;
                typeNode.Parent = idNode;
            }
            else
            {
                TypeNode typeNode = new(typeName, tree.Root);
                tree.Root.Parent = typeNode;

                tree.SetRoot(typeNode);
            }

            return tree;
        }

        public INode ConvertToNode(object obj)
        {
            IConverter converter = GetConverter(obj?.GetType());
            return converter.Convert(obj, this, SymbolTable);
        }

        public T DeconvertTree<T>(NodeTree tree)
        {
            ParsingTable.Clear();
            FindIds(tree);
            return DeconvertNode<T>(tree.Root);
        }

        public object DeconvertTree(Type type, NodeTree tree)
        {
            ParsingTable.Clear();
            FindIds(tree);
            return DeconvertNode(type, tree.Root);
        }

        public T DeconvertNode<T>(INode node)
        {
            IConverter converter = GetConverter(typeof(T));
            return (T)converter.Deconvert(node, this, ParsingTable);
        }

        public object DeconvertNode(Type type, INode node)
        {
            IConverter converter = GetConverter(type);
            return converter.Deconvert(node, this, ParsingTable);
        }

        public TypeName GetTypeName(Type type) => new(type);

        public Type GetTypeFromName(string name)
        {
            //if (Aliasses.Has(name))
            //    return Aliasses.Get(name);

            return new TypeName(name).ToType();
        }

        /* Private methods. */
        private void FindIds(NodeTree tree)
        {
            ParsingTable.Clear();
            FindIds(tree.Root, ParsingTable);
        }

        private void FindIds(INode node, ParsingTable table)
        {
            if (node is IdNode id)
            {
                table.Add(id);
                FindIds(id.Value, table);
            }
            else if (node is TypeNode type)
                FindIds(type.Value, table);
            else if (node is ListNode list)
            {
                for (int i = 0; i < list.Elements.Length; i++)
                {
                    FindIds(list.Elements[i], table);
                }
            }
            else if (node is DictNode dict)
            {
                for (int i = 0; i < dict.Pairs.Length; i++)
                {
                    FindIds(dict.Pairs[i].Key, table);
                    FindIds(dict.Pairs[i].Value, table);
                }
            }
            else if (node is ObjectNode obj)
            {
                for (int i = 0; i < obj.Members.Length; i++)
                {
                    FindIds(obj.Members[i].Value, table);
                }
            }
        }
    }
}