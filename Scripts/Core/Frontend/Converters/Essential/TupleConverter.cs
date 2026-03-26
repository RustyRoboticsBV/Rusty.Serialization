using Rusty.Serialization.Core.Nodes;
using System.Runtime.CompilerServices;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An enum flags converter.
    /// </summary>
    public class TupleConverter<T> : Converter
        where T : ITuple
    {
        /* Public methods. */
        // TODO
        public override INode CreateNode(object obj, CreateNodeContext context) => new ListNode();
    }
}