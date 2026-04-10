using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    public sealed class PopulateNodeContext : Context
    {
        public PopulateNodeContext(ObjectCodec codec) : base(codec) { }

        public INode CreateNode(Type declaredType, object obj)
        {
            return null;
        }
    }
}