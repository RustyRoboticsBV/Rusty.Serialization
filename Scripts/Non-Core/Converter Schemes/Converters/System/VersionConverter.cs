using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Version converter.
    /// </summary>
    public sealed class VersionConverter : ReferenceConverter<Version, StringNode>
    {
        /* Protected methods. */
        protected override StringNode CreateNode(Version obj, IConverterScheme scheme, SymbolTable table) => new(obj.ToString());
        protected override Version DeconvertRef(StringNode node, IConverterScheme scheme, NodeTree tree) => new(node.Value);
    }
}