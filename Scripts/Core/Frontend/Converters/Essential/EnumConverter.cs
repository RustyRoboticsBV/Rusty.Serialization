using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An enum converter.
    /// </summary>
    public class EnumConverter<T> : Converter
        where T : Enum
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => CreateNode(TryCast<T>(obj));

        /* Protected methods. */
        protected static SymbolNode CreateNode(T obj) => new SymbolNode(obj.ToString());
    }
}