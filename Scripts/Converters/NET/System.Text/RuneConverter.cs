#if NET5_0_OR_GREATER
using System.Text;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET rune converter.
    /// </summary>
    public sealed class RuneConverter : TypedCharConverter<Rune>
    {
        /* Protected method. */
        protected override UnicodePair ToChar(Rune obj) => new UnicodePair(obj.Value);
        protected override Rune FromChar(UnicodePair value) => (Rune)(int)value;
    }
}
#endif