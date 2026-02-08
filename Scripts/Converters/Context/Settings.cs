using System;

namespace Rusty.Serialization
{
    /// <summary>
    /// The settings used by the serializer.
    /// </summary>
    [Flags]
    public enum Settings
    {
        None = 0,
        IncludeFormatHeader = 1,
        PrettyPrint = 2,
        All = IncludeFormatHeader | PrettyPrint
    }
}