using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET guid converter.
    /// </summary>
    public class GuidConverter : TypedUidConverter<Guid>
    {
        /* Protected method. */
        protected override UidValue ToUid(Guid obj) => obj;
        protected override Guid FromUid(UidValue value) => value;
    }
}