using System.Collections.Generic;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET list converter.
    /// </summary>
    public class ListConverter<T> : ListConverter<List<T>, T>
    {
    }
}
