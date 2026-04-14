using System;
using System.Collections.Generic;
using System.Reflection;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic dictionary converter.
    /// </summary>
    public class DictionaryConverter<DictT, KeyT, ValueT> : Converter
        where DictT : IDictionary<KeyT, ValueT>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new DictNode();

        /* Protected methods. */
        protected sealed override void CollectChildNodeTypes(DictNode node, CollectTypesContext context)
        {
            for (int i = 0; i < node.Count; i++)
            {
                context.Collect(node.GetKeyAt(i), typeof(KeyT));
                context.Collect(node.GetValueAt(i), typeof(ValueT));
            }
        }

        protected sealed override object CreateObject(DictNode node, CreateObjectContext context) => CreateDict();

        protected sealed override object PopulateObject(DictNode node, object obj, PopulateObjectContext context)
        {
            DictT dict = (DictT)obj;
            for (int i = 0; i < node.Count; i++)
            {
                KeyT key = (KeyT)context.CreateChildObject(node.GetKeyAt(i), typeof(KeyT));
                ValueT value = (ValueT)context.CreateChildObject(node.GetValueAt(i), typeof(ValueT));
                dict.Add(key, value);
            }
            return obj;
        }

        protected virtual DictT CreateDict()
        {
            Type type = typeof(DictT);
            ConstructorInfo[] constructors = type.GetConstructors();
            for (int i = 0; i < constructors.Length; i++)
            {
                ConstructorInfo constructor = constructors[i];

                ParameterInfo[] parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                    return (DictT)constructor.Invoke(null);
                else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
                    return (DictT)constructor.Invoke(new object[1] { 0 });
            }

            throw new MissingMethodException($"The type '{type.Name}' does not have a parameterless constructor or a constructor "
                + "with a single int constructor.");
        }
    }
}