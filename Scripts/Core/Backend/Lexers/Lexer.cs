using System;

namespace Rusty.Serialization.Core.Codecs
{
    /// <summary>
    /// A base class for lexers.
    /// </summary>
    public abstract class Lexer
    {
        /* Public properties. */
        /// <summary>
        /// The current cursor position of the lexer in the source text.
        /// </summary>
        public int Cursor { get; protected set; }

        /* Public methods. */
        /// <summary>
        /// Reset the cursor back to 0.
        /// </summary>
        public void ResetCursor() => Cursor = 0;
        /// <summary>
        /// Lexes the next token. Returns false if there are no tokens left.
        /// </summary>
        public abstract bool GetNextToken(TextSpan text, out Token token);

        /* Protected methods. */
        /// <summary>
        /// Check if the cursor is at the end of the text.
        /// </summary>
        protected bool IsAtEnd(TextSpan text) => Cursor >= text.Length;
        /// <summary>
        /// Get the character at the cursor position.
        /// </summary>
        protected char Current(TextSpan text) => text[Cursor];
        /// <summary>
        /// Get the character at the position after the cursor.
        /// </summary>
        protected char Next(TextSpan text) => text[Cursor + 1];
        /// <summary>
        /// Advance the cursor.
        /// </summary>
        protected void Advance(int count = 1)
        {
            if (count < 0)
                throw new ArgumentException("Cannot advance the cursor with negative counts.");
            Cursor += count;
        }

        /// <summary>
        /// Create a token starting at the current cursor position.
        /// </summary>
        protected Lexeme MakeLexeme(int length) => new Lexeme(Cursor, length);
    }
}