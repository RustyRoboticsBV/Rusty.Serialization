using System;
using System.Reflection;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A delegate converter.
    /// </summary>
    public sealed class DelegateConverter<ValueT> : Converter<ValueT, CallableNode>
        where ValueT : Delegate
    {
        /* Protected methods. */
        protected override CallableNode CreateNode(ValueT obj, CreateNodeContext context)
        {
            IMemberNameNode name;
            if (obj.Method.IsHideBySig)
            {
                SymbolNode nameSymbol = new SymbolNode(obj.Method.Name);
                name = new ScopeNode(new TypeName(obj.Method.DeclaringType), nameSymbol);
            }
            else
                name = new SymbolNode(obj.Method.Name);

            if (obj.Target != null)
            {
                INode target = context.CreateNode(obj.Target);
                TypeNode typedTarget = new TypeNode(new TypeName(obj.Target.GetType()), target);
                return new CallableNode(typedTarget, name);
            }
            else
                return new CallableNode(name);
        }

        protected override ValueT CreateObject(CallableNode node, CreateObjectContext context)
        {
            // Get type of delegate.
            Type delegateType = typeof(ValueT);

            // Resolve target (can be null for static methods).
            object target = null;
            if (node.Target != null)
            {
                if (node.Target is TypeNode targetTypeNode)
                {
                    Type targetType = new TypeName(targetTypeNode.Name);
                    target = context.CreateObject(targetType, node.Target);
                }
                else
                    throw new FormatException("Cannot resolve callable nodes without annotating the target with a type label.");
            }

            // Get method.
            IMemberNameNode nameNode = node.Name;
            string methodName = nameNode.Name;

            MethodInfo method = null;

            if (target != null)
            {
                // Instance method
                Type targetType = target.GetType();
                MethodInfo[] methods = targetType.GetMethods(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                );

                for (int i = 0; i < methods.Length; i++)
                {
                    if (methods[i].Name == methodName)
                    {
                        method = methods[i];
                        break;
                    }
                }
            }
            else
            {
                // Static method, look on delegate's declaring type
                Type declaringType = delegateType.DeclaringType;

                if (declaringType != null)
                {
                    MethodInfo[] methods = declaringType.GetMethods(
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                    );

                    for (int i = 0; i < methods.Length; i++)
                    {
                        if (methods[i].Name == methodName)
                        {
                            method = methods[i];
                            break;
                        }
                    }
                }
            }

            if (method == null)
            {
                throw new InvalidOperationException("Could not resolve method for delegate.");
            }

            Delegate del = Delegate.CreateDelegate(delegateType, target, method);

            return (ValueT)del;
        }
    }
}