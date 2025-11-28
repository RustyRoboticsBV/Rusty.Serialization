using System;
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
        protected override IntNode ConvertValue(T obj, IConverterScheme scheme) => new(System.Convert.ToInt32(obj));

        protected override T DeconvertValue(IntNode node, IConverterScheme scheme)
        {
            Type enumType = typeof(T);
            Type underlyingType = enumType.GetEnumUnderlyingType();
            object value = System.Convert.ChangeType(node.Value, underlyingType);
            return (T)Enum.ToObject(enumType, value);
        }
    }
}