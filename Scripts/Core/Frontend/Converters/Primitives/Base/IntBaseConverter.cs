using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for integer converter.
    /// </summary>
    public abstract class IntBaseConverter<T> : Converter
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context)
        {
            return new IntNode(ToInt((T)obj));
        }

        /* Protected methods. */
        protected sealed override void CollectChildNodeTypes(IntNode node, CollectTypesContext context) { }

        protected sealed override object CreateObject(IntNode node, CreateObjectContext context) => FromInt(node.Value);

        protected sealed override object PopulateObject(IntNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an integer object to the internal IntValue representation.
        /// </summary>
        protected abstract IntValue ToInt(T obj);

        /// <summary>
        /// Deconvert a IntValue to an integer object of the desired type.
        /// </summary>
        protected abstract T FromInt(IntValue obj);
    }
}