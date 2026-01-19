namespace Rusty.Serialization.CSCD.Lexer
{
    /// <summary>
    /// A the CSCD token type.
    /// </summary>
    public enum TokenType
    {
        NONE,

        Word,

        Comma,
        Colon,

        ListStart,
        ListEnd,

        DictStart,
        DictEnd,

        ObjectStart,
        ObjectEnd
    }
}