using System;

namespace Rusty.Serialization.Core.Contexts
{
    /// <summary>
    /// A serialization context. It can serialize objects and deserialize strings, according to configurable schemes.
    /// </summary>
    public interface IContext
    {
        /* Public methods. */
        /// <summary>
        /// Serialize an object, using the current converter & serialization schemes.
        /// </summary>
        public string Serialize(object obj, bool prettyPrint = false);

        /// <summary>
        /// Deserialize an object, using the current converter & serialization schemes..
        /// </summary>
        public object Deserialize(Type type, string serialized);
        /// <summary>
        /// Deserialize an object, using the current converter & serialization schemes..
        /// </summary>
        public T Deserialize<T>(string serialized);
    }
}