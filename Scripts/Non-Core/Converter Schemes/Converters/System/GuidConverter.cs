using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Guid converter.
    /// </summary>
    public sealed class GuidConverter : ValueConverter<Guid, BinaryNode>
    {
        /* Protected methods. */
        protected override BinaryNode ConvertValue(Guid obj, IConverterScheme scheme, NodeTree tree) => new(obj.ToByteArray());
        protected override Guid DeconvertValue(BinaryNode node, IConverterScheme scheme, NodeTree tree) => new(node.Value);
    }
}