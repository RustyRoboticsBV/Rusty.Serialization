using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An ID serializer node.
    /// </summary>
    public class IdNode : IMetadataNode
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

        /* Conversion operators. */
        public static explicit operator IdNode(ObjectNode node)
        {
            // Find the type and value nodes.
            StringNode id = null;
            INode value = null;
            for (int i = 0; i < node.Count; i++)
            {
                if (node.GetNameAt(i) == "id" && id == null)
                    id = (StringNode)node.GetValueAt(i);
                else if (node.GetNameAt(i) == "value" && value == null)
                    value = node.GetValueAt(i);
            }

            // Make sure the child nodes weren't null.
            if (id == null)
                throw new NullReferenceException(nameof(id));
            if (value == null)
                throw new NullReferenceException(nameof(value));

            // Create a new type node.
            return new IdNode(id.Value, value);
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

        public void ReplaceChild(INode oldChild, INode newChild)
        {
            if (Value == oldChild)
            {
                if (oldChild.Parent == this)
                    oldChild.Parent = null;
                newChild.Parent = this;
                Value = newChild;
                return;
            }
            throw new ArgumentException($"'{oldChild}' was not a child of '{this}'.");
        }
    }
}