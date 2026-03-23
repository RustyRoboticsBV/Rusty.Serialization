using System;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET URI converter.
    /// </summary>
    public class UriConverter : TypedStringConverter<Uri>
    {
        /* Protected method. */
        protected override Uri FromString(string str) => new Uri(str);
    }
}