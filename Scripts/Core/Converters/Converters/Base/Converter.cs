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
        /* Public properties. */
        public Type[] AllowedNodeTypes { get; protected set; } = new Type[1] { typeof(NodeT) };

        /* Public methods. */
        /// <summary>
        /// Convert a node to a type that this converter can use.
        /// </summary>
        public INode ConvertNode(INode node)
        {
            if (node == null)
                return null;

            Type from = node.GetType();

            // If one of the allowed types, return as-is.
            for (int i = 0; i < AllowedNodeTypes.Length; i++)
            {
                if (from == AllowedNodeTypes[i])
                    return node;
            }

            // Else, check if there is a conversion.
            for (int i = 0; i < AllowedNodeTypes.Length; i++)
            {
                Type to = AllowedNodeTypes[i];

                // If no conversions needed, return the original node.
                if (to.IsAssignableFrom(from))
                    return (NodeT)node;

                // Check if there is a conversion.
                if (HasUserDefinedCast(from, to, out MethodInfo method))
                {
                    try
                    {
                        return (NodeT)method.Invoke(null, new object[] { node });
                    }
                    catch { }
                }
            }

            // No conversion available.
            throw new InvalidCastException($"The converter '{GetType().Name}' cannot handle node:\n{node}.");
        }

        public void CollectTypes(INode node, CollectTypesContext context)
        {
            NodeT castedNode;
            try
            {
                castedNode = (NodeT)node;
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Cannot cast node of type '{node.GetType()}' to node of type '{typeof(NodeT)}'.");
            }

            CollectTypes(castedNode, context);
        }
        INode IConverter.CreateNode(object obj, CreateNodeContext context) => CreateNode((TargetT)obj, context);
        object IConverter.CreateObject(INode node, CreateObjectContext context) => CreateObject((NodeT)node, context);

        /* Protected methods. */
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