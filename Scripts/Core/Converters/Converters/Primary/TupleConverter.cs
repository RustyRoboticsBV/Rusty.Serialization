using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A tuple converter.
    /// </summary>
    public sealed class TupleConverter<T> : CompositeConverter<T, ListNode>
        where T : ITuple
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
            for (int i = 0; i < node.Elements.Length; i++)
            {
                Type elementType = GetFields()[i].FieldType;
                context.CollectTypes(node.Elements[i], elementType);
            }
        }

        protected override T CreateObject(ListNode node, CreateObjectContext context)
        {
            object[] values = new object[node.Elements.Length];
            for (int i = 0; i < values.Length; i++)
            {
                Type type = GetFields()[i].FieldType;
                values[i] = context.CreateObject(type, node.Elements[i]);
            }
            return CreateTuple(values);
        }

        protected override T FixReferences(T obj, ListNode node, FixReferencesContext context)
        {
            object[] values = new object[node.Elements.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = context.FixReferences(obj[i], node.Elements[i]);
            }
            return CreateTuple(values);
        }

        /* Private methods. */
        private FieldInfo[] GetFields()
        {
            if (Fields == null)
                Fields = typeof(T).GetFields();
            return Fields;
        }

        /// <summary>
        /// Create a tuple object.
        /// </summary>
        private static T CreateTuple(object[] values)
        {
            return (T)Activator.CreateInstance(typeof(T), values)!;
        }
    }
}