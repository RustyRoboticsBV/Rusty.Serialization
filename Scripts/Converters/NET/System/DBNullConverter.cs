#if NETCOREAPP2_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET database null converter.
    /// </summary>
    public class DBNullConverter : Core.Conversion.Converter<DBNull, NullNode>
    {
        /* Protected method. */
        protected override NullNode CreateNode(DBNull obj, CreateNodeContext context)
        {
            return new NullNode();
        }

        protected override DBNull CreateObject(NullNode node, CreateObjectContext context)
        {
            return DBNull.Value;
        }
    }
}
#endif