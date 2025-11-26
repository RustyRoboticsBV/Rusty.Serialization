using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A char converter.
/// </summary>
public sealed class CharConverter : ValueConverter<char, CharNode>, IConverter
{
    public string TypeLabel => "chr";

    /* Protected methods. */
    protected override CharNode Convert(char obj, Context context) => new(obj);
    protected override char Deconvert(CharNode node, Context context) => node.Value;
}