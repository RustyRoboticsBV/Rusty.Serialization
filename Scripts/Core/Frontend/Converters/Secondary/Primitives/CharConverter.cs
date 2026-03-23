using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A char converter.
    /// </summary>
    public sealed class CharConverter : TypedCharConverter<char>
    {
        /* Protected methods. */
        protected override UnicodePair ToChar(char obj) => obj;
        protected override char FromChar(UnicodePair value) => (char)value;
    }
}