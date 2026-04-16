using System;

namespace Rusty.Serialization.CSV
{
    public ref struct CsvLexer
    {
        /* Fields. */
        private ReadOnlySpan<char> text;
        private int cursor;

        /* Constructors. */
        public CsvLexer(string csv)
        {
            text = csv.AsSpan();
            cursor = 0;
        }

        /* Public methods. */
        public bool GetNextCell(out ReadOnlySpan<char> cell, out bool endOfLine)
        {
            endOfLine = false;
            cell = default;

            if (cursor >= text.Length)
                return false;

            int start = cursor;

            // Delimited cell.
            if (text[cursor] == '"')
            {
                int quoteStart = cursor;
                cursor++;

                while (cursor < text.Length)
                {
                    if (text[cursor] == '"')
                    {
                        // Skip escaped double-quote.
                        if (cursor + 1 < text.Length && text[cursor + 1] == '"')
                            cursor += 2;

                        // Detect end of quoted cell.
                        else
                        {
                            cursor++;
                            break;
                        }
                    }

                    cursor++;
                }

                cell = text.Slice(quoteStart, cursor - quoteStart);
            }

            // Bare cell.
            else
            {
                while (cursor < text.Length && text[cursor] != ',' && text[cursor] != '\r' && text[cursor] != '\n')
                {
                    cursor++;
                }

                cell = text.Slice(start, cursor - start);
            }

            // Skip comma.
            if (cursor < text.Length && text[cursor] == ',')
            {
                cursor++;
                return true;
            }

            // Skip line-breaks.
            if (cursor < text.Length)
            {
                if (text[cursor] == '\r')
                {
                    cursor++;
                    if (cursor < text.Length && text[cursor] == '\n')
                        cursor++;
                }
                else if (text[cursor] == '\n')
                {
                    cursor++;
                }

                endOfLine = true;
            }

            return true;
        }
    }
}