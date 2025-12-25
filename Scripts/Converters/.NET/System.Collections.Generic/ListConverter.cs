using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET dictionary converter.
    /// </summary>
    public class ListConverter<T> : ListConverter<List<T>, T>
    {
    }
}
