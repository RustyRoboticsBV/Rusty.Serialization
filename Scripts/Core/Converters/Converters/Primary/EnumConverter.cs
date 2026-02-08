using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An enum converter.
    /// </summary>
    public sealed class EnumConverter<T> : Converter<T, SymbolNode>
        where T : struct, Enum
    {
        /* Protected methods. */
        protected override SymbolNode CreateNode(T obj, CreateNodeContext context) => new SymbolNode(obj.ToString());

        protected override T CreateObject(SymbolNode node, CreateObjectContext context)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), node.Value, false);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Could not convert value '{node.Value}' to enum type '{typeof(T).FullName}'.", ex);
            }
        }
    }
}