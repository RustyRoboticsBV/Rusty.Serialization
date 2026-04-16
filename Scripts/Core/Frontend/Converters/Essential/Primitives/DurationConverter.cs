using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic duration converter.
    /// </summary>
    public class DurationConverter<T> : TypedConverter<DurationValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new DurationNode(ToDuration((T)obj));

        /* Protected methods. */
        protected sealed override object CreateObject(DurationNode node, CreateObjectContext context) => FromDuration(node.Value);

        protected sealed override object PopulateObject(DurationNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an duration object to the internal value representation.
        /// </summary>
        protected virtual DurationValue ToDuration(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an duration object of the desired type.
        /// </summary>
        protected virtual T FromDuration(DurationValue obj) => FromValue(obj);
    }
}