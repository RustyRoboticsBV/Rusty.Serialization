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
            FieldInfo[] fields = GetFields();
            System.Console.WriteLine(fields.Length + " " + node.Count);
            for (int i = 0; i < node.Count; i++)
            {
                Type elementType = fields[i].FieldType;
                context.CollectTypes(node.Elements[i], elementType);
            }
        }

        protected override T CreateObject(ListNode node, CreateObjectContext context)
            => (T)RuntimeHelpers.GetUninitializedObject(typeof(T));

        protected override T AssignObject(T obj, ListNode node, AssignObjectContext context)
        {
            FieldInfo[] fields = GetFields();
            for (int i = 0; i < obj.Length; i++)
            {
                Type type = fields[i].FieldType;
                object element = context.CreateChildObject(type, node.Elements[i]);
                SetTupleField(obj, fields[i], element);
            }
            return obj;
        }

        /* Private methods. */
        private FieldInfo[] GetFields()
        {
            if (Fields == null)
            {
                Fields = typeof(T)
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .OrderBy(f => f.Name)
                    .ToArray();
            }
            return Fields;
        }

        static void SetTupleField(ITuple tuple, FieldInfo field, object value)
        {
            TypedReference tr = __makeref(tuple);
            field.SetValueDirect(tr, value);
        }
    }
}