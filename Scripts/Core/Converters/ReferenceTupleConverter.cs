using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic reference tuple converter.
    /// </summary>
    public sealed class ReferenceTupleConverter<TupleT> : ReferenceConverter<TupleT, ListNode>
        where TupleT : class, ITuple
    {
        /* Protected methods. */
        protected sealed override ListNode ConvertRef(TupleT obj, IConverterScheme scheme)
        {
            Type type = obj.GetType();
            ITuple tuple = obj;
            INode[] elementNodes = new INode[tuple.Length];
            for (int i = 0; i < tuple.Length; i++)
            {
                Type fieldType = type.GetFields()[i].FieldType;
                elementNodes[i] = ConvertNested(fieldType, tuple[i], scheme);
            }
            return new(elementNodes);
        }

        protected sealed override TupleT DeconvertRef(ListNode node, IConverterScheme scheme)
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