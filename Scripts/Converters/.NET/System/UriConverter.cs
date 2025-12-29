using System;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET URI converter.
    /// </summary>
    public class UriConverter : Core.Converters.Converter<Uri, StringNode>
    {
        /* Protected method. */
        protected override StringNode CreateNode(Uri obj, CreateNodeContext context)
        {
            return new StringNode(obj.ToString());
        }

        protected override Uri CreateObject(StringNode node, CreateObjectContext context)
        {
            return new Uri(node.Value);
        }
    }
}