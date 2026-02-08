#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A serialize test monobehavior.
    /// </summary>
    public class SerializeTest<T> : MonoBehaviour
    {
        private enum PerformanceTest { Off, Serialize, Parse, RoundTrip }

        [SerializeField] public Format Format = Format.Cscd;
        [SerializeField] public Settings Settings = Settings.All;
        [SerializeField] PerformanceTest TestPerformance;
        [SerializeField] public T Object;
        [SerializeField, Multiline(50)] public string Serialized;
    }

#if UNITY_EDITOR
    /// <summary>
    /// The editor for a Serialization test component.
    /// </summary>
    public class SerializerTestEditor<T, U> : Editor
        where T : SerializeTest<U>
    {
        public UCS cscd = new UCS(UCS.DefaultConverters, Format.Cscd);
        public UCS json = new UCS(UCS.DefaultConverters, Format.Json);
        public UCS xml = new UCS(UCS.DefaultConverters, Format.Xml);

        /* Unity events. */
        public override void OnInspectorGUI()
        {
            T t = (T)target;

            UCS ucs = cscd;
            if (t.Format == Format.Json)
                ucs = json;
            if (t.Format == Format.Xml)
                ucs = xml;

            // Draw buttons.
            if (GUILayout.Button("Serialize"))
            {
                string serialized = ucs.Serialize(t.Object, t.Settings);
                t.Serialized = serialized;
            }
            if (GUILayout.Button("Deserialize"))
            {
                if (t.Serialized == "")
                    Debug.LogError("Serialized text field is empty!! This is not a serializer bug!");
                else
                    t.Object = ucs.Parse<U>(t.Serialized);
            }
            if (GUILayout.Button("Clear Object"))
                t.Object = GetCleared();
            if (GUILayout.Button("Clear Text"))
                t.Serialized = "";
            if (GUILayout.Button("Free Memory"))
                ucs.Free();
            GUILayout.Space(10);

            // Draw normal inspector.
            DrawDefaultInspector();
        }

        private static U GetCleared()
        {
            if (typeof(U) == typeof(string))
                return (U)(object)"";
            else
                return Activator.CreateInstance<U>();
        }
    }
}
#endif
#endif