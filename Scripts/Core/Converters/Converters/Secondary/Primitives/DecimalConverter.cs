using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A decimal converter.
    /// </summary>
    public sealed class DecimalConverter : Converter<decimal, DecimalNode>
    {
        /* Protected methods. */
        protected override DecimalNode CreateNode(decimal obj, CreateNodeContext context)
        {
            UnityEngine.Debug.Log("input: " + obj);
            return new DecimalNode(obj);
        }
        protected override decimal CreateObject(DecimalNode node, CreateObjectContext context)
        {
            decimal d = (decimal)node.Value;
            UnityEngine.Debug.Log("Decimal: " + d);
            return d;
        }
    }
}