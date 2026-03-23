using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A type serializer node.
    /// </summary>
    public class TypeNode : IMetadataNode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public string Name { get; set; }
        public INode Child { get; set; }

        /* Constructors. */
        public TypeNode(string name, INode value)
        {
            Name = name;
            Child = value;

            if (Child != null)
                Child.Parent = this;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "type: " + Name + "\n" + PrintUtility.PrintChild(Child);
        }

        public void Clear()
        {
            Parent = null;
            Name = "";
            Child?.Clear();
            Child = null;
        }

        public void ReplaceChild(INode oldChild, INode newChild)
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