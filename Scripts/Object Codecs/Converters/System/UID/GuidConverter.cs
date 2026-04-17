using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A GUID converter.
    /// </summary>
    public sealed class GuidConverter : UidConverter<Guid>
    {
        /* Protected methods. */
        protected override UidValue ToUid(Guid obj) => obj;
        protected override Guid FromUid(UidValue obj) => (Guid)obj;
    }
}