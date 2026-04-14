using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic integer converter.
    /// </summary>
    public class IntConverter<T> : TypedConverter<IntValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new IntNode(ToInt((T)obj));

        /* Protected methods. */
        protected sealed override void CollectChildNodeTypes(IntNode node, CollectTypesContext context) { }

        protected sealed override object CreateObject(IntNode node, CreateObjectContext context) => FromInt(node.Value);

        protected sealed override object PopulateObject(IntNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an integer object to the internal value representation.
        /// </summary>
        protected virtual IntValue ToInt(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an integer object of the desired type.
        /// </summary>
        protected virtual T FromInt(IntValue obj) => FromValue(obj);
    }
}