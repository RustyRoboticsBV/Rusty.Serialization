# CSCD Format Specification
This document describes the syntax of a serialized data format called *Compact Serialized C# Data* (CSCD). CSCD is a human-readable, compact, and unambiguous format capable of fully expressing arbitrary C# object graphs.

The format is self-describing and does not require an external schema. Values may be annotated with optional type labels and ID metadata, enabling parsers to reconstruct objects with their original types and preserve reference links.

Note that many of the CSCD literals are more abstract than a C# type. For example, all signed and unsigned integer primitives, regardless of precision, are represented using a single integer literal. Some common data structures, such as lists, time and colors, have dedicated compact literal forms in order to reduce verbosity.

The main design goals are generality, compactness and unambiguousness. While usable in any programming language, it was specifically designed to be used in a C# context where object graphs may contain polymorphic types or shared / cyclic references.

## 1. General Formatting
### Character Set
Serialized data is expressed as a subset of the `ISO-8859-1` character set, and may only contain the following characters:
- ASCII whitespace characters: `0x20` (space), `0x09` (horizontal tab), `0x0A` (line feed), and `0x0D` (carriage return).
- ASCII letters, digits and punctuation: `0x21` (`!`) to `0x7E` (`~`).
- Latin-1 Supplement letters, digits and punctuation: `0xA1` (`¡`) to `0xAC` (`¬`) and `0xAE` (`®`) to `0xFF` (`ÿ`).

### Whitespace
Whitespace is allowed between punctuation, primitive literals, type labels, IDs and collection delimiters for formatting. With the exception of spaces inside of character and string literals, whitespace has no meaning and should simply be stripped during parsing.

Spaces inside of character and string literals should be preserved as-is. They may otherwise not break up primitive literals, type labels and IDs. For example: `12 3 4` is invalid and does not form the literal `1234`.

### Comments
Comments are allowed using the `/* Comment text */` syntax. Parsers should simply treat them as whitespace and strip them, and do not need to preserve them if a string of CSCD is deserialized and reserialized. Comments cannot be nested.

Comments cannot appear inside character or string literals; in that context, they are considered part of the literal.

## 2. Data Types
Two categories of values are supported: primitives and collections. Additionally, values can be annotated with metadata.

Exactly one top-level value must be present in any string of serialized CSCD. This value may be of any type (including both primitives and collections), and may optionally be annotated with metadata.

### 2.1. Metadata

#### Type Labels
Type labels can optionally placed before any value, and are written as a name between `()` parentheses. Type labels act as hints for deserializers, telling them what kind of object was originally serialized. The format has no knowledge about what a type name actually *means*.

Type names may contain all characters from the allowed character set, except for parentheses and whitespace characters. They are case-sensitive. Type labels may not be followed by an ID or by another type label - they must be followed by a value of some kind. They may be used inside collections.

Serializers are encouraged to only generate type labels in situations where it would otherwise be ambiguous what kind of type was serialized.

Examples: `(i32)`, `(dict<str,str>)`, `(my_object)`, `(my_namespace.my_class<int>.my_struct<list<f64>>[])`.

