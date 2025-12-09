using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Type converter.
    /// </summary>
    public sealed class TypeConverter : ReferenceConverter<Type, StringNode>
    {
        /* Protected methods. */
        protected override StringNode CreateNode(Type obj, IConverterScheme scheme, SymbolTable table) => new(obj.ToString());
        protected override Type CreateObject(StringNode node, IConverterScheme scheme, ParsingTable table) => new TypeName(node.Value).ToType();
    }
}