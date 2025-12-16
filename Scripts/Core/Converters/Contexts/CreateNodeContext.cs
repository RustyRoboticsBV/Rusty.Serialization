using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    public class CreateNodeContext
    {
        /* Public properties. */
        public TypeRegistry ConverterTypes { get; private set; }
        public InstanceRegistry ConverterInstances { get; private set; }
        public SymbolTable SymbolTable { get; private set; }
        public AssignNodeContext AssignNodeContext { get; internal set; } // TODO: internal setter is bad.

        /* Constructors. */
        public CreateNodeContext(TypeRegistry converterTypes, InstanceRegistry instanceTypes, SymbolTable symbolTable,
            AssignNodeContext assignNodeContext)
        {
            ConverterTypes = converterTypes;
            ConverterInstances = instanceTypes;
            SymbolTable = symbolTable;
            AssignNodeContext = assignNodeContext;
        }

        /* Public methods. */
        /// <summary>
        /// Create a node from an object of some type.
        /// </summary>
        public INode CreateNode(object obj) => CreateNode(obj.GetType(), obj);

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
                // Create ID if it didn't exist yet.
                if (!SymbolTable.HasIdFor(obj))
                {
                    string id = SymbolTable.CreateIdFor(obj).ToString();
                    targetNode = SymbolTable.GetNode(obj);
                    ITreeElement targetParent = targetNode.Parent;
                    IdNode idNode = new(id, targetNode);
                    if (targetParent is IContainerNode container)
                        container.ReplaceChild(targetNode, idNode);
                }

                // Create reference.
                return new RefNode(SymbolTable.GetIdFor(obj).ToString());
            }

            // Get converter.
            IConverter converter = ConverterInstances.Get(actualType);
            if (converter == null)
            {
                converter = ConverterTypes.Instantiate(actualType);
                ConverterInstances.Add(actualType, converter);
            }

            // Convert.
            targetNode = converter.CreateNode(obj, this);
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
                referenceConverter.AssignNode(targetNode, obj, AssignNodeContext);

            // Return finished node.
            return rootNode;
        }
    }
}