#if UNITY_5_3_OR_NEWER
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// A struct serialization test monobehavior.
    /// </summary>
    public class TimeSerializeTest : SerializeTest<DateTime>
    {
        [SerializeField] int year = 1;
        [SerializeField] int month = 1;
        [SerializeField] int day = 1;
        [SerializeField] int hour;
        [SerializeField] int minute;
        [SerializeField] int second;

        private void OnValidate()
        {
            Object = new DateTime(year, month, day, hour, minute, second);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// The editor for TimeSerializeTest.
    /// </summary>
    [CustomEditor(typeof(TimeSerializeTest))]
    public class TimeSerializeTestEditor : SerializerTestEditor<TimeSerializeTest, DateTime> { }
}
#endif
#endif