using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A char converter.
    /// </summary>
    public sealed class CharConverter : CharConverter<char>
    {
        /* Protected methods. */
        protected override CharValue ToChar(char obj) => new CharValue(obj);

        protected override char FromChar(CharValue value) => (char)value;
    }
}