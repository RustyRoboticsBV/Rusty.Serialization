using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    public class AssignObjectContext : SubContext
    {
        /* Constructors. */
        public AssignObjectContext(ConversionContext context) : base(context) { }

        /* Public methods. */
        /// <summary>
        /// Create a child object.
        /// </summary>
        public T CreateChildObject<T>(INode node)
        {
            return (T)Context.CreateObjectContext.CreateObject(typeof(T), node);
        }

        /// <summary>
        /// Create a child object.
        /// </summary>
        public object CreateChildObject(Type type, INode node)
        {
            return Context.CreateObjectContext.CreateObject(type, node);
        }
    }
}