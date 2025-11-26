using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rusty.Serialization;

public struct FullTypeName
{
    /* Fields. */
    private readonly TypeName @namespace;
    private readonly TypeName[] declaringTypes;
    private readonly TypeName name;

    /* Constructors. */
    public FullTypeName(Type type) : this(type.Namespace, GetDeclaringTypeChain(type), GetTypeName(type)) { }

    public FullTypeName(TypeName @namespace, TypeName[] declaringTypes, TypeName name)
    {
        this.@namespace = @namespace;
        this.declaringTypes = declaringTypes;
        this.name = name;
    }

    /* Conversion operators. */
    public static implicit operator TypeName(FullTypeName name)
    {
        StringBuilder sb = new();

        // Add namespace.
        if (name.@namespace != null)
        {
            sb.Append(name.@namespace);
            sb.Append('.');
        }

        // Add declaring types.
        for (int i = 0; i < name.declaringTypes.Length; i++)
        {
            sb.Append(name.declaringTypes[i]);
            if (i < name.declaringTypes.Length - 1)
                sb.Append('.');
        }

        // Add main type.
        if (name.declaringTypes.Length > 0)
            sb.Append('+');
        sb.Append(name.name);

        return sb.ToString();
    }

    /* Public method */
    public override string ToString() => name;

    /* Private methods. */
    /*    private static TypeName[] GetDeclaringTypeChain(Type type)
        {
            List<TypeName> declaringTypes = new();
            while (type.DeclaringType != null)
            {
                // Get declaring type.
                type = type.DeclaringType;

                // Store the type's name.
                declaringTypes.Insert(0, GetTypeName(type));
            }
            return declaringTypes.ToArray();
        }

        private static TypeName GetTypeName(Type type)
        {
            // Check if the serializing attribute is present.
            var attribute = type.GetCustomAttribute<SerializableAttribute>();
            if (attribute != null)
                return attribute.Name;

            // Otherwise, use the type's name.
            else
            {
                string name = type.Name;
                System.Console.WriteLine(type);

                // Remove the backtick + arity if present.
                int backtickIndex = name.IndexOf('`');
                if (backtickIndex >= 0)
                {
                    name = name.Substring(0, backtickIndex);
                }

                // If generic, add generic arguments.
                if (type.IsGenericType)
                {
                    Type[] genericArgs = type.GetGenericArguments();

                    // Count of generic arguments belonging to declaring type(s).
                    int declaringGenericCount = 0;
                    if (type.DeclaringType != null && type.DeclaringType.IsGenericType)
                    {
                        declaringGenericCount = type.DeclaringType.GetGenericArguments().Length;
                    }

                    // Only take the type's own generic arguments.
                    Type[] ownArgs = new Type[genericArgs.Length - declaringGenericCount];
                    for (int i = 0; i < ownArgs.Length; i++)
                    {
                        ownArgs[i] = genericArgs[declaringGenericCount + i];
                    }

                    string args = "";
                    for (int i = 0; i < ownArgs.Length; i++)
                    {
                        if (i > 0)
                            args += ", ";
                        args += new FullTypeName(ownArgs[i]);
                    }

                    name = name + '<' + args + '>';
                }

                return name;
            }
        }*/
    private static string GetFullName(Type type, Type[] actualGenericArgs)
    {
        string name = type.Name;
        int backtick = name.IndexOf('`');
        if (backtick >= 0) name = name.Substring(0, backtick);

        if (actualGenericArgs != null && actualGenericArgs.Length > 0)
        {
            string args = "";
            for (int i = 0; i < actualGenericArgs.Length; i++)
            {
                if (i > 0) args += ", ";
                args += GetFullName(actualGenericArgs[i], null); // recursive
            }
            name += "<" + args + ">";
        }

        if (type.DeclaringType != null)
            return GetFullName(type.DeclaringType, null) + "+" + name;

        if (!string.IsNullOrEmpty(type.Namespace))
            return type.Namespace + "." + name;

        return name;
    }

    private static TypeName[] GetDeclaringTypeChain(Type type)
    {
        List<TypeName> declaringTypes = new List<TypeName>();
        if (!type.IsNested) return declaringTypes.ToArray();

        // Build a stack of declaring types from outermost to innermost
        Stack<Type> stack = new Stack<Type>();
        Type current = type.DeclaringType;
        while (current != null)
        {
            stack.Push(current);
            current = current.DeclaringType;
        }

        // All actual generic arguments in flat order
        Type[] allArgs = type.IsGenericType ? type.GetGenericArguments() : Type.EmptyTypes;
        int argIndex = 0;

        while (stack.Count > 0)
        {
            current = stack.Pop();

            // How many generic parameters this type declares
            Type[] genericParams = current.IsGenericType ? current.GetGenericTypeDefinition().GetGenericArguments() : Type.EmptyTypes;
            int count = genericParams.Length;

            // Slice the next N arguments for this type
            Type[] ownArgs = new Type[count];
            for (int i = 0; i < count; i++)
                ownArgs[i] = allArgs[argIndex + i];

            argIndex += count;

            declaringTypes.Add(GetTypeName(current, ownArgs));
        }

        return declaringTypes.ToArray();
    }

    private static TypeName GetTypeName(Type type, Type[] actualGenericArgs)
    {
        string name = type.Name;
        int backtick = name.IndexOf('`');
        if (backtick >= 0) name = name.Substring(0, backtick);

        if (actualGenericArgs != null && actualGenericArgs.Length > 0)
        {
            string args = "";
            for (int i = 0; i < actualGenericArgs.Length; i++)
            {
                if (i > 0) args += ", ";
                args += new FullTypeName(actualGenericArgs[i]);
            }
            name += "<" + args + ">";
        }

        return name;
    }

    private static TypeName GetTypeName(Type type)
    {
        // Main type (last in chain)
        Type[] allArgs = type.IsGenericType ? type.GetGenericArguments() : Type.EmptyTypes;

        int outerCount = type.DeclaringType != null && type.DeclaringType.IsGenericType
            ? type.DeclaringType.GetGenericTypeDefinition().GetGenericArguments().Length
            : 0;

        int ownCount = allArgs.Length - outerCount;
        Type[] ownArgs = new Type[ownCount];
        for (int i = 0; i < ownCount; i++)
            ownArgs[i] = allArgs[outerCount + i];

        return GetTypeName(type, ownArgs);
    }

}