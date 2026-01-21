using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization
{
    /// <summary>
    /// The set of default converters, containing converters for common .NET, Unity and Godot types.
    /// </summary>
    public partial class DefaultConverters : Converters
    {
        public DefaultConverters() : base()
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