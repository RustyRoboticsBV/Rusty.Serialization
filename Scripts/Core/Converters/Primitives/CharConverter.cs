using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A char converter.
    /// </summary>
    public sealed class CharConverter : ValueConverter<char, CharNode>
    {
        /* Protected methods. */
        protected override CharNode Convert(char obj, Context context) => new(obj);
        protected override char Deconvert(CharNode node, Context context) => node.Value;
    }
}