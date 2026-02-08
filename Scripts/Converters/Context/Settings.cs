using System;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace Rusty.Serialization
{
    /// <summary>
    /// The settings used by the serializer.
    /// </summary>
    [Serializable]
    public struct Settings
    {
        /* Fields. */
        public static Settings All => new Settings()
        {
            PrettyPrint = true,
            IncludeFormatHeader = true
        };

#if UNITY_5_3_OR_NEWER
        [SerializeField]
#endif
        public bool PrettyPrint;
#if UNITY_5_3_OR_NEWER
        [SerializeField]
#endif
        public bool IncludeFormatHeader;
    }
}