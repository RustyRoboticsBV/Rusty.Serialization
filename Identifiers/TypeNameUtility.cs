using System;
using System.Linq;

namespace Rusty.Serialization;

/// <summary>
/// An utility for generating type names.
/// </summary>
public static class TypeNameUtility
{
    /* Public methods. */
    public static string GetFriendlyName(Type type)
    {
        if (type.IsGenericType)
            return GetGenericName(type);

        if (type.IsArray)
            return GetArrayName(type);

        if (Nullable.GetUnderlyingType(type) != null)
            return GetNullableName(type);

        return GetSimpleName(type);
    }

    /* Private methods. */
    private static string GetSimpleName(Type type)
    {
        if (type.DeclaringType != null)
            return GetSimpleName(type.DeclaringType) + "." + type.Name;

        return string.IsNullOrEmpty(type.Namespace)
            ? type.Name
            : $"{type.Namespace}.{type.Name}";
    }

    private static string GetGenericName(Type type)
    {
        var genericDef = type.GetGenericTypeDefinition();
        string baseName = genericDef.Name[..genericDef.Name.IndexOf('`')];

        // Example: Namespace.Outer+Inner<T> → Namespace.Outer.Inner<T>
        string fullBase = (genericDef.DeclaringType != null)
            ? GetFriendlyName(genericDef.DeclaringType) + "." + baseName
            : $"{genericDef.Namespace}.{baseName}";

        var args = type.GetGenericArguments()
            .Select(GetFriendlyName);

        return $"{fullBase}<{string.Join(", ", args)}>";
    }

    private static string GetArrayName(Type type)
    {
        return $"{GetFriendlyName(type.GetElementType())}[{new string(',', type.GetArrayRank() - 1)}]";
    }

    private static string GetNullableName(Type type)
    {
        var inner = Nullable.GetUnderlyingType(type);
        return GetFriendlyName(inner) + "?";
    }
}