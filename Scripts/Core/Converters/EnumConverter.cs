using System;
using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic enum converter.
    /// </summary>
    public sealed class EnumConverter<T> : ValueConverter<T, IntNode>
        where T : struct, Enum
    {
        /* Protected methods. */
        protected override IntNode Convert(T obj, Context context) => new(System.Convert.ToInt32(obj));

        protected override T Deconvert(IntNode node, Context context)
        {
            if (Enum.IsDefined(typeof(T), node.Value))
                return (T)Enum.ToObject(typeof(T), node.Value);
            throw new Exception($"Cannot convert '{node.Value}' to enum type '{typeof(T)}'.");
        }
    }
}