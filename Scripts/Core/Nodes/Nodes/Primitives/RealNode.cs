using PeterO.Numbers;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A real number serializer node.
    /// </summary>
    public class RealNode : INode
    {
        /* Public properties. */
        public EDecimal Value { get; set; }

        /* Constructors. */
        public RealNode(EDecimal value)
        {
            Value = value ?? 0;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "real: " + (Value?.ToString() ?? "(null)");
        }

        public void Clear()
        {
            Value = 0;
        }

        public void ClearRecursive() => Clear();
    }
}