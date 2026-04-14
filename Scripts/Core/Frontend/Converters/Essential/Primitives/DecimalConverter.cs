using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic decimal converter.
    /// </summary>
    public class DecimalConverter<T> : TypedConverter<DecimalValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new DecimalNode(ToDecimal((T)obj));

        /* Protected methods. */
        protected sealed override void CollectChildNodeTypes(DecimalNode node, CollectTypesContext context) { }

        protected sealed override object CreateObject(DecimalNode node, CreateObjectContext context) => FromDecimal(node.Value);

        protected sealed override object PopulateObject(DecimalNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an decimal object to the internal value representation.
        /// </summary>
        protected virtual DecimalValue ToDecimal(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an decimal object of the desired type.
        /// </summary>
        protected virtual T FromDecimal(DecimalValue obj) => FromValue(obj);
    }
}