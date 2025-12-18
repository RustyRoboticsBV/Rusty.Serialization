using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Dotnet
{
    /// <summary>
    /// A list converter.
    /// </summary>
    public sealed class ListConverter<T> : ListConverter<List<T>, T>
    { }
}