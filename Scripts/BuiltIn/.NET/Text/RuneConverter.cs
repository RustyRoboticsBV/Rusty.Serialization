#if NETCOREAPP3_0_OR_GREATER
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Dotnet
{
    /// <summary>
    /// A date/time converter.
    /// </summary>
    public sealed class RuneConverter : Converter<Rune, CharNode>
    {
        /* Protected methods. */
        protected override CharNode CreateNode(Rune obj, CreateNodeContext context) => new(obj.ToString());
        protected override Rune CreateObject(CharNode node, CreateObjectContext context) => Rune.GetRuneAt(node.Value, 0);
    }
}
#endif