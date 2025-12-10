namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A print utility for serializable node.
    /// </summary>
    internal static class PrintUtility
    {
        /* Pulblic methods. */
        public static string PrintNode(INode node)
        {
            return node?.ToString() ?? "(null)";
        }


        public static string PrintChild(INode child, bool isLast = true)
        {
            return PrintChild(PrintNode(child), isLast);
        }

        public static string PrintChild(string child, bool isLast = true)
        {
            if (isLast)
                return '\u2514' + (child?.Replace("\n", "\n ") ?? "(null)");
            else
                return '\u251C' + (child?.Replace("\n", "\n\u2502") ?? "(null)");
        }


        public static string PrintPair(INode key, INode value, bool isLast = true)
        {
            return PrintPair(PrintNode(key), PrintNode(value), isLast);
        }

        public static string PrintPair(string key, INode value, bool isLast = true)
        {
            return PrintPair(key, PrintNode(value), isLast);
        }

        public static string PrintPair(string key, string value, bool isLast = true)
        {
            string pair = "{"
                + "\n key: " + (key.Replace("\n", "\n ") ?? "(null)")
                + "\n val: " + (value.Replace("\n", "\n ") ?? "(null)")
                + "\n}";
            return PrintChild(pair, isLast);
        }
    }
}