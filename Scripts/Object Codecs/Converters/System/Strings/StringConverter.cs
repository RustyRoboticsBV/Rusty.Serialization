using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A string converter.
    /// </summary>
    public sealed class StringConverter : StringConverter<string>
    {
        /* Protected methods. */
        protected override StringValue ToString(string obj) => new StringValue(obj);

        protected override string FromString(StringValue value) => (string)value;
    }
}