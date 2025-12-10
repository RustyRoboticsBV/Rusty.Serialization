namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An ID serializer node.
    /// </summary>
    public class IdNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public string Name { get; set; }
        public INode Value { get; set; }

        /* Constructors. */
        public IdNode(string name, INode value)
        {
            Name = name ?? "";
            Value = value;

            if (Value != null)
                Value.Parent = this;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "ID: " + (Name ?? "(null)") + "\n" + PrintUtility.PrintChild(Value);
        }

        public void Clear()
        {
            Parent = null;
            Name = "";
            Value.Clear();
            Value = null;
        }
    }
}