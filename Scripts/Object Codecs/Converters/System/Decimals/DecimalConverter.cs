using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A decimal converter.
    /// </summary>
    public sealed class DecimalConverter : DecimalConverter<decimal>
    {
        /* Protected methods. */
        protected override DecimalValue ToDecimal(decimal obj) => obj;
        protected override decimal FromDecimal(DecimalValue obj) => (decimal)obj;
    }
}