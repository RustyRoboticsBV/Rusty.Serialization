using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Core.Contexts
{
    /// <summary>
    /// A serialization context. It can be used to serialize and deserialize objects.
    /// It contains serializers for all C# types with syntax-level support: bool, ints, reals, char, string, classes, structs,
    /// enums, arrays, tuples and nullables.
    /// </summary>
    public abstract class Context : IContext
    {
        /* Public properties. */
        /// <summary>
        /// The converter scheme of this serialization context.
        /// </summary>
        public virtual IConverterScheme ConverterScheme { get; set; }
        /// <summary>
        /// The serializer scheme of this serialization context.
        /// </summary>
        public virtual ISerializerScheme SerializerScheme { get; set; }

        /* Public methods. */
        /// <summary>
        /// Serialize an object.
        /// </summary>
        public string Serialize(object obj)
        {
            // Convert to node.
            INode node = ConverterScheme.Convert(obj);

            // Wrap in type node.
            Type objType = obj?.GetType();
            string typeName = ConverterScheme.GetTypeName(objType);
            node = new TypeNode(typeName, node);

            // Serialize node.
            return SerializerScheme.Serialize(node);
        }

        public T Deserialize<T>(string serialized)
        {
            INode node = SerializerScheme.Parse(serialized);
            return ConverterScheme.Deconvert<T>(node);
        }

        public object Deserialize(Type type, string serialized)
        {
            INode node = SerializerScheme.Parse(serialized);
            return ConverterScheme.Deconvert(type, node);
        }
    }
}