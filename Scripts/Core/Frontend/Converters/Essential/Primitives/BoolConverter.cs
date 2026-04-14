using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic boolean converter.
    /// </summary>
    public class BoolConverter<T> : TypedConverter<BoolValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new BoolNode(ToBool((T)obj));

        /* Protected methods. */
        protected sealed override object CreateObject(BoolNode node, CreateObjectContext context) => FromBool(node.Value);

        protected sealed override object PopulateObject(BoolNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an boolean object to the internal value representation.
        /// </summary>
        protected virtual BoolValue ToBool(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an boolean object of the desired type.
        /// </summary>
        protected virtual T FromBool(BoolValue obj) => FromValue(obj);
    }
}