using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rusty.Serialization.Testing
{
    public static class ObjectDumper
    {
        /* Public methods. */
        public static string Dump(object obj)
        {
            var visited = new Dictionary<object, int>(new ReferenceEqualityComparer());
            return Dump(obj, "root", 0, visited);
        }

        /* Private methods. */
        private static string Dump(object obj, string name, int indent, Dictionary<object, int> visited)
        {
            string indentStr = new string(' ', indent * 2);

            // Null.
            if (obj == null)
                return $"{indentStr}{name}: null";

            Type type = obj.GetType();

            // Simple value types: print directly.
            if (IsSimple(type))
                return $"{indentStr}{name}: ({type.Name}) {obj}";

            // If object already visited: avoid cycle.
            if (visited.ContainsKey(obj))
                return $"{indentStr}{name}: ({type.Name}) ==> #{visited[obj]}";

            // Assign reference address.
            int address = visited.Count + 1;
            visited[obj] = address;

            // Strings.
            if (obj is string)
                return $"{indentStr}{name}: ({type.Name}) {obj} [Address #{address}]";

            // Date/time.
            if (obj is DateTime dt)
                return $"{indentStr}{name}: ({type.Name}) {obj} [Address #{address}]";

            // Key-value pairs.
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                return $"{indentStr}{name}: ({type.Name}) [Address #{address}]"
                    + "\n" + Dump(type.GetProperty("Key").GetValue(obj), "key", indent + 1, visited)
                    + "\n" + Dump(type.GetProperty("Value").GetValue(obj), "value", indent + 1, visited);
            }

            // Class or struct header.
            else
            {
                string str = $"{indentStr}{name}: ({type.Name}) [Address #{address}]";

                // Collections (IEnumerable).
                if (obj is IEnumerable enumerable && !(obj is string))
                {
                    int index = 0;
                    foreach (var item in enumerable)
                    {
                        str += "\n" + Dump(item, $"{name}[{index}]", indent + 1, visited);
                        index++;
                    }
                }

                // Fields.
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    var value = SafeGet(() => field.GetValue(obj));
                    str += "\n" + Dump(value, field.Name, indent + 1, visited);
                }

                // Properties.
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    // Skip readonly properties.
                    if (prop.SetMethod == null)
                        continue;

                    // Skip indexers.
                    if (prop.GetIndexParameters().Length > 0)
                        continue;

                    var value = SafeGet(() => prop.GetValue(obj, null));
                    str += "\n" + Dump(value, prop.Name, indent + 1, visited);
                }

                return str;
            }
        }

        private static object SafeGet(Func<object> getter)
        {
            try
            {
                return getter();
            }
            catch
            {
                return "[unavailable]";
            }
        }

        /// <summary>
        /// Required to compare object references, not values.
        /// </summary>
        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y) => ReferenceEquals(x, y);
            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }

        /// <summary>
        /// Check if a type is a simple type.
        /// </summary>
        private static bool IsSimple(Type type)
        {
            return type.IsPrimitive
                || type.IsEnum
                || type == typeof(decimal);
        }
    }
}