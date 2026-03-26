using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An enum flags converter.
    /// </summary>
    public class FlagsConverter<T> : Converter
        where T : Enum
    {
        /* Public methods. */
        // TODO
        public override INode CreateNode(object obj, CreateNodeContext context) => new ListNode();
    }
}