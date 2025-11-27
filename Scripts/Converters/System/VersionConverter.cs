using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
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