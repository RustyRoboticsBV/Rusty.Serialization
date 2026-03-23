namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A string converter.
    /// </summary>
    public sealed class StringConverter : TypedStringConverter<string>
    {
        /* Protected methods. */
        protected override string ToString(string obj) => obj;
        protected override string FromString(string str) => str;
    }
}