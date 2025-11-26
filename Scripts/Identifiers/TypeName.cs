using System;
using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization;

/// <summary>
/// Represents a type name.
/// </summary>
public struct TypeName
{
    /* Fields. */
    private readonly string nameSpace;
    private readonly string name;
    private readonly TypeName[] genericArgs;

    /* Constructors. */
    public TypeName(Type type)
    {
        if (type.IsGenericParameter)
        {
            nameSpace = "";
            name = type.Name;
            genericArgs = [];
            return;
        }

        nameSpace = type.Namespace ?? "";

        Type[] genericArgTypes = type.GetGenericArguments();
        genericArgs = new TypeName[genericArgTypes.Length];
        for (int i = 0; i < genericArgTypes.Length; i++)
        {
            genericArgs[i] = new(genericArgTypes[i]);
        }

        name = type.Name;
        while (type.DeclaringType != null)
        {
            type = type.DeclaringType;
            name = type.Name + '+' + name;
        }
    }

    public TypeName(string name)
    {
        // Figure out namespace, main name and generic args parts.
        int namespaceIndex = -1;
        int genericsIndex = -1;

        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] == '.' && genericsIndex == -1)
                namespaceIndex = i;
            if (i < name.Length - 1 && name[i] == '[')
                genericsIndex = i;
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
            genericArgs = ParseGenericArgs(generics);
        }
        else
            genericArgs = [];

        // Type name.
        this.name = name;
    }

    private TypeName(string nameSpace, string name, TypeName[] genericArgs)
    {
        this.nameSpace = nameSpace;
        this.name = name;
        this.genericArgs = genericArgs;
    }

    /* Conversion operators. */
    public static implicit operator string(TypeName tn) => tn.ToString();
    public static implicit operator TypeName(string str) => new TypeName(str);

    /* Public methods. */
    public override string ToString()
    {
        StringBuilder str = new();

        if (nameSpace.Length > 0)
        {
            str.Append(nameSpace);
            str.Append('.');
        }

        str.Append(name);

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

    public TypeName Rename(string newName)
    {
        int index = name.IndexOf("[");
        if (index == -1)
            return this;

        return new("", newName + name.Substring(index), genericArgs);
    }

    public Type ParseType() => Type.GetType(name);

    /* Private methods. */
    private static TypeName[] ParseGenericArgs(string str)
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
            names[i] = new TypeName(terms[i]);
        }

        return names;
    }
}