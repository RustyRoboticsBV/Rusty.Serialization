using Rusty.Serialization.Core.Nodes;
using System;
using System.Reflection;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for integer converter.
    /// </summary>
    public class IntBaseConverter<T> : Converter
    {
        /* Private properties. */
        private MethodInfo ToConversionOperator { get; set; }
        private MethodInfo FromConversionOperator { get; set; }

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
        protected virtual IntValue ToInt(T obj)
        {
            // If needed, find the conversion operator.
            if (ToConversionOperator == null)
            {
                if (CastUtility.TryFindCast(typeof(T), typeof(IntValue), out MethodInfo method))
                    ToConversionOperator = method;
            }

            if (ToConversionOperator == null)
            {
                throw new InvalidCastException($"Objects of type '{typeof(T).Name}' cannot be converted to "
                    + $"'{typeof(IntValue).Name}'.");
            }

            // If we found the conversion operator, invoke it.
            return (IntValue)ToConversionOperator.Invoke(null, new object[1] { obj });
        }

        /// <summary>
        /// Deconvert a IntValue to an integer object of the desired type.
        /// </summary>
        protected virtual T FromInt(IntValue obj)
        {
            // If needed, find the conversion operator.
            if (FromConversionOperator == null)
            {
                if (CastUtility.TryFindCast(typeof(IntValue), typeof(T), out MethodInfo method))
                    FromConversionOperator = method;
            }

            if (FromConversionOperator == null)
            {
                throw new InvalidCastException($"Objects of type '{typeof(IntValue).Name}' cannot be converted to "
                    + $"'{typeof(T).Name}'.");
            }

            // If we found the conversion operator, invoke it.
            return (T)FromConversionOperator.Invoke(null, new object[1] { obj });
        }
    }
}