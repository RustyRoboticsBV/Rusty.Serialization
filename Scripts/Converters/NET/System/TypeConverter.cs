using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET type converter.
    /// </summary>
    public class TypeConverter : Core.Conversion.TypedConverter<Type, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode2(Type obj, CreateNodeContext context)
        {
            return new StringNode(new TypeName(obj));
        }

        protected override Type CreateObject2(StringNode node, CreateObjectContext context)
        {
            return new TypeName(node.Value);
        }
    }
}