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
        protected override StringNode ConvertRef(Type obj, IConverterScheme scheme) => new(new TypeName(obj));
        protected override Type DeconvertRef(StringNode node, IConverterScheme scheme) => new TypeName(node.Value).ToType();
    }
}