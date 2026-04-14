using System;
using System.Collections.Generic;
using System.Reflection;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A generic list converter.
    /// </summary>
    public class ListConverter<ListT, ElementT> : Converter
        where ListT : IList<ElementT>
    {
        /* Public methods. */
        public override INode CreateNode(object obj, CreateNodeContext context) => new ListNode();

        /* Protected methods. */
        protected sealed override void CollectChildNodeTypes(ListNode node, CollectTypesContext context)
        {
            for (int i = 0; i < node.Count; i++)
            {
                context.Collect(node.GetValueAt(i), typeof(ElementT));
            }
        }

        protected sealed override object CreateObject(ListNode node, CreateObjectContext context) => CreateList();

        protected sealed override object PopulateObject(ListNode node, object obj, PopulateObjectContext context)
        {
            ListT list = (ListT)obj;
            for (int i = 0; i < node.Count; i++)
            {
                list.Add((ElementT)context.CreateChildObject(node.GetValueAt(i), typeof(ElementT)));
            }
            return obj;
        }

        protected virtual ListT CreateList()
        {
            Type type = typeof(ListT);
            ConstructorInfo[] constructors = type.GetConstructors();
            for (int i = 0; i < constructors.Length; i++)
            {
                ConstructorInfo constructor = constructors[i];

                ParameterInfo[] parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                    return (ListT)constructor.Invoke(null);
                else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
                    return (ListT)constructor.Invoke(new object[1] { 0 });
            }

            throw new MissingMethodException($"The type '{type.Name}' does not have a parameterless constructor or a constructor "
                + "with a single int constructor.");
        }
    }
}