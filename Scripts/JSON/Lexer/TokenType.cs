namespace Rusty.Serialization.Json.Lexer
{
    /// <summary>
    /// A type of JSON lexer token.
    /// </summary>
    public enum TokenType
    {
        None,
        Null,
        True,
        False,
        Number,
        String,
        ObjectStart,
        ObjectEnd,
        ArrayStart,
        ArrayEnd,
        Colon,
        Comma
    }
}