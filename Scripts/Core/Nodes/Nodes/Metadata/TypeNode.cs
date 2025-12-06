namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A type label serializer node.
    /// </summary>
    public class TypeNode : INode
    {
        /* Public properties. */
        public string Name { get; set; }
        public INode Value { get; set; }

        /* Constructors. */
        public TypeNode(string name, INode value)
        {
            Name = name;
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "type: " + Name + "\n" + PrintUtility.PrintChild(Value);
        }

        public void Clear()
        {
            Name = "";
            Value = null;
        }

        public void ClearRecursive()
        {
            Value?.ClearRecursive();
            Clear();
        }

        /// <summary>
        /// Wrap the value node inside of an ID node.
        /// </summary>
        public void WrapId(ulong id)
        {
            IdNode node = new(id, Value);
            Value = node;
        }
    }
}