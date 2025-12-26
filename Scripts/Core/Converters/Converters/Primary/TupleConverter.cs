using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
#if NET5_0_OR_GREATER
    [RequiresUnreferencedCode(
        "TupleConverter uses reflection over tuple fields. " +
        "Ensure tuple types are preserved when trimming is enabled.")]
#endif
    /// <summary>
    /// A tuple converter.
    /// </summary>
    public sealed class TupleConverter<
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
    T> : CompositeConverter<T, ListNode>
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
            for (int i = 0; i < node.Count && i < obj.Length; i++)
            {
                node.Elements[i] = context.CreateNode(obj[i]);
            }
        }

        protected override void CollectTypes(ListNode node, CollectTypesContext context)
        {
            FieldInfo[] fields = GetFields();
            for (int i = 0; i < node.Count && i < fields.Length; i++)
            {
                Type elementType = fields[i].FieldType;
                context.CollectTypes(node.Elements[i], elementType);
            }
        }

        protected override T CreateObject(ListNode node, CreateObjectContext context)
        {
            return (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
        }

#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ValueTuple<>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ValueTuple<,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ValueTuple<,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ValueTuple<,,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ValueTuple<,,,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ValueTuple<,,,,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ValueTuple<,,,,,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ValueTuple<,,,,,,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Tuple<>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Tuple<,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Tuple<,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Tuple<,,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Tuple<,,,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Tuple<,,,,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Tuple<,,,,,,>))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Tuple<,,,,,,,>))]
#endif
        protected override T AssignObject(T obj, ListNode node, AssignObjectContext context)
        {
            FieldInfo[] fields = GetFields();
            for (int i = 0; i < node.Count && i < fields.Length; i++)
            {
                object value = context.CreateChildObject(
                    fields[i].FieldType,
                    node.GetValueAt(i));

                WriteField(ref obj, fields[i], value);
            }
            return obj;
        }

        /* Private methods. */
        private FieldInfo[] GetFields()
        {
            if (Fields != null)
                return Fields;

            Fields = typeof(T)
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .OrderBy(f => f.Name)
                .ToArray();

            return Fields;
        }

        private static void WriteField(ref T obj, FieldInfo field, object value)
        {
            var tr = __makeref(obj);
            field.SetValueDirect(tr, value);
        }
    }
}