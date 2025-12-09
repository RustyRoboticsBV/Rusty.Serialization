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
        protected override ListNode CreateNode(TupleT obj, IConverterScheme scheme, SymbolTable table)
        {
            return new(obj.Length);
        }

        protected override void AssignNode(ref ListNode node, TupleT obj, IConverterScheme scheme, SymbolTable table)
        {
            // Get tuple data.
            Type type = obj.GetType();
            ITuple tuple = obj;

            // Convert tuple elements.
            for (int i = 0; i < tuple.Length; i++)
            {
                Type fieldType = type.GetFields()[i].FieldType;
                node.Elements[i] = ConvertNested(fieldType, tuple[i], scheme, table);
                node.Elements[i].Parent = node;
            }
        }

        protected override TupleT CreateObject(ListNode node, IConverterScheme scheme, ParsingTable table)
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
                values[i] = DeconvertNested(fieldType, node.Elements[i], scheme, table);
            }

            // Invoke constructor.
            return (TupleT)ctor.Invoke(values);
        }
    }
}