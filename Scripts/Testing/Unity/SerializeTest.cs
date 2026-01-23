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
        [SerializeField] public bool PrettyPrint;
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
        /* Unity events. */
        public override void OnInspectorGUI()
        {

            T t = (T)target;
            // Draw buttons.
            if (GUILayout.Button("Serialize"))
            {
                string serialized = UCS.Serialize(t.Object, t.Format, t.PrettyPrint);
                t.Serialized = serialized;
            }
            if (GUILayout.Button("Deserialize"))
            {
                if (t.Serialized == "")
                    Debug.LogError("Serialized text field is empty!! This is not a serializer bug!");
                else
                    t.Object = UCS.Parse<U>(t.Serialized, t.Format);
            }
            if (GUILayout.Button("Clear Object"))
                t.Object = GetCleared();
            if (GUILayout.Button("Clear Text"))
                t.Serialized = "";
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