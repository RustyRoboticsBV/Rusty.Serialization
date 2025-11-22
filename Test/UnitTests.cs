#if DEBUG
using System;

namespace Rusty.Serialization.Test;

/// <summary>
/// Unit testing utility class.
/// </summary>
public static class UnitTests
{
    /// <summary>
    /// Run a bunch of test cases for parsing values from text to nodes.
    /// </summary>
    public static void RunParserTests(bool verbose = false, bool numberSequences = false)
    {
        // Set flags.
        ParseTester.Verbose = verbose;
        ParseTester.Numeric = numberSequences;
        ParseTester.Successes = 0;
        ParseTester.Failures = 0;

        // Run tests.
        Console.WriteLine("[BOOLEANS]");
        ParseTester.TestBool(null, ParseTester.Throw);
        ParseTester.TestBool("", ParseTester.Throw);
        ParseTester.TestBool("true", true);
        ParseTester.TestBool("false", false);
        ParseTester.TestBool("  \tfalse \t ", false);
        ParseTester.TestBool("fa lse", ParseTester.Throw);

        Console.WriteLine();
        Console.WriteLine("[INTEGERS]");
        ParseTester.TestInt(null, ParseTester.Throw);
        ParseTester.TestInt("", ParseTester.Throw);
        ParseTester.TestInt("0", 0);
        ParseTester.TestInt("10", 10);
        ParseTester.TestInt("-20", -20);
        ParseTester.TestInt(" \t  5   \t", 5);
        ParseTester.TestInt("-", ParseTester.Throw);
        ParseTester.TestInt("5 5", ParseTester.Throw);
        ParseTester.TestInt("abc", ParseTester.Throw);
        ParseTester.TestInt("--5", ParseTester.Throw);
        ParseTester.TestInt("5-", ParseTester.Throw);

        Console.WriteLine();
        Console.WriteLine("[REALS]");
        ParseTester.TestReal(null, ParseTester.Throw);
        ParseTester.TestReal("", ParseTester.Throw);
        ParseTester.TestReal("1000.0", 1000.0m);
        ParseTester.TestReal("1.", 1.0m);
        ParseTester.TestReal(".5", 0.5m);
        ParseTester.TestReal(".", 0.0m);
        ParseTester.TestReal("-.", 0.0m);
        ParseTester.TestReal("-.5", -0.5m);
        ParseTester.TestReal("  \t  10.0  \t", 10.0m);
        ParseTester.TestReal("1", ParseTester.Throw);
        ParseTester.TestReal("-", ParseTester.Throw);
        ParseTester.TestReal("1000 .0", ParseTester.Throw);
        ParseTester.TestReal("0..5", ParseTester.Throw);
        ParseTester.TestReal("abc", ParseTester.Throw);

        Console.WriteLine();
        Console.WriteLine("[CHARACTERS]");
        ParseTester.TestChar(null, ParseTester.Throw);
        ParseTester.TestChar("", ParseTester.Throw);
        ParseTester.TestChar("''", ParseTester.Throw);
        ParseTester.TestChar("'A'", 'A');
        ParseTester.TestChar("'''", '\'');
        ParseTester.TestChar("'\\''", '\'');
        ParseTester.TestChar("'\"'", '"');
        ParseTester.TestChar("'\\\"'", '"');
        ParseTester.TestChar("'\\'", '\\');
        ParseTester.TestChar("'\\\\'", '\\');
        ParseTester.TestChar("'\\t'", '\t');
        ParseTester.TestChar("'\\n'", '\n');
        ParseTester.TestChar("'\\0'", '\0');
        ParseTester.TestChar("'\\[21FF]'", '\u21ff');
        ParseTester.TestChar($"'ÿ'", 'ÿ');
        ParseTester.TestChar("'\t'", ParseTester.Throw);
        ParseTester.TestChar("'\n'", ParseTester.Throw);
        ParseTester.TestChar("'\0'", ParseTester.Throw);
        ParseTester.TestChar("'\u21ff'", ParseTester.Throw);
        ParseTester.TestChar($"'{(char)2}'", ParseTester.Throw);
        ParseTester.TestChar($"'{(char)127}'", ParseTester.Throw);

        Console.WriteLine();
        Console.WriteLine("[STRINGS]");
        ParseTester.TestString(null, ParseTester.Throw);
        ParseTester.TestString("", ParseTester.Throw);
        ParseTester.TestString("\"\"", "");
        ParseTester.TestString("\"ABC\"", "ABC");
        ParseTester.TestString("  \n  \"ABC\" \t\t  ", "ABC");
        ParseTester.TestString("\"'\'\"", "''");
        ParseTester.TestString("\" \\\\ \"", " \\ ");
        ParseTester.TestString("\" \\\" \"", " \" ");
        ParseTester.TestString("\" \\t \"", " \t ");
        ParseTester.TestString("\" \\n \"", " \n ");
        ParseTester.TestString("\" \\0 \"", " \0 ");
        ParseTester.TestString("\" \\[21ff] \"", " \u21ff ");
        ParseTester.TestString("\" \\\\ \\\" \\t \\n \\0 \\[21ff] \"", " \\ \" \t \n \0 \u21ff ");
        ParseTester.TestString("\"áèîõüÿ\"", "áèîõüÿ");
        ParseTester.TestString("ABC", ParseTester.Throw);
        ParseTester.TestString("\"\"\"", ParseTester.Throw);
        ParseTester.TestString("\" \\[21ff \"", ParseTester.Throw);
        ParseTester.TestString("\" \\[21ff\t\t] \"", ParseTester.Throw);
        ParseTester.TestString("\" \\ \"", ParseTester.Throw);
        ParseTester.TestString("\" \" \"", ParseTester.Throw);
        ParseTester.TestString("\" \t \"", ParseTester.Throw);
        ParseTester.TestString("\" \n \"", ParseTester.Throw);
        ParseTester.TestString("\" \0 \"", ParseTester.Throw);
        ParseTester.TestString("\" \u21ff \"", ParseTester.Throw);

        Console.WriteLine();
        Console.WriteLine("[COLORS]");
        ParseTester.TestColor(null, ParseTester.Throw);
        ParseTester.TestColor("", ParseTester.Throw);
        ParseTester.TestColor("#F00", (255, 0, 0, 255));
        ParseTester.TestColor("#F000", (255, 0, 0, 0));
        ParseTester.TestColor("#FF0000", (255, 0, 0, 255));
        ParseTester.TestColor("#FF000000", (255, 0, 0, 0));
        ParseTester.TestColor("#FF0000", (255, 0, 0, 255));
        ParseTester.TestColor("  \t  #FF000000 \t\n", (255, 0, 0, 0));
        ParseTester.TestColor("FFFFFF", ParseTester.Throw);
        ParseTester.TestColor("# FFFFFF", ParseTester.Throw);
        ParseTester.TestColor("#FF", ParseTester.Throw);
        ParseTester.TestColor("#YYYYYY", ParseTester.Throw);

        Console.WriteLine();
        Console.WriteLine("[BINARY]");
        ParseTester.TestBinary(null, ParseTester.Throw);
        ParseTester.TestBinary("", ParseTester.Throw);
        ParseTester.TestBinary("0x", new byte[0]);
        ParseTester.TestBinary("0x0123456789ABCDEF", new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF});
        ParseTester.TestBinary("0xaBcDeF", new byte[] { 0xAB, 0xCD, 0xEF });
        ParseTester.TestBinary("     \n\t0x0123456789ABCDEF \t\t", new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF});
        ParseTester.TestBinary("0x0123456789ABCDE", ParseTester.Throw);
        ParseTester.TestBinary("0x 123456789ABCDEF", ParseTester.Throw);

        Console.WriteLine();
        Console.WriteLine("[NULL]");
        ParseTester.TestNull(null, ParseTester.Throw);
        ParseTester.TestNull("", ParseTester.Throw);
        ParseTester.TestNull("null", new(null));
        ParseTester.TestNull("  \n\t null  \t", new(null));
        ParseTester.TestNull("n u l l ", ParseTester.Throw);
        ParseTester.TestNull("NULL", ParseTester.Throw);
        ParseTester.TestNull("abc", ParseTester.Throw);

        Console.WriteLine();
        Console.WriteLine("[TYPE LABELS]");
        ParseTester.TestType(null, ParseTester.Throw);
        ParseTester.TestType("", ParseTester.Throw);
        ParseTester.TestType("(i32)", (TypeName)"i32");
        ParseTester.TestType("(i32).", (TypeName)"i32");
        ParseTester.TestType("(  dict<i32,c>[,][])", (TypeName)"dict<i32,c>[,][]");
        ParseTester.TestType("( \t \n i32   \t )", (TypeName)"i32");
        ParseTester.TestType("( ( )", ParseTester.Throw);
        ParseTester.TestType("( ) )", ParseTester.Throw);

        Console.WriteLine();
        Console.WriteLine("Succesful tests: " + ParseTester.Successes + " out of " + ParseTester.Total);
        Console.WriteLine("Failed tests: " + ParseTester.Failures + " out of " + ParseTester.Total);
    }

    /// <summary>
    /// Run a bunch of test cases for serializing values to text.
    /// </summary>
    public static void RunSerializeTests()
    {
        Console.WriteLine("[BOOLEANS]");
        SerializeTester.TestBool(true, "true");
        SerializeTester.TestBool(false, "false");

        Console.WriteLine();
        Console.WriteLine("[INTEGERS]");
        SerializeTester.TestInt(-10, "-10");
        SerializeTester.TestInt(10, "10");

        Console.WriteLine();
        Console.WriteLine("[REALS]");
        SerializeTester.TestReal(10.0m, "10.");
        SerializeTester.TestReal(0.5m, ".5");
        SerializeTester.TestReal(-0.005m, "-.005");
        SerializeTester.TestReal(0, ".");

        Console.WriteLine();
        Console.WriteLine("[CHARACTERS]");
        SerializeTester.TestChar('A', "'A'");
        SerializeTester.TestChar('\\', "'\\'");
        SerializeTester.TestChar('\'', "'''");
        SerializeTester.TestChar('"', "'\"'");
        SerializeTester.TestChar('\t', "'\\t'");
        SerializeTester.TestChar('\n', "'\\n'");
        SerializeTester.TestChar('\0', "'\\0'");
        SerializeTester.TestChar('\u1234', "'\\[1234]'");
        SerializeTester.TestChar('ÿ', "'ÿ'");

        Console.WriteLine();
        Console.WriteLine("[STRINGS]");
        SerializeTester.TestString("A", "\"A\"");
        SerializeTester.TestString("\\", "\"\\\\\"");
        SerializeTester.TestString("'", "\"'\"");
        SerializeTester.TestString("\"", "\"\\\"\"");
        SerializeTester.TestString("\t", "\"\\t\"");
        SerializeTester.TestString("\n", "\"\\n\"");
        SerializeTester.TestString("\0", "\"\\0\"");
        SerializeTester.TestString("\u1234", "\"\\[1234]\"");
        SerializeTester.TestString("áèîõüÿ", "\"áèîõüÿ\"");

        Console.WriteLine();
        Console.WriteLine("[COLORS]");
        SerializeTester.TestColor((255, 0, 0, 255), "#F00");
        SerializeTester.TestColor((255, 0, 0, 0), "#F000");
        SerializeTester.TestColor((128, 0, 0, 255), "#800000");
        SerializeTester.TestColor((128, 0, 0, 0), "#80000000");

        Console.WriteLine();
        Console.WriteLine("[BINARY]");
        SerializeTester.TestBinary([], "0x");
        SerializeTester.TestBinary([0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF], "0x0123456789ABCDEF");

        Console.WriteLine();
        Console.WriteLine("[NULL]");
        SerializeTester.TestNull("null");
    }
}
#endif