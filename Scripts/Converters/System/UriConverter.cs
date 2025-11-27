using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
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