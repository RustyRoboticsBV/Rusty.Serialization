namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A binary string serializer node.
    /// </summary>
    public class BinaryNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public byte[] Value { get; set; }

        /* Constructors. */
        public BinaryNode(byte[] value)
        {
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"binary: b{HexUtility.ToHexString(Value)}";
        }

        public void Clear()
        {
            Parent = null;
            Value = null;
        }
    }
}