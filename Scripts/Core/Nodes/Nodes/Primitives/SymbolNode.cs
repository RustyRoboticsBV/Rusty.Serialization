namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A reference serializer node.
    /// </summary>
    public sealed class SymbolNode : LeafNode, IMemberNameNode
    {
        /* Public properties. */
        public string Name { get; set; }

        /* Constructors. */
        public SymbolNode() : this("") { }

        public SymbolNode(string name) : base()
        {
            Name = name.ToString() ?? "";
        }

        /* Public methods. */
        public override string ToString()
        {
            return "symbol: " + Name;
        }

        public override void Clear()
        {
            base.Clear();
            Name = "";
        }
    }
}