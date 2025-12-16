using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An enum type converter.
    /// </summary>
    public sealed class EnumConverter<T> : Converter<T, IntNode>
        where T : struct, Enum
    {
        /* Protected methods. */
        protected override IntNode CreateNode(T obj, CreateNodeContext context) => new(Convert.ToInt32(obj));

        protected override T CreateObject(IntNode node, CreateObjectContext context)
        {
            Type enumType = typeof(T);
            Type underlyingType = enumType.GetEnumUnderlyingType();
            object value = Convert.ChangeType(node.Value, underlyingType);
            return (T)Enum.ToObject(enumType, value);
        }
    }
}