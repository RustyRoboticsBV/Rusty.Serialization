using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic color converter.
    /// </summary>
    public class ColorConverter<T> : TypedConverter<ColorValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new ColorNode(ToColor((T)obj));

        /* Protected methods. */
        protected sealed override object CreateObject(ColorNode node, CreateObjectContext context) => FromColor(node.Value);

        protected sealed override object PopulateObject(ColorNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an color object to the internal value representation.
        /// </summary>
        protected virtual ColorValue ToColor(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an color object of the desired type.
        /// </summary>
        protected virtual T FromColor(ColorValue obj) => FromValue(obj);
    }
}