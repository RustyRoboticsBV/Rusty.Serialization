using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic UID converter.
    /// </summary>
    public class UidConverter<T> : TypedConverter<UidValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new UidNode(ToUid((T)obj));

        /* Protected methods. */
        protected sealed override object CreateObject(UidNode node, CreateObjectContext context) => FromUid(node.Value);

        protected sealed override object PopulateObject(UidNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an UID object to the internal value representation.
        /// </summary>
        protected virtual UidValue ToUid(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an UID object of the desired type.
        /// </summary>
        protected virtual T FromUid(UidValue obj) => FromValue(obj);
    }
}