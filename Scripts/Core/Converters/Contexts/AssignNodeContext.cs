using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A context for the IConverter.AssignNode method.
    /// </summary>
    public class AssignNodeContext : SubContext
    {
        /* Private properties. */
        private CreateNodeContext CreateNodeContext => Context.CreateNodeContext;

        /* Constructors. */
        public AssignNodeContext(Converters context) : base(context) { }

        /* Public methods. */
        /// <summary>
        /// Create a node from an object of some type.
        /// </summary>
        public INode CreateNode(object obj) => CreateNode(obj?.GetType(), obj);

        /// <summary>
        /// Create a node from an object of some type.
        /// </summary>
        public INode CreateNode(Type declaredType, object obj)
        {
            Type actualType = obj?.GetType();
            INode targetNode;

            // Handle null.
            if (obj == null)
                return new NullNode();

            // Create reference if this object was in the symbol table.
            bool isReferenceType = !actualType.IsValueType;
            if (isReferenceType && SymbolTable.HasObject(obj))
            {
                // Create address if it didn't exist yet.
                if (!SymbolTable.HasAddressFor(obj))
                {
                    string address = SymbolTable.CreateAddressFor(obj).ToString();
                    targetNode = SymbolTable.GetNode(obj);
                    ITreeElement targetParent = targetNode.Parent;
                    AddressNode addressNode = new AddressNode(address, targetNode);
                    if (targetParent is IContainerNode container)
                        container.ReplaceChild(targetNode, addressNode);
                }

                // Create reference.
                return new RefNode(SymbolTable.GetAddressFor(obj).ToString());
            }

            // Convert.
            IConverter converter = Converters.Get(actualType);
            targetNode = converter.CreateNode(obj, CreateNodeContext);
            INode rootNode = targetNode;

            // Wrap in type node if the types don't match.
            if (declaredType != actualType)
                rootNode = new TypeNode(new TypeName(actualType), targetNode);

            // Register node if the object was a reference type.
            if (isReferenceType)
            {
                SymbolTable.Add(obj);
                SymbolTable.SetNode(obj, targetNode);
            }

            // Assign node if the object was a composite type.
            if (converter is ICompositeConverter referenceConverter)
                referenceConverter.AssignNode(targetNode, obj, this);

            // Return finished node.
            return rootNode;
        }
    }
}