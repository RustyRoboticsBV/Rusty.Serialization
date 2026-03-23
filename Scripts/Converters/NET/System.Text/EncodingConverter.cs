using System.Text;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET encoding converter.
    /// </summary>
    public sealed class EncodingConverter : TypedStringConverter<Encoding>
    {
        /* Protected method. */
        protected override string ToString(Encoding obj) => obj.WebName;
        protected override Encoding FromString(string str) => Encoding.GetEncoding(str);
    }
}