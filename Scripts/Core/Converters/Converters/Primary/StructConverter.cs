using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A struct type converter.
    /// </summary>
    public class StructConverter<T> : ValueConverter<T, ObjectNode>
        where T : struct
    {
        /* Protected properties. */
        protected virtual HashSet<string> IgnoredMembers => new();

        /* Protected methods. */
        protected override ObjectNode CreateNode(T obj, CreateNodeContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override T CreateObject(ObjectNode node, CreateObjectContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}