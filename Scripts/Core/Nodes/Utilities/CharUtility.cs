namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An utility for checking if a character is inside the allowed character set.
    /// </summary>
    internal static class CharUtility
    {
        /* Public methods. */
        /// <summary>
        /// Check if a character is in the allowed character set.
        /// </summary>
        public static bool Check(char chr)
        {
            return (chr >= ' ' && chr <= '~')
                || (chr >= '\t' && chr <= '\r')
                || (chr >= '\u00A1' && chr <= '\u00AC')
                || (chr >= '\u00AE' && chr <= '\u00FF');
        }
    }
}