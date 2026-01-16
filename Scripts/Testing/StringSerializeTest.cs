#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A struct serialization test monobehavior.
    /// </summary>
    public class StringSerializeTest : SerializeTest<string> { }

#if UNITY_EDITOR
    /// <summary>
    /// The editor for StringSerializeTest.
    /// </summary>
    [CustomEditor(typeof(StringSerializeTest))]
    public class StringSerializeTestEditor : SerializerTestEditor<StringSerializeTest, string> { }
}
#endif
#endif