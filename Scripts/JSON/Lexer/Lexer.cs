using Rusty.Serialization.Core.Lexer;

namespace Rusty.Serialization.Json.Lexer
{
    public class Lexer : Lexer<TokenType>
    {
        /* Public methods. */
        public override bool GetNextToken(ref TextCursor cursor, out Token<TokenType> token)
        {
            cursor.Advance();
            token = new Token<TokenType>(default, cursor.Position - 1, 1);
            return true;
        } // TODO: replace placeholder code and implement.
    }
}
