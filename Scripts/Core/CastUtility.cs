using System;
using System.Reflection;

namespace Rusty.Serialization.Core
{
    /// <summary>
    /// A utility for finding casting operators.
    /// </summary>
    internal class CastUtility
    {
        /* Public methods. */
        /// <summary>
        /// Check if two types can be converted into each other.
        /// </summary>
        public static bool IsBilaterallyConvertable(Type a, Type b)
        {
            return TryFindCast(a, b, out _) && TryFindCast(b, a, out _);
        }

        /// <summary>
        /// Try to find a casting operator from 'fromType' to 'toType'.
        /// </summary>
        public static bool TryFindCast(Type from, Type to, out MethodInfo method)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            method = FindCast(from, from, to);
            if (method == null)
                method = FindCast(to, from, to);

            return method != null;
        }

        /* Private methods. */
        /// <summary>
        /// Try to find a casting operator on type 'typeToSearch' from 'fromType' to 'toType'.
        /// Returns null on failure.
        /// TODO: this returns the first eligible operator. Make it so that the cast with an argument closest to the fromType is used.
        /// </summary>
        private static MethodInfo FindCast(Type typeToSearch, Type fromType, Type toType)
        {
            // Get all public static methods.
            MethodInfo[] methods = typeToSearch.GetMethods(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            // For each method...
            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo method = methods[i];

                // Ignore methods that are not operators.
                if (method.Name != "op_Implicit" && method.Name != "op_Explicit")
                    continue;

                // Ignore methods with a non-matching return type.
                if (method.ReturnType != toType)
                    continue;

                // Choose this method if it has one parameter that our fromType can be assigned to.
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(fromType))
                    return method;
            }

            // Return null if no suitable method was found.
            return null;
        }

        /// <summary>
        /// Get the inheritance distance between two types.
        /// </summary>
        private static int GetTypeDistance(Type from, Type to)
        {
            // If the types are the same, the distance is 0.
            if (from == to)
                return 0;

            // Find the distance.
            int distance = 0;
            Type current = from;
            while (current != null)
            {
                if (current == to)
                    return distance;

                current = current.BaseType;
                distance++;
            }

            // Handle interfaces.
            if (to.IsInterface && to.IsAssignableFrom(from))
                return distance + 1;

            // Error if the types did not share an inheritance relationship.
            throw new ArgumentException($"The types '{from}' and '{to}' do not share an inheritance chain.");
        }
    }
}
