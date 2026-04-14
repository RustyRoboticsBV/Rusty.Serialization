using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic floating-point converter.
    /// </summary>
    public class FloatConverter<T> : TypedConverter<FloatValue, T>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context)
        {
            T value = (T)obj;
            if (IsNan(value))
                return new NanNode();
            else if (IsPositiveInfinity(value))
                return new InfinityNode(true);
            else if (IsNegativeInfinity(value))
                return new InfinityNode(false);
            else
                return new FloatNode(ToFloat(value));
        }

        /* Protected methods. */
        protected sealed override object CreateObject(FloatNode node, CreateObjectContext context) => FromFloat(node.Value);

        protected sealed override object PopulateObject(FloatNode node, object obj, PopulateObjectContext context) => obj;

        /// <summary>
        /// Convert an floating-point object to the internal value representation.
        /// </summary>
        protected virtual FloatValue ToFloat(T obj) => ToValue(obj);

        /// <summary>
        /// Deconvert a value to an floating-point object of the desired type.
        /// </summary>
        protected virtual T FromFloat(FloatValue obj) => FromValue(obj);

        /// <summary>
        /// Check if a float equals NaN.
        /// </summary>
        protected virtual bool IsNan(T obj) => false;

        /// <summary>
        /// Check if a float equals positive infinity.
        /// </summary>
        protected virtual bool IsPositiveInfinity(T obj) => false;

        /// <summary>
        /// Check if a float equals negative infinity.
        /// </summary>
        protected virtual bool IsNegativeInfinity(T obj) => false;
    }
}