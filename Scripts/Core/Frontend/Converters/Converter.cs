using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    public sealed class CollectTypesContext
    {
        public void Collect(Type declaredType, object obj) { }
    }
    public sealed class CreateNodeContext { }
    public sealed class PopulateNodeContext { }
    public sealed class CreateObjectContext { }
    public sealed class PopulateObjectContext { }

    /// <summary>
    /// An object to/from AST node converter.
    /// </summary>
    public abstract class Converter
    {
        /* Public methods. */
        public virtual void CollectTypes(object obj, CollectTypesContext context) { }

        public abstract INode CreateNode(object obj, CreateNodeContext context);

        public virtual void PopulateNode(object obj, INode node, PopulateNodeContext context) { }

        public virtual object CreateObject(INode node, CreateObjectContext context)
        {
            switch (node)
            {
                case AddressNode a: return CreateObject(a, context);
                case TypeNode t: return CreateObject(t, context);
                case ScopeNode s: return CreateObject(s, context);
                case OffsetNode o: return CreateObject(o, context);

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
                case RefNode r: return CreateObject(r, context);

                default: throw InvalidNodeException(node);
            }
        }

        public virtual object PopulateObject(INode node, object obj, PopulateObjectContext context)
        {
            switch (node)
            {
                case AddressNode a: return PopulateObject(a, obj, context);
                case TypeNode t: return PopulateObject(t, obj, context);
                case ScopeNode s: return PopulateObject(s, obj, context);
                case OffsetNode o: return PopulateObject(o, obj, context);

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
                case RefNode r: return PopulateObject(r, obj, context);

                default: throw InvalidNodeException(node);
            }
        }

        /* Protected methods. */

        // Create objeCt.

        protected virtual object CreateObject(AddressNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(TypeNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(ScopeNode node, CreateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object CreateObject(OffsetNode node, CreateObjectContext context) => throw InvalidNodeException(node);

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
        protected virtual object CreateObject(RefNode node, CreateObjectContext context) => throw InvalidNodeException(node);

        // Populate object.

        protected virtual object PopulateObject(AddressNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(TypeNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(ScopeNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(OffsetNode node, PopulateObjectContext context) => throw InvalidNodeException(node);

        protected virtual object PopulateObject(ListNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(DictNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(ObjectNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(CallableNode node, PopulateObjectContext context) => throw InvalidNodeException(node);

        protected virtual object PopulateObject(NullNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(BoolNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(BitmaskNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(IntNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(FloatNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(DecimalNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(InfinityNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(NanNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(CharNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(StringNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(ColorNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(UidNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(TimestampNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(DurationNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(BytesNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(SymbolNode node, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(RefNode node, PopulateObjectContext context) => throw InvalidNodeException(node);

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
            throw new NotImplementedException($"{GetType().Name} cannot handle {node.GetType()}s.");
        }
    }

    //public abstract class Converter<T> : Converter
    //{
    //    /* Public methods. */
    //    public new T CreateObject(INode node)
    //    {
    //        object obj = base.CreateObject(node);
    //        try
    //        {
    //            return (T)obj;
    //        }
    //        catch
    //        {
    //            throw new InvalidCastException($"Cannot convert {obj.GetType().Name} to {typeof(T).Name}.");
    //        }
    //    }
    //
    //    public T PopulateObject(T obj, INode node)
    //    {
    //        object populated = base.PopulateObject(node, obj);
    //        try
    //        {
    //            return (T)populated;
    //        }
    //        catch
    //        {
    //            throw new InvalidCastException($"Cannot convert {populated.GetType().Name} to {typeof(T).Name}.");
    //        }
    //    }
    //}*/
}