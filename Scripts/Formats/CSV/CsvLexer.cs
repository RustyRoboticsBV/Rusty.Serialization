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
            /*bool quoted = false;

            if (text[cursor] == '"')
            {
                quoted = true;
                start = ++cursor;

                while (cursor < text.Length)
                {
                    if (text[cursor] == '"')
                    {
                        if (cursor + 1 < text.Length && text[cursor + 1] == '"')
                        {
                            cursor += 2;
                            continue;
                        }

                        break;
                    }

                    cursor++;
                }

                cell = text.Slice(start, cursor - start);
                cursor++;
            }
            else
            {
                while (cursor < text.Length &&
                       text[cursor] != ',' &&
                       text[cursor] != '\r' &&
                       text[cursor] != '\n')
                {
                    cursor++;
                }

                cell = text.Slice(start, cursor - start);
            }*/
            if (text[cursor] == '"')
            {
                int quoteStart = cursor; // include opening quote
                cursor++;                // move past opening quote

                while (cursor < text.Length)
                {
                    if (text[cursor] == '"')
                    {
                        if (cursor + 1 < text.Length && text[cursor + 1] == '"')
                        {
                            cursor += 2;
                            continue;
                        }

                        cursor++;
                        break;
                    }

                    cursor++;
                }

                cell = text.Slice(quoteStart, cursor - quoteStart);
            }
            else
            {
                while (cursor < text.Length &&
                       text[cursor] != ',' &&
                       text[cursor] != '\r' &&
                       text[cursor] != '\n')
                {
                    cursor++;
                }

                cell = text.Slice(start, cursor - start);
            }

            if (cursor < text.Length && text[cursor] == ',')
            {
                cursor++;
                return true;
            }

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