#### IDs
IDs can optionally be placed before any value or type label, and are written as a name between `` ` `` backticks. Values that have been marked with an ID can be used in a reference literal (see the section on references below).

ID names may only consist of letters, numbers and `_` underscores. IDs must be unique, and are always defined globally (in other words, there are no local scopes or namespaces). They are case-sensitive. IDs must be followed by either a value or a type label.

Serializers are encouraged to only generate IDs in situations where a single object is referenced multiple times, or when cyclic references exist.

Examples: `` `my_referenced_int`5``, `` `my_referenced_object`<a:0,b:"abc">``, `` `my_referenced_typed_value`  (MyStringType)"abcdefg"``

### 2.2. Primitives

#### Null
Null values are encoded with `null` literals. Null values must be lower-case. They can be annotated with type labels like any other value.

#### Booleans
Booleans can be one of two literals: `true` or `false`. Boolean values must be lowercase.

#### Integers
Integer literals can be any combination of digits. Optionally, they may start with a negative sign. Leading zeros are allowed.

Examples:
- `1` and `001` are both valid representations of the number `1`.
- `-50` and `-00050` are both valid representations of the number `-50`.

#### Reals
Real literals must contain a decimal point. Optionally, they may start with a negative sign. Other than that, they may only consist of digits. If the integer part and/or the fractional part are equal to 0, they may be omitted. Consequently, the literal `.` can be used to represent 0; `-.` can be used for -0. Leading and trailing zeros are allowed.

Examples:
- `0.0`, `000.000`, `0.`, `.0` and `.` are all valid representations of the number `0.0`.
- `-0.0`, `-000.000`, `-0.`, `-.0` and `-.` are all valid representations of the number `-0.0`.
- `-0.5`, `-.5` and `-00.50` are all valid representations of the number `-0.5`.

#### Infinities
Infinity values are encoded as one of two literals: `inf` for positive infinity and `-inf` for negative infinity. Both must be lowercase.

#### Not A Number
NaN values are encoded with the `nan` literal. NaN values must be lowercase.

#### Characters
Characters must be enclosed in `'` single-quotes. Only a single character may be stored inside. Empty character literals are not allowed.

The following characters from the character set may not appear in a character literal: `0x09` (horizontal tab), `0x0A` (newline) and `0x0D` (carriage return).

A few special character literals exist:
- `'\''`: alternative way of writing `'''`.
- `'\"'`: alternative way of writing `'"'`.
- `'\\'`: alternative way of writing `'\'`.
- `'\t'`: expresses a horizontal tab.
- `'\n'`: expresses a newline.
- `'\...\'`: expresses a unicode character. `...` must be a hexadecimal number of 1-8 digits, representing a Unicode code point between `0` and `10FFFF`. Leading zeros are allowed. Letters in hexcodes must be uppercase.

Examples: `'A'`, `'ç'`, `'''`, `'\n'`, `'\21FF\'`.

#### Strings
Strings must be enclosed in `"` double-quotes. Empty strings are allowed. The same character set is used as for character literals. The same special character rules apply as well, except that using unescaped double-quotes and backslashes is NOT allowed -  you MUST use `\"` and `\\` to represent them.

Example: `"This is a \"string\"!"`, `"¡No habló español!"`, `"\21FF\tarrow"`, `"C:\\path\\to\\file"`.

#### Colors
Colors literals must start with a `#` hex sign, followed by the hexadecimal representation of the color. Four conventions are available:
- `#RGB`: short notation. Corresponds to the same color as `#RRGGBB` (so each digit is duplicated). Example: `#800` equals `#880000`.
- `#RGBA`: short notation with alpha. Corresponds to the same color as `#RRGGBBAA`. Example: `#800F` equals `#880000FF`.
- `#RRGGBB`: full notation without alpha. The alpha is assumed to be `FF`.
- `#RRGGBBAA`: full notation with alpha.

Color literals are case-sensitive and must be uppercase.

#### Time
Time literals contain date and/or time-of-day data, and can be used to express both date/time values as well as timespans. They use the format `Y...M...D...h...m...s...f...n...`, where the characters between the letters should only consist of digits. A term may be omitted if it equals 0 (and omitted terms should be parsed as such), as long as at least one term remains. Additionally, the first character may be a `-` minus sign for negative times (this sign applies to the *entire* time literal).

Each part represents a different unit:
- `Y`: the number of years.
- `M`: the number of months.
- `D`: the number of days.
- `h`: the number of hours.
- `m`: the number of minutes.
- `s`: the number of seconds.
- `f`: the number of milliseconds.
- `n`: the number of nanoseconds.

These prefixes are case-sensitive.

Any positive integer number is allowed for each term, negative numbers are not (i.e. `Y5M-2D5`). Leading zeros are allowed. Terms that equal 0 can be omitted, and the different terms may come in any order. All unit prefixes must be followed by at least one digit - empty terms are not allowed (i.e. `YMD200`). A single unit cannot appear more than once (i.e. `Y2Y2`).

