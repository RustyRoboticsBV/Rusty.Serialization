using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic value tuple converter.
    /// </summary>
    public sealed class ValueTupleConverter<TupleT> : ValueConverter<TupleT, ListNode>
        where TupleT : struct, ITuple
    {
        /* Protected methods. */
        protected sealed override ListNode ConvertValue(TupleT obj, IConverterScheme scheme)
        {
            // Get tuple data.
            Type type = obj.GetType();
            ITuple tuple = obj;

            // Create node.
            ListNode node = new(tuple.Length);

            // Convert tuple elements.
            for (int i = 0; i < tuple.Length; i++)
            {
                Type fieldType = type.GetFields()[i].FieldType;
                node.Elements[i] = ConvertNested(fieldType, tuple[i], scheme);
                node.Elements[i].Parent = node;
            }

            return node;
        }

        protected sealed override TupleT DeconvertValue(ListNode node, IConverterScheme scheme)
        {
            // Get constructor.
            Type type = typeof(TupleT);
            ConstructorInfo ctor = type.GetConstructors().FirstOrDefault(
                c => c.GetParameters().Length == node.Elements.Length);

            if (ctor == null)
                throw new InvalidOperationException($"Tuple type '{type}' has no constructor with {node.Elements.Length} parameters.");

            // Deconvert node.
            object[] values = new object[node.Elements.Length];
            for (int i = 0; i < values.Length; i++)
            {
                Type fieldType = type.GetFields()[i].FieldType;
                values[i] = DeconvertNested(fieldType, node.Elements[i], scheme);
            }

            // Invoke constructor.
            return (TupleT)ctor.Invoke(values);
        }
    }
}