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
        protected override NullNode ConvertRef(DBNull obj, IConverterScheme scheme) => new();
        protected override DBNull DeconvertRef(NullNode node, IConverterScheme scheme) => DBNull.Value;
    }
}