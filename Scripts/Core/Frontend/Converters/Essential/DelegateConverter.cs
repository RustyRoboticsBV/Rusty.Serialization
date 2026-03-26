using Rusty.Serialization.Core.Nodes;
using System;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A delegate converter.
    /// </summary>
    public class DelegateConverter<T> : Converter
        where T : Delegate
    {
        /* Public methods. */
        // TODO
        public override INode CreateNode(object obj, CreateNodeContext context)
        {
            throw new NotImplementedException();
        }
    }
}