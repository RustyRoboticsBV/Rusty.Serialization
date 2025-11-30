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
            IConverter converter = GetConverter(obj?.GetType());
            return converter.Convert(obj, this);
        }

        public INode Convert<T>(T obj) => Convert(ref obj);

        public INode Convert<T>(ref T obj)
        {
            IConverter converter = GetConverter(obj?.GetType());
            if (converter is IConverter<T> typed)
                return typed.Convert(obj, this);
            else
                return converter.Convert(obj, this);
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

        /// <summary>
        /// Get a type's name.
        /// </summary>
        public TypeName GetTypeName(Type type) => new(type);

        /// <summary>
        /// Get a type from a type name.
        /// </summary>
        public Type GetTypeFromName(string name)
        {
            //if (Aliasses.Has(name))
            //    return Aliasses.Get(name);

            return new TypeName(name).ToType();
        }
    }
}