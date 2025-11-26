using System;
using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization;

public readonly struct FullTypeName
{
    /* Fields. */
    private readonly string @namespace;
    private readonly List<Segment> segments;

    /* Private types. */
    private readonly struct Segment
    {
        public readonly string name;
        public readonly FullTypeName[] genericArgs;

        public Segment(string name, FullTypeName[] genericArgs)
        {
            this.name = name;
            this.genericArgs = genericArgs;
        }
    }

    /* Constructors. */
    public FullTypeName(Type type)
    {
        if (type.IsGenericParameter)
        {
            @namespace = "";
            segments = new() { new Segment(type.Name, []) };
            return;
        }

        // Build declaring type chain, outer to inner.
        var chain = new List<Type>();
        var cursor = type;

        while (cursor != null)
        {
            chain.Insert(0, cursor);
            cursor = cursor.DeclaringType;
        }

        @namespace = chain[0].Namespace ?? "";

        // Flatten all generic args in order.
        var flatArgs = type.IsGenericType ? type.GetGenericArguments() : [];
        int index = 0;

        segments = new List<Segment>();

        foreach (var t in chain)
        {
            // Count generic args belonging to THIS declaring level.
            int ownCount = 0;

            if (t.IsGenericType)
            {
                ownCount = t.GetGenericTypeDefinition()
                            .GetGenericArguments()
                            .Length;

                // But this count includes outer generic args for nested types.
                // Adjust down to actual number belonging to this type.
                if (t.DeclaringType != null && t.DeclaringType.IsGenericType)
                {
                    int outerCount = t.DeclaringType
                                      .GetGenericTypeDefinition()
                                      .GetGenericArguments().Length;

                    ownCount -= outerCount;
                }
            }

            // Consume correct subset.
            var argList = new FullTypeName[ownCount];
            for (int i = 0; i < ownCount; i++)
            {
                argList[i] = new FullTypeName(flatArgs[index++]);
            }

            // Strip backtick.
            string name = t.Name;
            int pos = name.IndexOf('`');
            if (pos >= 0)
                name = name[..pos];

            segments.Add(new Segment(name, argList));
        }
    }

    /* Public methods. */
    public override string ToString()
    {
        var sb = new StringBuilder();

        // Add namespace.
        if (!string.IsNullOrEmpty(@namespace))
            sb.Append(@namespace).Append('.');

        // Add nested segments.
        for (int i = 0; i < segments.Count; i++)
        {
            if (i > 0)
                sb.Append('+');

            Segment s = segments[i];

            // Add name.
            sb.Append(s.name);

            // Add generic arguments.
            if (s.genericArgs.Length > 0)
            {
                sb.Append('<');
                for (int g = 0; g < s.genericArgs.Length; g++)
                {
                    if (g > 0)
                        sb.Append(',');
                    sb.Append(s.genericArgs[g]);
                }
                sb.Append('>');
            }
        }

        return sb.ToString();
    }

    /* Conversion operators. */
    public static implicit operator string(FullTypeName t) => t.ToString();

    public static implicit operator TypeName(FullTypeName typeName) => (string)typeName;
}