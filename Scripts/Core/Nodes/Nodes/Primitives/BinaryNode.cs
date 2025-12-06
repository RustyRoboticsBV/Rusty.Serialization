namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A binary string serializer node.
    /// </summary>
    public class BinaryNode : INode
    {
        /* Public properties. */
        public INode Parent { get; set; }
        public byte[] Value { get; set; }

        /* Constructors. */
        public BinaryNode(byte[] value)
        {
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"binary: 0x{HexUtility.ToHexString(Value)}";
        }

        public void Clear()
        {
            Parent = null;
            Value = null;
        }
    }
}