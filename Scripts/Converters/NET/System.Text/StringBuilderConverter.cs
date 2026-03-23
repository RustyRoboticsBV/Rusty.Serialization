using System.Text;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET string builder converter.
    /// </summary>
    public sealed class StringBuilderConverter : TypedStringConverter<StringBuilder>
    {
        /* Protected method. */
        protected override StringBuilder FromString(string str) => new StringBuilder(str);
    }
}