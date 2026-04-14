using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An object target type to converter instance cache.
    /// </summary>
    internal class ConverterInstanceCache
    {
        /* Private properties. */
        private Dictionary<Type, Converter> targetToConverter = new Dictionary<Type, Converter>();

        /* Public methods. */
        public override string ToString()
        {
            string str = "";
            foreach (var pair in targetToConverter)
            {
                if (str != "")
                    str += "\n";
                str += pair.Key.Name.ToString() + " = " + pair.Value.GetType().Name.ToString();
            }
            return str;
        }

        /// <summary>
        /// Register a converter type for some target type.
        /// </summary>
        public void Add(Type targetType, Converter converter)
        {
            if (targetType == null)
                targetType = typeof(object);
            targetToConverter[targetType] = converter;
        }

        /// <summary>
        /// Get a converter for some type if it exists. Returns null if it doesn't.
        /// </summary>
        public Converter Get(Type targetType)
        {
            if (targetToConverter.TryGetValue(targetType, out Converter converter))
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