namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A reference serializer node.
    /// </summary>
    public sealed class RefNode : LeafNode
    {
        /* Public properties. */
        public string Address { get; set; }

        /* Constructors. */
        public RefNode() : this("") { }

        public RefNode(string address) : base()
        {
            Address = address.ToString() ?? "";
        }

        /* Public methods. */
        public override string ToString()
        {
            return "ref: " + Address;
        }

        public override void Clear()
        {
            base.Clear();
            Address = "";
        }
    }
}