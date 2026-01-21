using System.Runtime.CompilerServices;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for vector converters.
    /// </summary>
    public abstract class VectorConverter<VectorT, ElementT> : CompositeConverter<VectorT, ListNode>
    {
        /* Protected methods. */
        /// <summary>
        /// Get the length of the vector.
        /// </summary>
        protected abstract int GetLength();

        /// <summary>
        /// Set the element at some index.
        /// </summary>
        protected abstract void SetElementAt(ref VectorT vector, int index, ref ElementT element);

        /// <summary>
        /// Get the element at some index.
        /// </summary>
        protected abstract ElementT GetElementAt(ref VectorT vector, int index);

        protected sealed override ListNode CreateNode(VectorT obj, CreateNodeContext context)
        {
            return new ListNode(GetLength());
        }

        protected sealed override void AssignNode(ListNode node, VectorT obj, AssignNodeContext context)
        {
            for (int i = 0; i < node.Count && i < GetLength(); i++)
            {
                node.Elements[i] = context.CreateNode(GetElementAt(ref obj, i));
            }
        }

        protected sealed override void CollectTypes(ListNode node, CollectTypesContext context)
        {
            for (int i = 0; i < node.Count && i < GetLength(); i++)
            {
                context.CollectTypes(node.Elements[i], typeof(ElementT));
            }
        }

        protected sealed override VectorT CreateObject(ListNode node, CreateObjectContext context)
        {
            return (VectorT)RuntimeHelpers.GetUninitializedObject(typeof(VectorT));
        }

        protected sealed override VectorT AssignObject(VectorT obj, ListNode node, AssignObjectContext context)
        {
            for (int i = 0; i < node.Count && i < GetLength(); i++)
            {
                ElementT element = context.CreateChildObject<ElementT>(node.Elements[i]);
                SetElementAt(ref obj, i, ref element);
            }
            return obj;
        }
    }
}