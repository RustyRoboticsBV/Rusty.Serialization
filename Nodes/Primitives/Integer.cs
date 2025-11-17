using System;
using System.Globalization;

namespace Rusty.Serialization.Nodes;

/// <summary>
/// An integer serializer node.
/// </summary>
public struct Integer : INode
{
    /* Fields. */
    private readonly decimal value;

    /* Constructors. */
    private Integer(decimal value)
    {
        this.value = value;
    }

    public Integer(sbyte value) : this((decimal)value) { }
    public Integer(byte value) : this((decimal)value) { }
    public Integer(short value) : this((decimal)value) { }
    public Integer(ushort value) : this((decimal)value) { }
    public Integer(int value) : this((decimal)value) { }
    public Integer(uint value) : this((decimal)value) { }
    public Integer(long value) : this((decimal)value) { }
    public Integer(ulong value) : this((decimal)value) { }

    /* Conversion operators. */
    public static implicit operator Integer(sbyte value) => new(value);
    public static implicit operator Integer(byte value) => new(value);
    public static implicit operator Integer(short value) => new(value);
    public static implicit operator Integer(ushort value) => new(value);
    public static implicit operator Integer(int value) => new(value);
    public static implicit operator Integer(uint value) => new(value);
    public static implicit operator Integer(long value) => new(value);
    public static implicit operator Integer(ulong value) => new(value);

    public static implicit operator sbyte(Integer node) => (sbyte)node.value;
    public static implicit operator byte(Integer node) => (byte)node.value;
    public static implicit operator short(Integer node) => (short)node.value;
    public static implicit operator ushort(Integer node) => (ushort)node.value;
    public static implicit operator int(Integer node) => (int)node.value;
    public static implicit operator uint(Integer node) => (uint)node.value;
    public static implicit operator long(Integer node) => (long)node.value;
    public static implicit operator ulong(Integer node) => (ulong)node.value;

    /* Public methods. */
    public override readonly string ToString()
    {
        return "int: " + value;
    }

    public readonly string Serialize()
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static Integer Deserialize(string text)
    {
        try
        {
            decimal value = decimal.Parse(text, CultureInfo.InvariantCulture);
            if (value < 0)
                return new((long)value);
            else
                return new((ulong)value);
        }
        catch
        {
            throw new ArgumentException($"Could not parse string '{text}' as an integer.");
        }
    }
}