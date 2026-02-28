using System;
using System.Reflection;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A base class for all converter.
    /// </summary>
    public abstract class Converter<TargetT, NodeT> : IConverter
        where NodeT : class, INode
    {
        /* Public methods. */
        public void CollectTypes(INode node, CollectTypesContext context)
        {
            node = ConvertNode(node);
            CollectTypes((NodeT)node, context);
        }

        INode IConverter.CreateNode(object obj, CreateNodeContext context) => CreateNode((TargetT)obj, context);

        object IConverter.CreateObject(INode node, CreateObjectContext context)
        {
            node = ConvertNode(node);
            return CreateObject((NodeT)node, context);
        }

        /* Protected methods. */
        protected virtual bool CanHandleNode(INode node) => typeof(NodeT).IsAssignableFrom(node.GetType());

        protected void NodeError(INode node)
        {
            throw new InvalidCastException($"The converter {GetType().FullName} cannot handle {node.GetType().Name} nodes\n{node}.");
        }

        /// <summary>
        /// Collect the type of a node, as well as the types of members.
        /// </summary>
        protected virtual void CollectTypes(NodeT node, CollectTypesContext context) { }
        /// <summary>
        /// Create a node from an object.
        /// </summary>
        protected abstract NodeT CreateNode(TargetT obj, CreateNodeContext context);
        /// <summary>
        /// Create an object from a node.
        /// </summary>
        protected abstract TargetT CreateObject(NodeT node, CreateObjectContext context);

        /* Private methods. */
        private NodeT ConvertNode(INode value)
        {
            if (value == null)
                return null;

            Type from = value.GetType();
            Type to = typeof(NodeT);

            // If no conversions needed, return the original node.
            if (to.IsAssignableFrom(from))
                return (NodeT)value;

            // Else, check if there is a conversion.
            if (HasUserDefinedCast(from, to, out MethodInfo method))
                return (NodeT)method.Invoke(null, new object[] { value });

            // No conversion available.
            throw new InvalidCastException($"The converter {GetType().FullName} cannot handle {value.GetType().Name} nodes\n{value}.");
        }

        private static bool HasUserDefinedCast(Type from, Type to, out MethodInfo method)
        {
            if (from == null)
                throw new ArgumentNullException();
            if (to == null)
                throw new ArgumentNullException();

            method = FindCast(from, from, to);
            if (method == null)
                method = FindCast(to, from, to);

            return method != null;
        }

        private static MethodInfo FindCast(Type typeToSearch, Type from, Type to)
        {
            MethodInfo[] methods = typeToSearch.GetMethods(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo method = methods[i];

                if (method.Name != "op_Implicit" && method.Name != "op_Explicit")
                    continue;

                if (method.ReturnType != to)
                    continue;

                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(from))
                    return method;
            }

            return null;
        }
    }
}