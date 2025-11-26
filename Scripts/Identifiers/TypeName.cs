using System;

namespace Rusty.Serialization;

public struct TypeName
{
    /* Fields. */
    private readonly string name;

    /* Constructors. */
    public TypeName(Type type)
    {
        if (type.IsGenericParameter)
        {
            name = type.Name;
            return;
        }

        string @namespace = type.Namespace;
        Type[] genericArgs = type.GenericTypeArguments;

        name = RemoveBacktick(type.Name);

        while (type.DeclaringType != null)
        {
            type = type.DeclaringType;
            name = RemoveBacktick(type.Name) + '+' + name;
        }

        if (@namespace != null)
            name = @namespace + '.' + name;

        if (genericArgs.Length > 0)
        {
            name += '<';
            for (int i = 0; i < genericArgs.Length; i++)
            {
                if (i > 0)
                    name += ',';
                name += new TypeName(genericArgs[i]);
            }
            name += '>';
        }
    }

    public TypeName(string name)
    {
        this.name = name;
    }

    /* Conversion operators. */
    public static implicit operator string(TypeName identifier) => identifier.name;
    public static implicit operator TypeName(string name) => new TypeName(name);

    /* Public methods. */
    public override string ToString() => name;
    public override int GetHashCode() => name?.GetHashCode() ?? 0;

    public TypeName Rename(string newName)
    {
        int index = name.IndexOf('<');
        return newName + name.Substring(index);
    }

    /* Private methods. */
    private static string RemoveBacktick(string str)
    {
        int backtickIndex = str.IndexOf('`');
        if (backtickIndex >= 0)
            return str.Substring(0, backtickIndex);
        return str;
    }
}
