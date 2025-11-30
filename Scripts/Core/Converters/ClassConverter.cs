using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic class converter.
    /// </summary>
    public class ClassConverter<T> : ObjectConverter<T>
        where T : class, new()
    {
        /* Protected methods. */
        public override INode Convert(T obj, IConverterScheme scheme)
        {
            // Handle null.
            if (obj == null)
                return new NullNode();

            // Handle non-null.
            return base.Convert(obj, scheme);
        }

        public override T Deconvert(INode node, IConverterScheme scheme)
        {
            if (node is NullNode)
                return null;

            // Handle non-null.
            return base.Deconvert(node, scheme);
        }
    }
}