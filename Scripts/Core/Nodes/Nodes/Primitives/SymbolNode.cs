namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A symbol serializer node.
    /// </summary>
    public class SymbolNode : ValueNode<string>
    {
        /* Constructors. */
        public SymbolNode() : base() { }

        public SymbolNode(string value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "symbol: " + (Name ?? "(null)");
        }
    }
}