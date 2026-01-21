using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An target type to IConverter instance registry.
    /// </summary>
    public class ConverterInstanceRegistry
    {
        /* Private properties. */
        private Dictionary<Type, IConverter> targetToConverter = new();

        /* Public methods. */
        /// <summary>
        /// Register a converter type for some target type.
        /// </summary>
        public void Add(Type targetType, IConverter converter)
        {
            if (targetType == null)
                targetType = typeof(object);
            targetToConverter[targetType] = converter;
        }

        /// <summary>
        /// Get a converter for some type if it exists. Returns null if it doesn't.
        /// </summary>
        public IConverter Get(Type targetType)
        {
            if (targetToConverter.TryGetValue(targetType, out IConverter converter))
                return converter;
            return null;
        }

        /// <summary>
        /// Check a converter for some type if it exists.
        /// </summary>
        public bool Has(Type targetType)
        {
            if (targetType == null)
                return false;
            return targetToConverter.ContainsKey(targetType);
        }
    }
}