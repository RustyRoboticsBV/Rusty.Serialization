using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic bitmaskean converter.
    /// </summary>
    public class BitmaskConverter<T> : TypedConverter<BitmaskValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new BitmaskNode(ToBitmask((T)obj));

        /* Protected methods. */
        protected sealed override object CreateObject(BitmaskNode node, CreateObjectContext context) => FromBitmask(node.Value);

        protected sealed override object PopulateObject(BitmaskNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an bitmaskean object to the internal value representation.
        /// </summary>
        protected virtual BitmaskValue ToBitmask(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an bitmaskean object of the desired type.
        /// </summary>
        protected virtual T FromBitmask(BitmaskValue obj) => FromValue(obj);
    }
}