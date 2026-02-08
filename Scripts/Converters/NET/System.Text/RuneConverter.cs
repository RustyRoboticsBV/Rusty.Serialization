#if NET5_0_OR_GREATER
using System.Text;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET rune converter.
    /// </summary>
    public class RuneConverter : Converter<Rune, CharNode>
    {
        /* Protected method. */
        protected override CharNode CreateNode(Rune obj, CreateNodeContext context)
        {
            return new CharNode(obj.Value);
        }

        protected override Rune CreateObject(CharNode node, CreateObjectContext context)
        {
            return new Rune(node.Value.CodePoint);
        }
    }
}
#endif