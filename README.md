# Compact C# Data
A C# serialization/deserialization module, with support for all built-in C# types and all built-in Unity and Godot structs.

The module uses a custom serialization format, called Compact C# Data (CSCD). It's designed to be compact, general and relatively easy to parse while preserving a somewhat readable structure.

## Data format Specification
The format describes nested data structures.

The data may only contain characters from a subset of the ASCII table:
- Characters from 0x20 (space) to 0x7E (~).
- Other whitespace characters: 0x09 (horizontal tab), 0x0A (newline), 0x0B (vertical tab), 0x0C (form feed) and 0x0D (carriage return).

When hexadecimal numbers are mentioned, their values may use both upper-case and lower-case characters.

Whitespaces are allowed between tokens, but have no meaning.

Two types of values are supported: primitives and collections.

### Type Labels
Type labels can placed before a value to give a deserializer more information about how to deserialize a value. Type labels must be enclosed with `()` parentheses and may not contain whitespace. The top-level element is always required to have a type label, unless it is `null`.

### Primitives

#### Bool
Booleans can be one of two literals: `true` or `false`. Boolean values must be lowercase.

#### Int
Integers can be any combination of digits. Optionally, they may start with a negative sign.

#### Reals
Real numbers must contain a decimal point. Optionally, they may starts with a negative sign. If the integer part and/or the fractional part are equal to 0, they may be omitted. So `0.0`, `0.`, `.0` and `.` are all valid representations of the number `0.0`.

#### Characters
Characters must be enclosed in single-quote characters. Only a single character may be stored inside. Empty character literals are not allowed. Only characters from 0x20 (space) to 0x7E (tilde) are allowed.

A few special character literals exist:
- `'\''`: alternative way of writing `'''`.
- `'\"'`: alternative way of writing `'"'`.
- `'\\'`: alternative way of writing `'\'`.
- `'\t'`: a tab.
- `'\n'`: a newline.
- `'\0'`: a null character.
- `'\[#]'`: an unicode character. # must be a valid hexadecimal number.

#### Strings
Strings must be enclosed in double-quote characters. Empty strings are allowed. The same special character codes are used, however using unescaped double-quotes and backslashes is NOT allowed like in character literals, you MUST use `\"` and `\\` to represent them. Like with characters, only characters from 0x20 (space) to 0x7E (tilde) are allowed.

#### Colors
Colors literals must start with a hex sign, followed by the hexadecimal representation of the color. Four conventions are available:
- `#RGB`: short notation, corresponds to the same color as `#RRGGBB`.
- `#RGBA`: short notation with alpha, corresponds to the same color as `#RRGGBBAA`.
- `#RRGGBB`: full notation without alpha. The alpha is assumed to be `FF`.
- `#RRGGBBAA`: full notation with alpha.

#### Binary
Binary data strings store arbitrary data in hexadecimal format. They start with `0x`, followed by the hexadecimal representation of the data. The literal `0x` represents a binary data string of length 0.

#### Null
Null values are encoded with `null` literals. Like booleans, null values must be lower-case.

### Collections

#### Lists
Lists are collections of values. They are comma-delimited and enclosed with `[]` square brackets. Lists may have values of multiple types. Empty elements are not allowed. Example: `[1,'2',"3"]`.

#### Dictionaries
Dictionaries are collections of key-value pairs. They are comma-delimited and enclosed with `{}` curly braces. Keys and values are seprated by `:` colons. Both keys and values may be of any type, including other collections, and dictionaries may contain keys and values of multiple types. Example: `{"a":"abc",'b':"def",["c"]:"hij"}`.

#### Objects
Objects represent a collection of identifier-value pairs. They are comma-delimited and are enclosed with `<>` angular brackets. Identifiers are unquoted strings that may only consist of letters, digits and `_` underscores, and may not start with a number. Example: `<my_int:0,my_real:0.0,my_char:'A'>`.

Objects are similar to dictionaries, and are essentually a more compact but also more strict syntax, used to serialize classes and structs.