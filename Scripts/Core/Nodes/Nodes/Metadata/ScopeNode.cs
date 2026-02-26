using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A scope serializer node.
    /// </summary>
    public class ScopeNode : IMetadataNode, IMemberNameNode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public string Name { get; set; }
        public SymbolNode Value { get; set; }
        INode IMetadataNode.Value => Value;

        /* Constructors. */
        public ScopeNode(string name, SymbolNode value)
        {
            Name = name;
            Value = value;

            if (Value != null)
                Value.Parent = this;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "scope: " + Name + "\n" + PrintUtility.PrintChild(Value);
        }

        public void Clear()
        {
            Parent = null;
            Name = "";
            Value.Clear();
            Value = null;
        }

        void IContainerNode.ReplaceChild(INode oldChild, INode newChild)
            => ReplaceChild(oldChild as SymbolNode, newChild as SymbolNode);
        public void ReplaceChild(SymbolNode oldChild, SymbolNode newChild)
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