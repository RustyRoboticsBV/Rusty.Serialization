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
        public virtual IConverterScheme ConverterScheme { get; set; }
        public virtual ISerializerScheme SerializerScheme { get; set; }

        /* Public methods. */
        /// <summary>
        /// Serialize an object.
        /// </summary>
        public string Serialize(object obj, bool prettyPrint = false)
        {
            // Convert to node.
            NodeTree node = ConverterScheme.ConvertToTree(obj);

            // Serialize node.
            SerializerScheme.PrettyPrint = prettyPrint;
            return SerializerScheme.Serialize(node);
        }

        public T Deserialize<T>(string serialized)
        {
            NodeTree tree = SerializerScheme.ParseAsTree(serialized);
            return ConverterScheme.DeconvertTree<T>(tree);
        }

        public object Deserialize(Type type, string serialized)
        {
            NodeTree tree = SerializerScheme.ParseAsTree(serialized);
            return ConverterScheme.DeconvertTree(type, tree);
        }
    }
}