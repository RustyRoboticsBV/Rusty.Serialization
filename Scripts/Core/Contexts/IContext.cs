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
        public string Serialize(object obj);

        /// <summary>
        /// Deserialize an object, using the current converter & serialization schemes..
        /// </summary>
        public object Deserialize(string serialized);
        /// <summary>
        /// Deserialize an object, using the current converter & serialization schemes..
        /// </summary>
        public T Deserialize<T>(string serialized);
        /// <summary>
        /// Deserialize an object, using the current converter & serialization schemes..
        /// </summary>
        public void Deserialize(ref object obj, string serialized);
        /// <summary>
        /// Deserialize an object, using the current converter & serialization schemes..
        /// </summary>
        public void Deserialize<T>(ref T obj, string serialized);
    }
}