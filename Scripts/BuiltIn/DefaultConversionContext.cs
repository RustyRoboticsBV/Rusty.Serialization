using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization
{
    /// <summary>
    /// The default conversion context, with built-in support for various .Net, Unity and Godot types.
    /// </summary>
    public partial class DefaultConversionContext : ConversionContext
    {
        /* Constructors. */
        public DefaultConversionContext() : base()
        {
            AddDotnetConverters();
        }
    }
}