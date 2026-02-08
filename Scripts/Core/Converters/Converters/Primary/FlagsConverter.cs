using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An enum converter.
    /// </summary>
    public sealed class FlagsConverter<T> : Converter<T, ListNode>
        where T : struct, Enum
    {
        /* Protected methods. */
        protected override ListNode CreateNode(T obj, CreateNodeContext context)
        {
            ListNode list = new ListNode();

            long value = Convert.ToInt64(obj);
            Array enumValues = Enum.GetValues(typeof(T));

            // "Everything".
            if (value == -1)
            {
                for (int i = 0; i < enumValues.Length; i++)
                {
                    T enumValue = (T)enumValues.GetValue(i);
                    long enumNumeric = Convert.ToInt64(enumValue);

                    if (enumNumeric > 0 && IsPowerOfTwo(enumNumeric))
                    {
                        list.AddValue(new SymbolNode(enumValue.ToString()));
                    }
                }
                return list;
            }

            // Zero.
            if (value == 0)
            {
                for (int i = 0; i < enumValues.Length; i++)
                {
                    T enumValue = (T)enumValues.GetValue(i);
                    if (Convert.ToInt64(enumValue) == 0)
                    {
                        list.AddValue(new SymbolNode(enumValue.ToString()));
                        break;
                    }
                }
                return list;
            }

            // Normal flags.
            for (int i = 0; i < enumValues.Length; i++)
            {
                T enumValue = (T)enumValues.GetValue(i);
                long enumNumeric = Convert.ToInt64(enumValue);

                if (enumNumeric > 0 && (value & enumNumeric) == enumNumeric)
                {
                    list.AddValue(new SymbolNode(enumValue.ToString()));
                }
            }

            return list;
        }


        protected override T CreateObject(ListNode node, CreateObjectContext context)
        {
            ulong combinedValue = 0;

            for (int i = 0; i < node.Count; i++)
            {
                INode item = node.GetValueAt(i);
                if (item is SymbolNode s)
                {
                    try
                    {
                        T enumValue = (T)Enum.Parse(typeof(T), s.Value, false);
                        combinedValue |= Convert.ToUInt64(enumValue);
                    }
                    catch (ArgumentException)
                    {
                        throw new InvalidOperationException(
                            $"Could not convert value '{s.Value}' to enum type '{typeof(T).FullName}'.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("All list entries must be SymbolNodes.");
                }
            }

            return (T)Enum.ToObject(typeof(T), combinedValue);
        }

        /* Private methods. */
        private static bool IsPowerOfTwo(long value)
        {
            return (value & (value - 1)) == 0;
        }
    }
}