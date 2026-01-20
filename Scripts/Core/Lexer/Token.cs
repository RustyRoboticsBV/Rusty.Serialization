namespace Rusty.Serialization.Core.Lexing
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
        public static implicit operator TextSpan(Token token) => token.Text;

        /* Public methods. */
        public override string ToString() => new string(Text);
    }
}