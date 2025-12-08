using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Uri converter.
    /// </summary>
    public sealed class UriConverter : ReferenceConverter<Uri, StringNode>
    {
        /* Protected methods. */
        protected override StringNode CreateNode(Uri obj, IConverterScheme scheme, SymbolTable table) => new(obj.ToString());
        protected override Uri DeconvertRef(StringNode node, IConverterScheme scheme, NodeTree tree) => new(node.Value);
    }
}