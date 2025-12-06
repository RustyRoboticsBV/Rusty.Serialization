using Rusty.Serialization.Core.Nodes;
using System;

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
        private SymbolTable SymbolTable { get; set; }

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

        public INode Convert(object obj)
        {
            // If the symbol table is null, this object is a root.
            bool isRoot = false;
            if (SymbolTable == null)
            {
                SymbolTable = new();
                isRoot = true;
            }

            // If the object is a reference and is present in the symbol table...
            bool isReferenceType = obj != null && !obj.GetType().IsValueType;
            if (isReferenceType && SymbolTable.HasObject(obj))
            {
                // If there was no ID for the object yet, create one and wrap the original node.
                if (!SymbolTable.HasIdFor(obj))
                    WrapInId(SymbolTable.GetNode(obj), SymbolTable.GetOrCreateId(obj));

                // Return a reference to the object.
                INode @ref = new RefNode(SymbolTable.GetOrCreateId(obj).ToString());

                if (isRoot)
                {
                    NodeTree tree = new(@ref);
                    return tree;
                }
                else
                    return @ref;
            }

            // Convert the object.
            IConverter converter = GetConverter(obj?.GetType());
            INode node = converter.Convert(obj, this);

            // Register in symbol table (if it's a reference type an it wasn't registered yet).
            if (isReferenceType && !SymbolTable.HasObject(obj))
                SymbolTable.Add(obj, node);

            if (isRoot)
            {
                NodeTree tree = new(node);
                return tree;
            }
            else
                return node;
        }

        public T Deconvert<T>(INode node)
        {
            IConverter converter = GetConverter(typeof(T));
            return (T)converter.Deconvert(node, this);
        }

        public object Deconvert(Type type, INode node)
        {
            IConverter converter = GetConverter(type);
            return converter.Deconvert(node, this);
        }

        public TypeName GetTypeName(Type type) => new(type);

        public Type GetTypeFromName(string name)
        {
            //if (Aliasses.Has(name))
            //    return Aliasses.Get(name);

            return new TypeName(name).ToType();
        }

        public void ClearSymbolTable()
        {
            SymbolTable = null;
        }

        /* Private methods. */
        private static IdNode WrapInId(INode node, ulong id)
        {
            if (node.Parent != null && node.Parent is ICollectionNode collection)
            {
                IdNode idNode = new(id.ToString(), null);
                collection.WrapChild(node, idNode);
                return idNode;
            }
            else // TODO: This throws an exception if the root node is ID-wrapped.
                throw new ArgumentException($"Node has no parent: " + node);
        }
    }
}