For example:
- `Y1999M2D1h13` and `D1M2Y1999h13m0s0` are both valid representations of the date and time `February 1st 1999, 1 P.M.`.
- `s1f100` and `m0f100s1` are both valid representations of the timespan `1 second and 100 milliseconds`.
- `-Y100000` and `-Y100000M0D0h0m0s0f0` are both valid representations the year `-100,000 B.C.`.

Since time literals can represent both date/times and timespans, they do NOT have to represent valid dates or times of day - so a value like `Y2005M13D200` is allowed.

#### Currency
Currency literals represent decimal numbers with significant fractional digits, where trailing zeros have explicit meaning. They are primarily intended for monetary values, but may be used for any numeric value where preserving the exact decimal representation is important. Parsers should make sure to preserve fractional digits, including trailing zeros.

The syntax is `$1.00` for positive values and `-$1.00` for negative values. Any number of fractional digits is allowed (i.e. `$10.12345`). If there are no fractional digits, then the decimal point may be omitted (so `$1.` is equivalent to `$1`). Leading zeros are allowed, but have no meaning. If the integer part fully consists of zeros, it may be omitted entirely (i.e. `$00.5`, `$0.5` and `$.5` are all equivalent). However, `$1.0` and `$1.00` are NOT equivalent.

The literal `$` is equivalent to `$0`; `-$` is equivalent to `-$0`.

Currency literals exist to preserve decimal scale information during intermediate parsing stages (such as node-tree representations) where target language types are not yet known. Real literals do not encode whether trailing zeros are significant, and so they may be truncated upon reserialization. Currency literals ensure that the number of fractional digits remains intact.

#### Bytes
Bytes literals store arbitrary data in the RFC 4648 Base64 format. They must start with `b_`, followed by the Base64-encoded data. Base64 strings must have a length that is a multiple of 4; padding using `=` characters is **mandatory**. For example, the bytestring `00 02 04 07 09 0E 03` is represented by the bytes literal `b_AAIEBwkPAw==`. The literal `b_` (without any padding) represents a bytestring of length 0.

#### References
Reference values are used to link to values that have been marked with an ID. They must start with an `&` ampersand, followed by the name of an ID (example: `&my_id`). This ID must exist elsewhere in the data.

There are no scope limitations on where in the data an ID can be referenced: IDs that are defined before the reference, after the reference or inside a different nested collection are all allowed. Cyclic references are also allowed.

References can be annotated with a type labels, but may NOT be annotated with an ID. They may not appear as the top-level value, but may otherwise appear anywhere inside any collection literal.

### 2.3. Collections

#### Lists
Lists are collections of values. They are comma-delimited and enclosed with `[]` square brackets. Empty elements are not allowed (in other words, trailing commas are not allowed).

Lists may contain values of multiple types.

Example: `[1,'2',"3"]`.

#### Dictionaries
Dictionaries are collections of key-value pairs. They are comma-delimited and enclosed with `{}` curly braces. Empty elements are not allowed (in other words, trailing commas are not allowed).

Keys and values are separated by `:` colons. Both keys and values may be of any type, including other collections, and dictionaries may contain keys and values of multiple types. To reduce parsing overhead and ensure maximum generality, keys are NOT required to be unique by the format.

Note that dictionary keys can be annotated with type labels, as can their values.

Example: `{"a":"abc",'b':"def",["c"]:"hij"}`.

#### Objects
Objects represent a collection of identifier-value pairs. They are comma-delimited and are enclosed with `<>` angular brackets. Empty members are not allowed (in other words, trailing commas are not allowed).

Identifiers are unquoted strings that may only consist of ASCII letters, digits and `_` underscores, and may not start with a number. They are not considered to be values, and cannot be annotated with type labels. They are case-sensitive. Just like with dictionary keys, identifiers are NOT required to be unique by the format.

Objects are similar to dictionaries, and are essentially a more compact but also more strict syntax, used to serialize classes and structs.

Example: `<my_int:0,my_real:0.0,my_char:'A'>`.
