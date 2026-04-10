using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// The built-in object conversion codec.
    /// </summary>
    public class BuiltInObjectCodec : ObjectCodec
    {
        /* Constructors. */
        public BuiltInObjectCodec() : base()
        {
            AddCoreTypes();
        }

        /* Private methods. */
        private void AddCoreTypes()
        {
            Converters.Add<int, IntConverter>();
        }
    }
}
