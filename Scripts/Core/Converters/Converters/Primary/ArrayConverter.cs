using Rusty.Serialization.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An array converter.
    /// </summary>
    public sealed class ArrayConverter<T> : ListConverter<T[], T>
    {
        /* Protected methods. */
        protected override T[] CreateObject(ListNode node, CreateObjectContext context) => new T[node.Count];

        protected override T[] AssignObject(T[] obj, ListNode node, AssignObjectContext context)
        {
            Type elementType = typeof(T);
            for (int i = 0; i < node.Count; i++)
            {
                obj[i] = context.CreateChildObject<T>(node.GetValueAt(i));
            }
            return obj;
        }
    }
}