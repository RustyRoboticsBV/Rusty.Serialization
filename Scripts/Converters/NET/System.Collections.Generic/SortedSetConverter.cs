using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET sorted set converter.
    /// </summary>
    public class SortedSetConverter<T> : SetConverter<SortedSet<T>, T>
    {
    }
}
