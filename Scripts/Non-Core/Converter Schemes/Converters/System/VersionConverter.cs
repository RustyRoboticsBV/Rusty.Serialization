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
        protected override StringNode ConvertRef(Version obj, IConverterScheme scheme) => new(obj.ToString());
        protected override Version DeconvertRef(StringNode node, IConverterScheme scheme) => new(node.Value);
    }
}