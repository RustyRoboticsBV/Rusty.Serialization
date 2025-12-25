using System;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A converter registry.
    /// </summary>
    public class ConverterRegistry
    {
        /* Private properties. */
        private ConverterTypeRegistry Types { get; } = new();
        private ConverterInstanceRegistry Instances { get; } = new();

        /* Public methods. */
        /// <summary>
        /// Register a converter type for some target type.
        /// </summary>
        public void Add<TargetT, ConverterT>()
            where ConverterT : IConverter
        {
            Add(typeof(TargetT), typeof(ConverterT));
        }

        /// <summary>
        /// Register a converter type for some target type.
        /// </summary>
        public void Add(Type targetType, Type converterType)
        {
            Types.Add(targetType, converterType);
        }

        /// <summary>
        /// Get an instance of a converter for some type.
        /// </summary>
        public IConverter Get(Type targetType)
        {
            // Try to get the instance.
            IConverter converterInstance = Instances.Get(targetType);
            if (converterInstance != null)
                return converterInstance;

            // Else, try to instantiate.
            try
            {
                converterInstance = Types.Instantiate(targetType);
                Instances.Add(targetType, converterInstance);
                return converterInstance;
            }

            // Else, throw exception.
            catch
            {
                throw new ArgumentException($"No converter type was registered for target type '{targetType}'.");
            }
        }
    }
}