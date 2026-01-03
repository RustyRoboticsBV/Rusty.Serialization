using System.Collections.Generic;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET hash set converter.
    /// </summary>
    public class HashSetConverter<T> : SetConverter<HashSet<T>, T>
    {
    }
}
