using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A generic array converter.
/// </summary>
public class ArrayConverter<T> : IListConverter<T[], T>
{
    /* Protected methods. */
    protected sealed override T[] Instantiate() => [];

    protected sealed override ListNode Convert(T[] obj, Context context)
    {
        INode[] elementNodes = new INode[obj.Length];
        for (int i = 0; i < obj.Length; i++)
        {
            elementNodes[i] = ConvertElement(obj[i], context);
        }
        return new(elementNodes);
    }

    protected sealed override T[] Deconvert(ListNode node, Context context)
    {
        T[] array = new T[node.Elements.Length];
        for (int i = 0; i < node.Elements.Length; i++)
        {
            array[i] = DeconvertElement(node.Elements[i], context);
        }
        return array;
    }
}