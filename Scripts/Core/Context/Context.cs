using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Core.Contexts
{
    /// <summary>
    /// A serialization context. It can be used to serialize and deserialize objects.
    /// It contains serializers for all C# types with syntax-level support: bool, ints, reals, char, string, classes, structs,
    /// enums, arrays, tuples and nullables.
    /// </summary>
    public class Context
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

        /* Constructors. */
        public Context()
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
                TypeName targetName = new(targetNameOrAlias, Aliasses);
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

        /// <summary>
        /// Serialize an object.
        /// </summary>
        public string Serialize(object obj, Type expectedType = null)
        {
            Type objType = obj?.GetType();

            // Get converter.
            IConverter converter = GetConverter(objType);

            // Convert object to node.
            INode node = converter.Convert(obj, this);

            // Wrap in type node if necessary.
            if (objType != expectedType)
                node = new TypeNode(GetTypeName(objType), node);

            // Serialize node.
            return node.Serialize();
        }

        /// <summary>
        /// Serialize an object.
        /// </summary>
        public string Serialize<T>(T obj, Type expectedType = null)
        {
            Type objType = obj?.GetType();

            // Get converter.
            IConverter converter = GetConverter(objType);

            // Convert object to node.
            INode node = converter.Convert(obj, this);

            // Wrap in type node if necessary.
            if (objType != expectedType)
                node = new TypeNode(GetTypeName(objType), node);

            // Serialize node.
            return node.Serialize();
        }

        /// <summary>
        /// Get a type's name.
        /// </summary>
        public TypeName GetTypeName(Type type) => new(type, Aliasses);

        /// <summary>
        /// Get a type from a type name.
        /// </summary>
        public Type GetTypeFromName(string name) => (Type)new TypeName(name, Aliasses);
    }
}