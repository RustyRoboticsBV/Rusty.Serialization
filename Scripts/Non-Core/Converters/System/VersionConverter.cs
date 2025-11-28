using System;
using Rusty.Serialization.Core.Contexts;
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
        protected override StringNode Convert(Version obj, Context context) => new(obj.ToString());
        protected override Version Deconvert(StringNode node, Context context) => new(node.Value);
    }
}