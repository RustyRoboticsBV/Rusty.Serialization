using Rusty.Serialization.Core.Lexer;

namespace Rusty.Serialization.Json.Lexer
{
    public class Lexer : Core.Lexer.Lexer
    {
        /* Public methods. */
        public override bool GetNextToken(TextSpan text, out Token token)
        {
            Advance();
            token = MakeToken(0);
            return true;
        } // TODO: replace placeholder code and implement.
    }
}
