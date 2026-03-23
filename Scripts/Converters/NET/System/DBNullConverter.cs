#if NETCOREAPP2_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET database null converter.
    /// </summary>
    public class DBNullConverter : Core.Conversion.TypedConverter<DBNull, NullNode>
    {
        /* Protected method. */
        protected override NullNode CreateNode2(DBNull obj, CreateNodeContext context)
        {
            return new NullNode();
        }

        protected override DBNull CreateObject2(NullNode node, CreateObjectContext context)
        {
            return DBNull.Value;
        }
    }
}
#endif