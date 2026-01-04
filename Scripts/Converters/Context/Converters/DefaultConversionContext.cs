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
#if GODOT
            AddGodot();
#endif
#if UNITY_5_3_OR_NEWER
            AddUnity();
#endif
        }
    }
}