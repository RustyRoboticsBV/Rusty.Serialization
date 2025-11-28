using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A null converter.
    /// </summary>
    public sealed class NullConverter : ReferenceConverter<object, NullNode>
    {
        /* Protected methods. */
        protected override NullNode ConvertRef(object obj, IConverterScheme scheme) => new();
        protected override object DeconvertRef(NullNode node, IConverterScheme scheme) => null;
    }
}