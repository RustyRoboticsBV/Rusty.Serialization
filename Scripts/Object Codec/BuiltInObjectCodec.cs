using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion
{
    /// <summary>
    /// The built-in object conversion codec.
    /// </summary>
    public partial class BuiltInObjectCodec : ObjectCodec
    {
        /* Constructors. */
        public BuiltInObjectCodec() : base()
        {
            AddSystemTypes();
        }
    }
}
