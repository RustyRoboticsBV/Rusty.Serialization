using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET type converter.
    /// </summary>
    public class TypeConverter : Core.Converters.Converter<Type, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode(Type obj, CreateNodeContext context)
        {
            return new StringNode(new TypeName(obj));
        }

        protected override Type CreateObject(StringNode node, CreateObjectContext context)
        {
            return new TypeName(node.Value);
        }
    }
}