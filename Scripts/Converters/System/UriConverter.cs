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
        protected override StringNode Convert(Uri obj, Context context) => new(obj.ToString());
        protected override Uri Deconvert(StringNode node, Context context) => new(node.Value);
    }
}