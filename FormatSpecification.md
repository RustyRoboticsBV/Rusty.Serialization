# Data format Specification
The CSCD format describes nested data structures.

## 1. General Formatting
### Character Set
Serialized data is expressed as a subset of the `ISO-8859-1` character set, and may only contain the following characters:
- ASCII whitespace characters: `0x20` (space), `0x09` (horizontal tab), `0x0A` (newline), `0x0B` (vertical tab), `0x0C` (form feed) and `0x0D` (carriage return).
- ASCII letters, digits and punctuation: `0x21` (`!`) to `0x7E` (`~`).
- Latin-1 Supplement letters, digits and punctuation: `0xA1` (`¡`) to `0xAC` (`¬`) and `0xAE` (`®`) to `0xFF` (`ÿ`).

### Hexadecimals
When hexadecimal numbers are mentioned, their values may use both upper-case and lower-case characters (in other words, they are not case sensitive).

### Whitespace
Whitespace is allowed between tokens for formatting reasons, but generally have no meaning.

## 2. Data Types
Two categories of values are supported: primitives and collections.

### 2.1. Type Labels
Type labels can placed before any value, providing information on how to deserialize it. They are written as a type name between `()` parentheses.

The top-level value is always required to have a type label, unless it is `null`. Type labels may not be followed by another type label, they must be followed by a value of some kind.

Type names may contain all characters from the allowed character set, except for parentheses and whitespace characters (whitespace characters between an outer parenthesis and the name itself are allowed, but have no meaning). They are case-sensitive.

Examples: `(int)`, `(dict<str,str>)`, `(my_object)` `(  my_namespace.my_class<int>.my_struct<list<f64>>[] )`.

### 2.2. Primitives

#### Booleans
Booleans can be one of two literals: `true` or `false`. Boolean values must be lowercase.

#### Integers
Integers can be any combination of digits. Optionally, they may start with a negative sign. Leading zeros are allowed.

Examples:
- `1` and `001` are both valid representations of the number `1`.
- `-50` and `-00050` are both valid representations of the number `-50`.

#### Reals
Real numbers must contain a decimal point. Optionally, they may starts with a negative sign. Other than that, they may only consist of digits. If the integer part and/or the fractional part are equal to 0, they may be omitted. Leading and trailing zeros are allowed.

Examples:
- `0.0`, `000.000`, `0.`, `.0` and `.` are all valid representations of the number `0.0`.
- `-0.0`, `-000.000`, `-0.`, `-.0` and `-.` are all valid representations of the number `-0.0`.
- `-0.5`, `-.5` and `-00.50` are all valid representations of the number `-0.5`.

#### Characters
Characters must be enclosed in `'` single-quotes. Only a single character may be stored inside. Empty character literals are not allowed.

The following characters from the character set may not appear in a character literal: `0x09` (horizontal tab), `0x0A` (newline), `0x0B` (vertical tab), `0x0C` (form feed) and `0x0D` (carriage return).

A few special character literals exist:
- `'\''`: alternative way of writing `'''`.
- `'\"'`: alternative way of writing `'"'`.
- `'\\'`: alternative way of writing `'\'`.
- `'\t'`: expresses a horizontal tab.
- `'\n'`: expresses a newline.
- `'\0'`: expresses a null character.
- `'\[#]'`: expresses a unicode character. # must be a valid hexadecimal number, and can be of any length greater than 0.

Examples: `'A'`, `'ç'`, `'''`, `'\n'`, `'\[21ff]'`.

#### Strings
Strings must be enclosed in `"` double-quotes. Empty strings are allowed. The same character set is used as for character literals. The same special character rules apply as well, except that using unescaped double-quotes and backslashes is NOT allowed -  you MUST use `\"` and `\\` to represent them.

Example: `"This is a \"string\"!"`, `¡No habló español!`, `\[21ff]\tarrow`, `C:\\path\\to\\file`.

#### Colors
Colors literals must start with a `#` hex sign, followed by the hexadecimal representation of the color. Four conventions are available:
- `#RGB`: short notation. Corresponds to the same color as `#RRGGBB`. Example: `#800` equals `#880000`.
- `#RGBA`: short notation with alpha. Corresponds to the same color as `#RRGGBBAA`. Example: `#800F` equals `#880000FF`.
- `#RRGGBB`: full notation without alpha. The alpha is assumed to be `FF`.
- `#RRGGBBAA`: full notation with alpha.

#### Binary
Binary data strings store arbitrary data in hexadecimal format. They must start with `0x`, followed by the hexadecimal representation of the data, for example: `0x0004BAF890`. The literal `0x` represents a binary data string of length 0. The character length of the hexadecimal number must be an even number.

#### Null
Null values are encoded with `null` literals. Like booleans, null values must be lower-case. Null values never need type labels, but are allowed to be annotated with them.

### 2.3. Collections

#### Lists
Lists are collections of values. They are comma-delimited and enclosed with `[]` square brackets. Empty elements are not allowed (in other words, trailing commas are not allowed).

Lists may contain values of multiple types.

Example: `[1,'2',"3"]`.

#### Dictionaries
Dictionaries are collections of key-value pairs. They are comma-delimited and enclosed with `{}` curly braces. Empty elements are not allowed (in other words, trailing commas are not allowed).

Keys and values are seprated by `:` colons. Both keys and values may be of any type, including other collections, and dictionaries may contain keys and values of multiple types. To reduce parsing overhead and ensure maximum generality, keys are NOT required to be unique by the format.

Note that dictionary keys can be annotated with type labels, as can their values.

Example: `{"a":"abc",'b':"def",["c"]:"hij"}`.

#### Objects
Objects represent a collection of identifier-value pairs. They are comma-delimited and are enclosed with `<>` angular brackets. Empty members are not allowed (in other words, trailing commas are not allowed).

Identifiers are unquoted strings that may only consist of ASCII letters, digits and `_` underscores, and may not start with a number. They are not considered to be values, and cannot be annotated with type labels. They are case-sensitive. Just like with dictionary keys, identifiers are NOT required to be unique by the format.

Objects are similar to dictionaries, and are essentially a more compact but also more strict syntax, used to serialize classes and structs.

Example: `<my_int:0,my_real:0.0,my_char:'A'>`.