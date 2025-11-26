using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rusty.Serialization
{
    /// <summary>
    /// Represents a fully qualified type name, including namespace, declaring types, and generic arguments.
    /// </summary>
    public struct FullTypeName
    {
        private readonly string @namespace;
        private readonly FullTypeName[] declaringTypes;
        private readonly TypeName name;

        public FullTypeName(Type type)
        {
            @namespace = type.Namespace;
            declaringTypes = GetDeclaringTypeChain(type);
            name = GetTypeName(type);
        }

        /// <summary>
        /// Converts FullTypeName to a fully qualified string including namespace and nested types.
        /// </summary>
        public static implicit operator string(FullTypeName typeName)
        {
            StringBuilder sb = new StringBuilder();

            // Namespace
            if (!string.IsNullOrEmpty(typeName.@namespace))
            {
                sb.Append(typeName.@namespace);
                sb.Append('.');
            }

            // Declaring types
            for (int i = 0; i < typeName.declaringTypes.Length; i++)
            {
                sb.Append(typeName.declaringTypes[i]);
                sb.Append('+');
            }

            // Main type
            sb.Append(typeName.name);

            return sb.ToString();
        }

        public static implicit operator TypeName(FullTypeName typeName) => (string)typeName;

        public override string ToString() => (string)this;

        // -------------------- Private Helpers --------------------

        /// <summary>
        /// Builds the chain of declaring types, fully qualified with their actual generic arguments.
        /// </summary>
        private static FullTypeName[] GetDeclaringTypeChain(Type type)
        {
            List<FullTypeName> chain = new List<FullTypeName>();
            if (!type.IsNested) return chain.ToArray();

            Stack<Type> stack = new Stack<Type>();
            Type current = type.DeclaringType;
            while (current != null)
            {
                stack.Push(current);
                current = current.DeclaringType;
            }

            Type[] allArgs = type.IsGenericType ? type.GetGenericArguments() : Type.EmptyTypes;
            int argIndex = 0;

            while (stack.Count > 0)
            {
                current = stack.Pop();

                // Generic parameters declared on this type
                Type[] genericParams = current.IsGenericType ? current.GetGenericTypeDefinition().GetGenericArguments() : Type.EmptyTypes;
                int count = genericParams.Length;

                Type[] ownArgs = new Type[count];
                for (int i = 0; i < count; i++)
                    ownArgs[i] = allArgs[argIndex + i];

                argIndex += count;

                chain.Add(new FullTypeName(current, ownArgs));
            }

            return chain.ToArray();
        }

        /// <summary>
        /// Constructs a FullTypeName from a type and its actual generic arguments.
        /// </summary>
        private FullTypeName(Type type, Type[] actualGenericArgs)
        {
            @namespace = type.Namespace;
            declaringTypes = GetDeclaringTypeChain(type);
            name = GetTypeName(type, actualGenericArgs);
        }

        /// <summary>
        /// Returns the type name for a type including its generic arguments.
        /// </summary>
        private static string GetTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            Type[] allArgs = type.GetGenericArguments();
            int outerCount = type.DeclaringType != null && type.DeclaringType.IsGenericType
                ? type.DeclaringType.GetGenericTypeDefinition().GetGenericArguments().Length
                : 0;

            int ownCount = allArgs.Length - outerCount;
            Type[] ownArgs = new Type[ownCount];
            for (int i = 0; i < ownCount; i++)
                ownArgs[i] = allArgs[outerCount + i];

            return GetTypeName(type, ownArgs);
        }

        /// <summary>
        /// Returns the type name including actual generic arguments.
        /// </summary>
        private static string GetTypeName(Type type, Type[] actualGenericArgs)
        {
            string name = type.Name;
            int backtick = name.IndexOf('`');
            if (backtick >= 0)
                name = name.Substring(0, backtick);

            if (actualGenericArgs != null && actualGenericArgs.Length > 0)
            {
                StringBuilder args = new StringBuilder();
                for (int i = 0; i < actualGenericArgs.Length; i++)
                {
                    if (i > 0) args.Append(", ");
                    args.Append(new FullTypeName(actualGenericArgs[i]));
                }

                name += $"<{args}>";
            }

            return name;
        }
    }
}
