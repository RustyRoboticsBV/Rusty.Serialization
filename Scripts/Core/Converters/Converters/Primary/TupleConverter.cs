using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A tuple converter.
    /// </summary>
    public sealed class TupleConverter<T> : CompositeConverter<T, ListNode>
        where T : struct, ITuple
    {
        /* Private properties. */
        FieldInfo[] Fields { get; set; } = null;

        /* Protected methods. */
        protected override ListNode CreateNode(T obj, CreateNodeContext context)
        {
            return new(obj.Length);
        }

        protected override void AssignNode(ListNode node, T obj, AssignNodeContext context)
        {
            for (int i = 0; i < node.Count; i++)
            {
                node.Elements[i] = context.CreateNode(obj[i]);
            }
        }

        protected override void CollectTypes(ListNode node, CollectTypesContext context)
        {
            FieldInfo[] fields = GetFields();
            for (int i = 0; i < node.Count; i++)
            {
                Type elementType = fields[i].FieldType;
                context.CollectTypes(node.Elements[i], elementType);
            }
        }

        protected override T CreateObject(ListNode node, CreateObjectContext context) => new();

        protected override T AssignObject(T obj, ListNode node, AssignObjectContext context)
        {
            object boxed = obj;
            FieldInfo[] fields = GetFields();
            for (int i = 0; i < obj.Length && i < node.Count; i++)
            {
                object element = context.CreateChildObject(fields[i].FieldType, node.GetValueAt(i));
                fields[i].SetValue(boxed, element);
            }
            return (T)boxed;
        }

        /* Private methods. */
        private FieldInfo[] GetFields()
        {
            if (Fields == null)
            {
                Fields = typeof(T)
                    .GetFields()
                    .OrderBy(f => f.Name)
                    .ToArray();
            }
            return Fields;
        }
    }
}