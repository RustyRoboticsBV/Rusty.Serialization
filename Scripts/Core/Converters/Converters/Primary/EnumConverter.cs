using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An enum converter.
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

            object value = underlyingType switch
            {
                Type t when t == typeof(byte) => byte.Parse(node.Value),
                Type t when t == typeof(sbyte) => sbyte.Parse(node.Value),
                Type t when t == typeof(short) => short.Parse(node.Value),
                Type t when t == typeof(ushort) => ushort.Parse(node.Value),
                Type t when t == typeof(int) => int.Parse(node.Value),
                Type t when t == typeof(uint) => uint.Parse(node.Value),
                Type t when t == typeof(long) => long.Parse(node.Value),
                Type t when t == typeof(ulong) => ulong.Parse(node.Value),
                _ => throw new NotSupportedException($"Unsupported enum underlying type {underlyingType}")
            };

            return (T)Enum.ToObject(enumType, value);
        }
    }
}