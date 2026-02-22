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
        public static void Print(object obj)
        {
            var visited = new Dictionary<object, int>(new ReferenceEqualityComparer());
            PrintObject(obj, "root", 0, visited);
        }

        /* Private methods. */
        private static void PrintObject(object obj, string name, int indent, Dictionary<object, int> visited)
        {
            string indentStr = new string(' ', indent * 2);

            // Null.
            if (obj == null)
            {
                Console.WriteLine($"{indentStr}{name}: null");
                return;
            }

            Type type = obj.GetType();

            // Simple value types: print directly.
            if (IsSimple(type))
            {
                Console.WriteLine($"{indentStr}{name}: ({type.Name}) {obj}");
                return;
            }

            // If object already visited: avoid cycle.
            if (visited.ContainsKey(obj))
            {
                Console.WriteLine($"{indentStr}{name}: ({type.Name}) ==> #{visited[obj]}");
                return;
            }

            // Assign reference address.
            int address = visited.Count + 1;
            visited[obj] = address;

            // Strings.
            if (obj is string)
            {
                Console.WriteLine($"{indentStr}{name}: ({type.Name}) {obj} [Address #{address}]");
                return;
            }

            // Key-value pairs.
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                Console.WriteLine($"{indentStr}{name}: ({type.Name}) [Address #{address}]");
                PrintObject(type.GetProperty("Key").GetValue(obj), "key", indent + 1, visited);
                PrintObject(type.GetProperty("Value").GetValue(obj), "value", indent + 1, visited);
                return;
            }

            // Class or struct header.
            else
            {
                Console.WriteLine($"{indentStr}{name}: ({type.Name}) [Address #{address}]");

                // Collections (IEnumerable).
                if (obj is IEnumerable enumerable && !(obj is string))
                {
                    int index = 0;
                    foreach (var item in enumerable)
                    {
                        PrintObject(item, $"{name}[{index}]", indent + 1, visited);
                        index++;
                    }
                }

                // Fields.
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    var value = SafeGet(() => field.GetValue(obj));
                    PrintObject(value, field.Name, indent + 1, visited);
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
                    PrintObject(value, prop.Name, indent + 1, visited);
                }
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