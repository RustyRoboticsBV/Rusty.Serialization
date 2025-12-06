namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A type label serializer node.
    /// </summary>
    public class TypeNode : INode
    {
        /* Public properties. */
        public INode Parent { get; set; }
        public string Name { get; set; }
        public INode Value { get; set; }

        /* Constructors. */
        public TypeNode(string name, INode value)
        {
            Name = name;
            Value = value;

            if (Value != null)
                Value.Parent = this;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "type: " + Name + "\n" + PrintUtility.PrintChild(Value);
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