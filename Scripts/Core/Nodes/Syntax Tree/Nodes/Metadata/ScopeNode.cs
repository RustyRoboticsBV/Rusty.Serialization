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
        public SymbolNode Child { get; set; }
        INode IMetadataNode.Child => Child;

        /* Constructors. */
        public ScopeNode(string name, SymbolNode value)
        {
            Name = name;
            Child = value;

            if (Child != null)
                Child.Parent = this;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "scope: " + Name + "\n" + PrintUtility.PrintChild(Child);
        }

        public void Clear()
        {
            Parent = null;
            Name = "";
            Child.Clear();
            Child = null;
        }

        void IContainerNode.ReplaceChild(INode oldChild, INode newChild)
            => ReplaceChild(oldChild as SymbolNode, newChild as SymbolNode);
        public void ReplaceChild(SymbolNode oldChild, SymbolNode newChild)
        {
            if (Child == oldChild)
            {
                if (oldChild.Parent == this)
                    oldChild.Parent = null;
                newChild.Parent = this;
                Child = newChild;
                return;
            }
            throw new ArgumentException($"'{oldChild}' was not a child of '{this}'.");
        }
    }
}