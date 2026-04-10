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
                case AddressNode ad: CollectChildNodeTypes(ad, context); break;
                case TypeNode ty: CollectChildNodeTypes(ty, context); break;
                case ScopeNode sc: CollectChildNodeTypes(sc, context); break;
                case OffsetNode of: CollectChildNodeTypes(of, context); break;

                case ListNode li: CollectChildNodeTypes(li, context); break;
                case DictNode di: CollectChildNodeTypes(di, context); break;
                case ObjectNode ob: CollectChildNodeTypes(ob, context); break;
                case CallableNode ca: CollectChildNodeTypes(ca, context); break;

                case NullNode nu: CollectChildNodeTypes(nu, context); break;
                case BoolNode bo: CollectChildNodeTypes(bo, context); break;
                case BitmaskNode bi: CollectChildNodeTypes(bi, context); break;
                case IntNode @int: CollectChildNodeTypes(@int, context); break;
                case FloatNode fl: CollectChildNodeTypes(fl, context); break;
                case DecimalNode de: CollectChildNodeTypes(de, context); break;
                case InfinityNode inf: CollectChildNodeTypes(inf, context); break;
                case NanNode na: CollectChildNodeTypes(na, context); break;
                case CharNode ch: CollectChildNodeTypes(ch, context); break;
                case StringNode st: CollectChildNodeTypes(st, context); break;
                case ColorNode co: CollectChildNodeTypes(co, context); break;
                case UidNode ui: CollectChildNodeTypes(ui, context); break;
                case TimestampNode ti: CollectChildNodeTypes(ti, context); break;
                case DurationNode du: CollectChildNodeTypes(du, context); break;
                case BytesNode by: CollectChildNodeTypes(by, context); break;
                case SymbolNode sy: CollectChildNodeTypes(sy, context); break;
                case RefNode re: CollectChildNodeTypes(re, context); break;

                default: throw InvalidNodeException(node);
            }
        }

        public object CreateObject(INode node, CreateObjectContext context)
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

        public object PopulateObject(INode node, object obj, PopulateObjectContext context)
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

        // Collect types.

        protected virtual void CollectChildNodeTypes(AddressNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(TypeNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(ScopeNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(OffsetNode node, CollectTypesContext context) => throw InvalidNodeException(node);

        protected virtual void CollectChildNodeTypes(ListNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(DictNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(ObjectNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(CallableNode node, CollectTypesContext context) => throw InvalidNodeException(node);

        protected virtual void CollectChildNodeTypes(NullNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(BoolNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(BitmaskNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(IntNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(FloatNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(DecimalNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(InfinityNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(NanNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(CharNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(StringNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(ColorNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(UidNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(TimestampNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(DurationNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(BytesNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(SymbolNode node, CollectTypesContext context) => throw InvalidNodeException(node);
        protected virtual void CollectChildNodeTypes(RefNode node, CollectTypesContext context) => throw InvalidNodeException(node);

        // Create object.

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

        protected virtual object PopulateObject(AddressNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(TypeNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(ScopeNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);
        protected virtual object PopulateObject(OffsetNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);

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
        protected virtual object PopulateObject(RefNode node, object obj, PopulateObjectContext context) => throw InvalidNodeException(node);

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