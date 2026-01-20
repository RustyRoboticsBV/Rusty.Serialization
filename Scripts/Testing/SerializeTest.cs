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
        /* Fields. */
        private DefaultContext cscd = new(Format.Cscd);
        private DefaultContext json = new(Format.Json);
        private DefaultContext xml = null;// new(Format.Xml);

        /* Unity events. */
        public override void OnInspectorGUI()
        {
            T t = (T)target;

            DefaultContext context = GetContext(t.Format);

            // Draw buttons.
            if (GUILayout.Button("Serialize"))
            {
                string serialized = context.Serialize(t.Object, t.PrettyPrint);
                t.Serialized = serialized;
            }
            if (GUILayout.Button("Deserialize"))
            {
                if (t.Format == Format.Cscd)
                {
                    CSCD.Lexing.Lexer lexer = new();
                    CSCD.Parsing.Parser parser = new();
                    Debug.Log(parser.Parse(t.Serialized.AsSpan(), lexer));
                }
                if (t.Serialized == "")
                    Debug.LogError("Serialized text field is empty!! This is not a serializer bug!");
                else
                {
                    U obj = context.Parse<U>(t.Serialized);
                    t.Object = obj;
                }
            }
            if (GUILayout.Button("Clear Object"))
                t.Object = GetCleared();
            if (GUILayout.Button("Clear Text"))
                t.Serialized = "";
            GUILayout.Space(10);

            // Draw normal inspector.
            DrawDefaultInspector();
        }

        private DefaultContext GetContext(Format format)
        {
            switch (format)
            {
                case Format.Cscd:
                    return cscd;
                case Format.Json:
                    return json;
                case Format.Xml:
                    return xml;
                default:
                    throw new Exception();
            }
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