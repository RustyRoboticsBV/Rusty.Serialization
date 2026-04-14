using System;
using System.Reflection;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for typed converters.
    /// </summary>
    public abstract class TypedConverter<ValueT, TargetT> : Converter
    {
        /* Private properties. */
        private MethodInfo ToConversionOperator { get; set; }
        private MethodInfo FromConversionOperator { get; set; }

        /* Protected methods. */
        /// <summary>
        /// Convert a target object to the internal value representation.
        /// </summary>
        protected ValueT ToValue(TargetT obj)
        {
            // If needed, find the conversion operator.
            if (ToConversionOperator == null)
            {
                if (CastUtility.TryFindCast(typeof(TargetT), typeof(ValueT), out MethodInfo method))
                    ToConversionOperator = method;
            }

            if (ToConversionOperator == null)
            {
                throw new InvalidCastException($"Objects of type '{typeof(TargetT).Name}' cannot be converted to "
                    + $"'{typeof(ValueT).Name}'.");
            }

            // If we found the conversion operator, invoke it.
            return (ValueT)ToConversionOperator.Invoke(null, new object[1] { obj });
        }

        /// <summary>
        /// Deconvert a value object to a target object.
        /// </summary>
        protected TargetT FromValue(ValueT obj)
        {
            // If needed, find the conversion operator.
            if (FromConversionOperator == null)
            {
                if (CastUtility.TryFindCast(typeof(ValueT), typeof(TargetT), out MethodInfo method))
                    FromConversionOperator = method;
            }

            if (FromConversionOperator == null)
            {
                throw new InvalidCastException($"Objects of type '{typeof(ValueT).Name}' cannot be converted to "
                    + $"'{typeof(TargetT).Name}'.");
            }

            // If we found the conversion operator, invoke it.
            return (TargetT)FromConversionOperator.Invoke(null, new object[1] { obj });
        }
    }
}