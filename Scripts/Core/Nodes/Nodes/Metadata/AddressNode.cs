using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An address serializer node.
    /// </summary>
    public class AddressNode : IMetadataNode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public string Name { get; set; }
        public INode Value { get; set; }

        /* Constructors. */
        public AddressNode(string name, INode value)
        {
            Name = name ?? "";
            Value = value;

            if (Value != null)
                Value.Parent = this;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Address: " + Name + "\n" + PrintUtility.PrintChild(Value);
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