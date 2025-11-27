using System;
using System.Collections.Generic;
using System.Text;
using Rusty.Serialization.Converters;

namespace Rusty.Serialization;

/// <summary>
/// Represents a type name.
/// </summary>
public struct TypeName
{
    /* Fields. */
    private readonly AliasRegistry context;
    private readonly Type type;

    private readonly string nameSpace;
    private readonly string name;
    private readonly TypeName[] genericArgs;

    /* Constructors. */
    public TypeName(Type type, AliasRegistry context)
    {
        // Store type & context.
        this.type = type;
        this.context = context;

        // Handle null.
        if (type == null)
        {
            nameSpace = "";
            name = "null";
            genericArgs = [];
            return;
        }

        // Handle generic type parameters.
        if (type.IsGenericParameter)
        {
            nameSpace = "";
            name = type.Name;
            genericArgs = [];
            return;
        }

        // Namespace.
        nameSpace = type.Namespace ?? "";

        // Generic type arguments.
        Type[] genericArgTypes = type.GetGenericArguments();
        genericArgs = new TypeName[genericArgTypes.Length];
        for (int i = 0; i < genericArgTypes.Length; i++)
        {
            genericArgs[i] = new(genericArgTypes[i], context);
        }

        // Main name.
        name = type.Name;
        while (type.DeclaringType != null)
        {
            type = type.DeclaringType;
            name = type.Name + '+' + name;
        }
    }

    public TypeName(string name, AliasRegistry context)
    {
        // Figure out namespace, main name and generic args parts.
        int namespaceIndex = -1;
        int genericsIndex = -1;

        int genericDepth = 0;
        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] == '.' && genericsIndex == -1)
                namespaceIndex = i;
            if (name[i] == '[')
            {
                if (genericDepth == 0)
                    genericsIndex = i;
                genericDepth++;
            }
            if (name[i] == ']')
                genericDepth--;
        }

        // Namespace.
        if (namespaceIndex >= 0)
        {
            nameSpace = name.Substring(0, namespaceIndex);
            name = name.Substring(namespaceIndex + 1);
        }
        else
            nameSpace = "";

        // Generic arguments.
        if (genericsIndex >= 0)
        {
            string generics = name.Substring(genericsIndex + 2);
            name = name.Substring(0, genericsIndex);
            genericArgs = ParseGenericArgs(generics, context);
        }
        else
            genericArgs = [];

        // Type name.
        this.name = name;

        // Parse type.
        type = Type.GetType(name);
    }

    /* Conversion operators. */
    public static implicit operator Type(TypeName tn) => tn.type;
    public static implicit operator string(TypeName tn) => tn.ToString();

    /* Public methods. */
    public override string ToString()
    {
        StringBuilder str = new();

        // Add namespace.
        if (nameSpace.Length > 0)
        {
            str.Append(nameSpace);
            str.Append('.');
        }

        // Add type.
        str.Append(name);

        // Apply alias.
        if (context.Has(type))
        {
            str.Clear();
            str.Append(context.Get(type));
        }

        // Add generic type arguments.
        if (genericArgs.Length > 0)
        {
            str.Append("[");
            for (int i = 0; i < genericArgs.Length; i++)
            {
                if (i > 0)
                    str.Append(',');
                str.Append(genericArgs[i]);
            }
            str.Append("]");
        }

        return str.ToString();
    }

    public override int GetHashCode()
    {
        int hashcode = (nameSpace.GetHashCode() ^ 11 + name.GetHashCode()) ^ 11;
        foreach (TypeName type in genericArgs)
        {
            hashcode = hashcode ^ 11 + type.GetHashCode();
        }
        return hashcode;
    }

    /* Private methods. */
    private static TypeName[] ParseGenericArgs(string str, AliasRegistry context)
    {
        if (str.StartsWith('[') && str.EndsWith(']'))
            str = str.Substring(1, str.Length - 2);

        int depth = 0;
        int termStart = 0;
        List<string> terms = new();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '[')
                depth++;
            else if (str[i] == ']')
                depth--;
            else if (str[i] == ',' && depth == 0)
            {
                terms.Add(str.Substring(termStart, i));
                termStart = i + 1;
            }
        }
        terms.Add(str.Substring(termStart));

        TypeName[] names = new TypeName[terms.Count];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = new TypeName(terms[i], context);
        }

        return names;
    }
}