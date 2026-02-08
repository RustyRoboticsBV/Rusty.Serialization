namespace Rusty.Serialization.Core.Codecs
{
    /// <summary>
    /// A token, representing a substring from a string of serialized data.
    /// </summary>
    public readonly ref struct Token
    {
        /* Fields. */
        /// <summary>
        /// The source text that this token is a substring of.
        /// </summary>
        public readonly TextSpan SourceText;
        /// <summary>
        /// The underlying lexeme that this token represents.
        /// </summary>
        public readonly Lexeme Lexeme;

        /* Public properties. */
        /// <summary>
        /// Construct an EOF token.
        /// </summary>
        public static Token EOF => new Token();

        /// <summary>
        /// Whether or not this token represents the end of the file.
        /// </summary>
        public bool IsEOF => Lexeme.Start >= SourceText.Length;
        /// <summary>
        /// The length the token in the source text.
        /// </summary>
        public int Length => Lexeme.Length;
        /// <summary>
        /// The text that this token represents.
        /// </summary>
        public TextSpan Text => Lexeme.ExtractFrom(SourceText);

        /* Constructors. */
        public Token(TextSpan sourceText, Lexeme lexeme)
        {
            SourceText = sourceText;
            Lexeme = lexeme;
        }

        /* Conversion operators. */
        public static explicit operator TextSpan(Token token) => token.Text;

        /* Public methods. */
        public override string ToString() => new string(Text);

        /// <summary>
        /// Get the text between enclosing delimiters.
        /// </summary>
        public TextSpan Unpack(int prefixLength, int suffixLength)
        {
            return Text.Slice(prefixLength, Text.Length - prefixLength - suffixLength);
        }
    }
}