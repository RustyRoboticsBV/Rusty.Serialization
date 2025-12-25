using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization
{
    /// <summary>
    /// The default conversion context, containing converters for the .NET, Unity and Godot types.
    /// </summary>
    public partial class DefaultConversionContext : ConversionContext
    {
        public DefaultConversionContext() : base()
        {
            AddSystem();
        }
    }
}