namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A reference serializer node.
    /// </summary>
    public class SymbolNode : IMemberNameNode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public string Name { get; set; }

        /* Constructors. */
        public SymbolNode() : this("") { }

        public SymbolNode(string name)
        {
            Name = name.ToString() ?? "";
        }

        /* Public methods. */
        public override string ToString()
        {
            return "symbol: " + Name;
        }

        public void Clear()
        {
            Parent = null;
            Name = "";
        }
    }
}