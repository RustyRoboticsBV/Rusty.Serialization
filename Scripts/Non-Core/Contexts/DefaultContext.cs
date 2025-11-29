#if GODOT && !UNITY_5_OR_NEWER
#define GODOT_CONTEXT

#elif !GODOT && UNITY_5_OR_NEWER
#define UNITY_CONTEXT
#endif
using Rusty.Serialization.Converters;
using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization
{
    /// <summary>
    /// A default serialization context. By default, it contains handlers for various .NET and Godot/Unity types, and serializes
    /// to the CSCD format.
    /// </summary>
    public class DefaultContext : Context
    {
        /* Public properties. */
        /// <summary>
        /// A set of default converters.
        /// </summary>
        public static DefaultConverters DefaultConverters => new();
        /// <summary>
        /// A CSCD serialization scheme.
        /// </summary>
        public static Serializers.CSCD.Scheme CscdScheme => new();

        public override IConverterScheme ConverterScheme { get; set; } = DefaultConverters;
        public override ISerializerScheme SerializerScheme { get; set; } = CscdScheme;
    }
}