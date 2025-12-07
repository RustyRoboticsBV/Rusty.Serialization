using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.DBNull converter.
    /// </summary>
    public sealed class DBNullConverter : ReferenceConverter<DBNull, NullNode>
    {
        /* Protected methods. */
        protected override NullNode ConvertRef(DBNull obj, IConverterScheme scheme, NodeTree tree) => new();
        protected override DBNull DeconvertRef(NullNode node, IConverterScheme scheme, NodeTree tree) => DBNull.Value;
    }
}