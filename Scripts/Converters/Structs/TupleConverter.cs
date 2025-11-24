using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A generic value tuple converter.
/// </summary>
public sealed class TupleConverter<TupleT> : ValueConverter<TupleT, ListNode>
    where TupleT : struct, ITuple
{
    /* Protected methods. */
    protected sealed override ListNode Convert(TupleT obj, Context context)
    {
        ITuple tuple = obj;
        INode[] elementNodes = new INode[tuple.Length];
        for (int i = 0; i < tuple.Length; i++)
        {
            elementNodes[i] = ConvertElement(tuple[i], context);
        }
        return new(elementNodes);
    }

    protected sealed override TupleT Deconvert(ListNode node, Context context)
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
            values[i] = DeconvertElement(node.Elements[i], context);

        // Invoke constructor.
        return (TupleT)ctor.Invoke(values);
    }

    /* Private methods. */
    private INode ConvertElement(object obj, Context context)
    {
        return context.GetConverter(obj.GetType()).Convert(obj, context);
    }

    private object DeconvertElement(INode node, Context context)
    {
        Type targetType = ((IConverter)this).TargetType;
        return context.GetConverter(targetType).Deconvert(node, context);
    }
}