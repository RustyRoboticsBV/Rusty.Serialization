using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

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

            if (obj == null)
            {
                Console.WriteLine($"{indentStr}{name}: null");
                return;
            }

            Type type = obj.GetType();

            // Value types: print directly.
            if (type.IsValueType)
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

            // Assign reference ID.
            int id = visited.Count + 1;
            visited[obj] = id;

            // Strings.
            if (obj is string)
                Console.WriteLine($"{indentStr}{name}: ({type.Name}) {obj} [ID #{id}]");
            else
                Console.WriteLine($"{indentStr}{name}: ({type.Name}) [ID #{id}]");

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

                // Skip indexers
                if (prop.GetIndexParameters().Length > 0)
                    continue;

                var value = SafeGet(() => prop.GetValue(obj, null));
                PrintObject(value, prop.Name, indent + 1, visited);
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
            public int GetHashCode(object obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        }
    }
}