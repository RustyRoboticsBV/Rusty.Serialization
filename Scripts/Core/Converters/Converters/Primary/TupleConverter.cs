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
    public sealed class TupleConverter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T> : CompositeConverter<T, ListNode>
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
            for (int i = 0; i < fields.Length; i++)
            {
                System.Console.WriteLine(i + ": " + fields[i].FieldType + " " +  fields[i].Name);
            }
            for (int i = 0; i < node.Count && i < fields.Length; i++)
            {
                Type elementType = fields[i].FieldType;
                System.Console.WriteLine(elementType + " " + node.Elements[i]);
                context.CollectTypes(node.Elements[i], elementType);
            }
        }

        protected override T CreateObject(ListNode node, CreateObjectContext context)
        {
            return (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
        }

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
        protected override T AssignObject(T obj, ListNode node, AssignObjectContext context)
        {
            FieldInfo[] fields = GetFields();
            int count = Math.Min(fields.Length, node.Count);

            //ref byte raw = ref Unsafe.As<T, byte>(ref obj);

            for (int i = 0; i < count; i++)
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