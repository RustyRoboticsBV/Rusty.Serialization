namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A reference serializer node.
    /// </summary>
    public class RefNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public string Address { get; set; }

        /* Constructors. */
        public RefNode(string address)
        {
            Address = address.ToString() ?? "";
        }

        /* Public methods. */
        public override string ToString()
        {
            return "ref: " + Address;
        }

        public void Clear()
        {
            Parent = null;
            Address = "";
        }
    }
}