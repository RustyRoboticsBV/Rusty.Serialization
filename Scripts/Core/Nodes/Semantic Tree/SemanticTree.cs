using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A container for a syntax tree that performs semantic analysis, builds an address table and builds a limited type table
    /// for nodes that have been wrapped in a type node.
    /// 
    /// The input tree is assumed to be free of cycles, and each node in the tree is assumed to appear only once.
    /// </summary>
    public class SemanticTree
    {
        /* Private properties. */
        private SyntaxTree SyntaxTree { get; set; }
        private Dictionary<string, AddressNode> AddressTable { get; set; }
        private Dictionary<INode, string> TypeTable { get; set; }
        private List<RefNode> References { get; set; }

        /* Public properties. */
        public INode Root => SyntaxTree.Root;

        /* Constructors. */
        public SemanticTree(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
            AddressTable = new Dictionary<string, AddressNode>();
            TypeTable = new Dictionary<INode, string>();
            References = new List<RefNode>();

            // Analyze root.
            if (SyntaxTree == null)
                SemanticError("The syntax tree was null.");
            if (Root == null)
                SemanticError("The root node may not be null.");
            Analyze(Root, false);

            // Check references and optionally assign them types.
            foreach (RefNode @ref in References)
            {
                // Validate the stated address exists.
                if (!AddressTable.ContainsKey(@ref.Address))
                    SemanticError(@ref, $"Target address '{@ref.Address}' does not exist.");

                // Propagate address' type to reference (unless the reference was already in the type table).
                AddressNode address = AddressTable[@ref.Address];
                if (TypeTable.TryGetValue(address, out string type))
                    TypeTable.TryAdd(@ref, type);
            }
        }

        /* Public methods. */
        /// <summary>
        /// Get the address node corresponding to a reference.
        /// </summary>
        public AddressNode GetAddress(RefNode reference) => AddressTable[reference.Address];

        /// <summary>
        /// Get the type that a node has been marked with. Returns null if the node was not marked with an explicit type.
        /// </summary>
        public string GetType(INode node)
        {
            if (TypeTable.TryGetValue(node, out string type))
                return type;
            return null;
        }

        /* Private methods. */
        private void Analyze(INode node, bool allowScopes)
        {
            // Metadata.
            if (node is AddressNode address)
                Analyze(address);

            else if (node is TypeNode type)
                Analyze(type);

            else if (node is ScopeNode scope)
            {
                if (!allowScopes)
                    SemanticError(node, "Scope nodes may only be used as object member names or callable names.");
                else
                    Analyze(scope);
            }

            else if (node is OffsetNode offset)
                Analyze(offset);

            // Collections.
            else if (node is ListNode list)
                Analyze(list);

            else if (node is DictNode dict)
                Analyze(dict);

            else if (node is ObjectNode obj)
                Analyze(obj);

            else if (node is CallableNode func)
                Analyze(func);

            // Primitives.
            else if (node is RefNode @ref)
                References.Add(@ref);

            else if (node is NullNode) { }
            else if (node is BoolNode) { }
            else if (node is BitmaskNode) { }
            else if (node is IntNode) { }
            else if (node is FloatNode) { }
            else if (node is DecimalNode) { }
            else if (node is NanNode) { }
            else if (node is InfinityNode) { }
            else if (node is CharNode) { }
            else if (node is StringNode) { }
            else if (node is ColorNode) { }
            else if (node is UidNode) { }
            else if (node is TimestampNode) { }
            else if (node is DurationNode) { }
            else if (node is SymbolNode) { }
            else if (node is BytesNode) { }

            else
                SemanticError(node, "Unknown node type.");
        }

        private void Analyze(AddressNode node)
        {
            // Register address.
            if (AddressTable.ContainsKey(node.Name))
                SemanticError(node, $"Duplicate address '{node.Name}'.");
            AddressTable.Add(node.Name, node);

            // Analyze child.
            if (node.Child == null)
                SemanticError(node, "Child node was null.");
            if (node.Child is RefNode)
                SemanticError(node, $"Child may not be a reference node:\n{node.Child}");
            Analyze(node.Child, false);

            // If child is type, register type.
            if (node.Child is TypeNode type)
                TypeTable.TryAdd(node, type.Name);
        }

        private void Analyze(TypeNode node)
        {
            // Analyze child.
            if (node.Child == null)
                SemanticError(node, "Child node was null.");
            if (node.Child is AddressNode)
                SemanticError(node, $"Child node may not be an address node:\n{node.Child}.");
            if (node.Child is TypeNode)
                SemanticError(node, $"Child node may not be another type node:\n{node.Child}.");

            Analyze(node.Child, false);

            // Register type for type node and all child nodes until the first non-metadata node.
            INode current = node;
            while (current is IMetadataNode metadata)
            {
                TypeTable.TryAdd(current, node.Name);
                current = metadata.Child;
            }
            TypeTable.TryAdd(current, node.Name);
        }

        private void Analyze(ScopeNode node)
        {
            if (node.Child == null)
                SemanticError(node, "Child node was null.");
            if (node.Child is not SymbolNode)
                SemanticError(node, $"Child node must be a symbol node:\n{node.Child}.");

            Analyze(node.Child, false);
        }

        private void Analyze(OffsetNode node)
        {
            if (node.Child == null)
                SemanticError(node, "Child node was null.");
            if (node.Child is not TimestampNode)
                SemanticError(node, $"Child node must be a timestamp node:\n{node.Child}.");

            Analyze(node.Child, false);
        }

        private void Analyze(ListNode node)
        {
            for (int i = 0; i < node.Count; i++)
            {
                INode value = node.GetValueAt(i);
                if (value == null)
                    SemanticError(node, $"Value at index {i} was null.");
                Analyze(value, false);
            }
        }

        private void Analyze(DictNode node)
        {
            for (int i = 0; i < node.Count; i++)
            {
                INode key = node.GetKeyAt(i);
                if (key == null)
                    SemanticError(node, $"Key at index {i} was null.");
                Analyze(key, false);

                INode value = node.GetValueAt(i);
                if (value == null)
                    SemanticError(node, $"Value at index {i} was null.");
                Analyze(value, false);
            }
        }

        private void Analyze(ObjectNode node)
        {
            for (int i = 0; i < node.Count; i++)
            {
                INode key = node.GetNameAt(i);
                if (key == null)
                    SemanticError(node, $"Key at index {i} was null.");
                if (key is not ScopeNode && key is not SymbolNode)
                    SemanticError(node, $"Key must be a scope node or symbol node:\n{key}.");
                Analyze(key, true);

                INode value = node.GetValueAt(i);
                if (value == null)
                    SemanticError(node, $"Value at index {i} was null.");
                Analyze(value, false);
            }
        }

        private void Analyze(CallableNode node)
        {
            if (node.Target != null)
                Analyze(node.Target, false);

            if (node.Name == null)
                SemanticError(node, $"Name was null.");
            if (node.Name is not ScopeNode && node.Name is not SymbolNode)
                SemanticError(node, $"Callable name must be a scope or symbol node:\n{node.Name}.");
            Analyze(node.Name, true);
        }

        private static void SemanticError(string message)
        {
            throw new FormatException($"Semantic error:\n{message}");
        }

        private static void SemanticError(INode node, string message)
        {
            throw new FormatException($"Semantic error at {GetTypeName(node)} node:\n{node}\n\n{message}");
        }

        private static string GetTypeName(INode node)
        {
            switch (node)
            {
                case AddressNode: return "address";
                case TypeNode: return "type";
                case ScopeNode: return "scope";
                case OffsetNode: return "offset";
                case ListNode: return "list";
                case DictNode: return "dictionary";
                case ObjectNode: return "object";
                case CallableNode: return "callable";
                case NullNode: return "null";
                case BoolNode: return "bool";
                case BitmaskNode: return "bitmask";
                case IntNode: return "int";
                case FloatNode: return "float";
                case DecimalNode: return "decimal";
                case NanNode: return "nan";
                case InfinityNode: return "infinity";
                case CharNode: return "char";
                case StringNode: return "string";
                case ColorNode: return "color";
                case UidNode: return "UID";
                case TimestampNode: return "timestamp";
                case DurationNode: return "duration";
                case SymbolNode: return "symbol";
                case BytesNode: return "bytes";
                case RefNode: return "reference";
                default: return "unknown";
            }
        }
    }
}