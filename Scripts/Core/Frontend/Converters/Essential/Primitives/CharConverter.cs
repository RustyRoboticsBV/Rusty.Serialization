using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic character converter.
    /// </summary>
    public class CharConverter<T> : TypedConverter<UnicodePair, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new CharNode(ToChar((T)obj));

        /* Protected methods. */
        protected sealed override void CollectChildNodeTypes(CharNode node, CollectTypesContext context) { }

        protected sealed override object CreateObject(CharNode node, CreateObjectContext context) => FromChar(node.Value);

        protected sealed override object PopulateObject(CharNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an character object to the internal value representation.
        /// </summary>
        protected virtual UnicodePair ToChar(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an character object of the desired type.
        /// </summary>
        protected virtual T FromChar(UnicodePair obj) => FromValue(obj);
    }
}