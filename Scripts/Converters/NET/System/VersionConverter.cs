using System;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET version converter.
    /// </summary>
    public class VersionConverter :  TypedStringConverter<Version>
    {
        /* Protected method. */
        protected override Version FromString(string str) => Version.Parse(str);
    }
}