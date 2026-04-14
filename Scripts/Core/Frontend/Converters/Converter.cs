using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An object to/from AST node converter.
    /// </summary>
    public abstract class Converter
    {
        /* Public methods. */

        // Conversion.

        public abstract INode CreateNode(object obj, CreateNodeContext context);

        public virtual void PopulateNode(object obj, INode node, PopulateNodeContext context) { }

        // Deconversion.

        public void CollectChildNodeTypes(INode node, CollectTypesContext context)
        {
            switch (node)
            {
                case ListNode li: CollectChildNodeTypes(li, context); break;
                case DictNode di: CollectChildNodeTypes(di, context); break;
                case ObjectNode ob: CollectChildNodeTypes(ob, context); break;
                case CallableNode ca: CollectChildNodeTypes(ca, context); break;
            }
        }

        public object CreateObject(INode node, CreateObjectContext context)
        {
            switch (node)
            {
                case ListNode l: return CreateObject(l, context);
                case DictNode d: return CreateObject(d, context);
                case ObjectNode o2: return CreateObject(o2, context);
                case CallableNode c: return CreateObject(c, context);

                case NullNode n: return CreateObject(n, context);
                case BoolNode b: return CreateObject(b, context);
                case BitmaskNode b2: return CreateObject(b2, context);
                case IntNode i: return CreateObject(i, context);
                case FloatNode f: return CreateObject(f, context);
                case DecimalNode d2: return CreateObject(d2, context);
                case InfinityNode i2: return CreateObject(i2, context);
                case NanNode n2: return CreateObject(n2, context);
                case CharNode c2: return CreateObject(c2, context);
                case StringNode s2: return CreateObject(s2, context);
                case ColorNode c3: return CreateObject(c3, context);
                case UidNode u: return CreateObject(u, context);
                case TimestampNode t: return CreateObject(t, context);
                case DurationNode d2: return CreateObject(d2, context);
                case BytesNode b3: return CreateObject(b3, context);
                case SymbolNode s2: return CreateObject(s2, context);

                default: throw InvalidNodeException(node);
            }
        }

        public object PopulateObject(INode node, object obj, PopulateObjectContext context)
        {
            switch (node)
            {
                case ListNode l: return PopulateObject(l, obj, context);
                case DictNode d: return PopulateObject(d, obj, context);
                case ObjectNode o2: return PopulateObject(o2, obj, context);
                case CallableNode c: return PopulateObject(c, obj, context);

                case NullNode n: return PopulateObject(n, obj, context);
                case BoolNode b: return PopulateObject(b, obj, context);
                case BitmaskNode b2: return PopulateObject(b2, obj, context);
                case IntNode i: return PopulateObject(i, obj, context);
                case FloatNode f: return PopulateObject(f, obj, context);
                case DecimalNode d2: return PopulateObject(d2, obj, context);
                case InfinityNode i2: return PopulateObject(i2, obj, context);
                case NanNode n2: return PopulateObject(n2, obj, context);
                case CharNode c2: return PopulateObject(c2, obj, context);
                case StringNode s2: return PopulateObject(s2, obj, context);
                case ColorNode c3: return PopulateObject(c3, obj, context);
                case UidNode u: return PopulateObject(u, obj, context);
                case TimestampNode t: return PopulateObject(t, obj, context);
                case DurationNode d2: return PopulateObject(d2, obj, context);
                case BytesNode b3: return PopulateObject(b3, obj, context);
                case SymbolNode s2: return PopulateObject(s2, obj, context);

                default: throw InvalidNodeException(node);
            }
        }

        /* Protected methods. */

        // Collect types.

        protected virtual void CollectChildNodeTypes(ListNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(DictNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(ObjectNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(CallableNode node, CollectTypesContext context) => throw InvalidNodeException(node);

        // Create object.

        protected virtual object CreateObject(ListNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(DictNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(ObjectNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(CallableNode node, CreateObjectContext context) => throw InvalidNodeException(node);

        protected virtual object CreateObject(NullNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(BoolNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(BitmaskNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(IntNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(FloatNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(DecimalNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(InfinityNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(NanNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(CharNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(StringNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(ColorNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(UidNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(TimestampNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(DurationNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(BytesNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(SymbolNode node, CreateObjectContext context) => throw InvalidNodeException(node);

        // Populate object.

        protected virtual object PopulateObject(ListNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(DictNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(ObjectNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(CallableNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);

        protected virtual object PopulateObject(NullNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(BoolNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(BitmaskNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(IntNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(FloatNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(DecimalNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(InfinityNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(NanNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(CharNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(StringNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(ColorNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(UidNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(TimestampNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(DurationNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(BytesNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(SymbolNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);

        // Misc.
        protected static T TryCast<T>(object obj)
        {
            try
            {
                return (T)obj;
            }
            catch
            {
                throw new InvalidCastException($"Cannot cast {obj.GetType().Name} to {typeof(T).Name}.");
            }
        }

        /* Private methods. */
        private NotImplementedException InvalidNodeException(INode node)
        {
            return new NotImplementedException($"{GetType().Name} cannot handle {node.GetType()}s.");
        }
    }
}