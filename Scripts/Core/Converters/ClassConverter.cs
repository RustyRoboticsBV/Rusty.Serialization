using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic class converter.
    /// </summary>
    public sealed class ClassConverter<T> : ObjectConverter<T>
        where T : class, new()
    {
        /* Protected methods. */
        public override INode Convert(T obj, Context context)
        {
            // Handle null.
            if (obj == null)
                return new NullNode();

            // Handle non-null.
            return base.Convert(obj, context);
        }

        public override T Deconvert(INode node, Context context)
        {
            if (node is NullNode)
                return null;

            // Handle non-null.
            return base.Deconvert(node, context);
        }
    }
}