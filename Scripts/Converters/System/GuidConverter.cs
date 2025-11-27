using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A System.Guid converter.
/// </summary>
public sealed class GuidConverter : ValueConverter<Guid, BinaryNode>
{
    /* Protected methods. */
    protected override BinaryNode Convert(Guid obj, Context context) => new(obj.ToByteArray());
    protected override Guid Deconvert(BinaryNode node, Context context) => new(node.Value);
}