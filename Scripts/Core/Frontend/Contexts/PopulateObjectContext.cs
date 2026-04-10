using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    public sealed class PopulateObjectContext : Context
    {
        /* Constructors. */
        public PopulateObjectContext(ObjectCodec codec) : base(codec) { }

        /* Public methods. */
        public object CreateChildObject(INode node, Type type)
        {
            return Codec.CreateObjectContext.CreateObject(node, type);
        }
    }
}