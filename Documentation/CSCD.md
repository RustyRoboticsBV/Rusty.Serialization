# CSCD Format Specification

## Introduction
This formal specification document describes the syntax of a serialized data format called *Compact Serialized C# Data* (CSCD). CSCD is a human-readable, compact, and unambiguous format capable of fully expressing arbitrary C# object graphs.

The format is self-describing and does not require an external schema. Values may be annotated with optional type labels and ID metadata, enabling parsers to reconstruct objects with their original types and preserve reference links.

Note that many of the CSCD literals are more abstract than a C# type. For example, all signed and unsigned integer primitives, regardless of precision, are represented using a single integer literal. Some common data structures, such as lists, time and colors, have dedicated compact literal forms in order to reduce verbosity.

The main design goals are generality, compactness and unambiguousness. While usable in any programming language, it was specifically designed to be used in a C# context where object graphs may contain polymorphic types or shared / cyclic references.

The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT", "SHOULD", "SHOULD NOT", "RECOMMENDED",  "MAY", and "OPTIONAL" in this document are to be interpreted as described in [RFC 2119](https://datatracker.ietf.org/doc/html/rfc2119) and [RFC 8174](https://datatracker.ietf.org/doc/html/rfc8174).

## 1. General Formatting
### 1.1 Character Set
Serialized data MUST be expressed as a subset of the `ISO-8859-1` character set, and MUST only contain the following characters:
- ASCII whitespace characters: `0x20` (space), `0x09` (horizontal tab), `0x0A` (line feed), and `0x0D` (carriage return).
- ASCII letters, digits and punctuation: `0x21` (`!`) to `0x7E` (`~`).
- Latin-1 Supplement letters, digits and punctuation: `0xA1` (`¡`) to `0xAC` (`¬`) and `0xAE` (`®`) to `0xFF` (`ÿ`).

The character set defined in this section applies to the serialized textual representation of CSCD. Unicode [escape sequences](#unicode-escape-sequences) allow characters outside this set to be represented indirectly; after escape processing, values may contain arbitrary Unicode scalar values.

### 1.2 Structure
A serialized CSCD string MUST contain exactly one top-level value, which forms the root of the serialized representation. Parsers MUST interpret this top-level value as the entry point for deserialization. The top-level value MAY be annotated with metadata. Values that support nested content MAY contain nested values.

### 1.3 Whitespace
Whitespace MAY appear between literals and punctuation (`, : [ ] { } < >`) for formatting purposes. Whitespace MAY also appear before and after the top-level value. Unless otherwise stated, whitespace MUST NOT break up literals that are multiple characters long.

Serializers SHOULD NOT emit whitespace in situations where readability is not important.

### 1.4 Comments
Comments MAY be used with the `/* Comment text */` syntax and MAY appear anywhere where whitespace may appear. Parsers MUST treat them as whitespace and strip them. Parsers MAY discard comments during deserialization and reserialization. Comments MUST NOT be nested.

Comments MUST NOT be recognized inside character or string literals. Within such literals, the comment syntax MUST be treated as literal content.

### 1.5 Escape Sequences
Escape sequences MAY appear in literals that support them. Literals MUST NOT allow escape sequences unless explicitly stated otherwise in their definition.

The following escape sequences MUST be recognized by parsers if they appear in a literal that allows escape sequences:

|Character  |Code point |Escape sequence|       |Character  |Code point |Escape sequence|
|-----------|-----------|---------------|-------|-----------|-----------|---------------|
|tab        |`0x09`     |`\t`           |       |`/`        |`0x2F`     |`\/`           |
|line feed  |`0x0A`     |`\n`           |       |`:`        |`0x3A`     |`\:`           |
|space      |`0x20`     |`\s`           |       |`>`        |`0x3E`     |`\>`           |
|`"`        |`0x22`     |`\"`           |       |`\`        |`0x5C`     |`\\`           |
|`'`        |`0x27`     |`\'`           |       |`]`        |`0x5D`     |`\]`           |
|`)`        |`0x29`     |`\)`           |       |`` ` ``    |`0x60`     |`` \` ``       |
|`,`        |`0x2C`     |`\,`           |       |`}`        |`0x7D`     |`\}`           |

Parsers MUST recognize and correctly interpret all escape sequences from this table when they appear in a literal that allows escape sequences. Invalid escape sequences MUST be rejected by a parser.

#### Unicode Escape Sequences
In addition to the table above, literals that allow for escape sequences MAY contain Unicode escape sequences of the form `\...\`.
- Unicode escape sequences MUST start and end with a `\` backslash.
- `...` MUST consist of one or more uppercase hexadecimal digits (`0`-`9`, `A`-`F`).
- The numeric value represented by `...` MUST be in the range `0x0` to `0x10FFFF`.
- Parsers MUST correctly interpret escape sequences regardless of the number of digits; leading zeroes SHOULD be discarded.
- Parsers MUST replace each Unicode escape sequence with the corresponding Unicode code point when interpreting the literal content.

Serializers SHOULD emit Unicode escape sequences using the minimal number of digits necessary to represent the code point. For example, `\B\` SHOULD be used instead of `\000B\`.

## 2. Data Types
CSCD supports two categories of value literals: primitives and collections. Additionally, values MAY be annotated with metadata.

A valid serialized CSCD string MUST contain exactly one top-level value. This top-level value MAY be any type of literal except reference literals, as parsers cannot resolve a reference without an accompanying ID. The top-level value MAY be annotated with metadata.

### 2.1. Metadata

#### Type Labels
Type labels MAY be placed before any value, and are written as a name between `()` parentheses. Type labels act as hints for parsers, indicating what kind of object was originally serialized. The format does not interpret or validate type names; it is up to the parser to map a type name to the corresponding runtime type.

Type labels are case-sensitive and MUST NOT contain whitespace. Escape sequences MAY be used; ASCII spaces, tabs, line feeds, `)` right parentheses and `\` backslashes MUST be escaped using their respective [escape sequences](#14-escape-sequences). All other characters from the character set MAY appear unescaped.

Type labels MUST be immediately followed by a value. They MUST NOT be followed by an ID or another type label. Type labels MAY appear inside collections.

Serializers SHOULD only emit type labels when necessary to disambiguate the type of a value.

Examples: `(i32)`, `(dict<str,str>)`, `(my_object)`, `(my_namespace.my_class<int>.my_struct<list<f64>>[])`.

#### IDs
IDs MAY be placed before any concrete value or type label, and are written as a name between `` ` `` backticks. Values annotated with an ID can be referenced elsewhere using a [reference literal](#references). IDs MUST NOT be applied to reference literals. IDs MUST be unique, and are always defined globally; there are no local scopes or namespaces.

IDs are case-sensitive and MUST NOT contain whitespace. Escape sequences MAY be used; ASCII spaces, tabs, line feeds, `` ` `` backticks and `\` backslashes MUST be escaped using their respective [escape sequences](#14-escape-sequences). All other characters from the character set MAY appear unescaped.

Serializers SHOULD emit IDs only when a single object is referenced multiple times or when a cyclic reference exists.

Examples: `` `my_referenced_int`5``, `` `my_referenced_object`<a:0,b:"abc">``, `` `my_referenced_typed_value`  (MyStringType)"abcdefg"``

### 2.2. Primitives

#### Null
Null values MUST be encoded using the literal `null`. Null values MUST be lower-case. Null literals MAY be annotated with type labels like any other value.

#### Booleans
Boolean values MUST be encoded using one of the literals `true` or `false`. Boolean literals MUST be lowercase.

#### Integers
Integer literals MUST consist of one or more decimal digits (`0`-`9`). An optional leading `-` minus sign MAY be used to indicate a negative value. Leading zeros SHOULD be discarded by a parser.

Parsers SHOULD distinguish positive and negative zero if the runtime type permits the distinction.

Examples:
- `1` and `001` are both valid representations of the number `1`.
- `-50` and `-00050` are both valid representations of the number `-50`.

#### Floats
Float literals MUST contain a decimal point. An optional leading `-` minus sign MAY be used to indicate a negative value. Aside from the decimal point and optional sign, float literals MUST consist only of decimal digits (`0`-`9`).

The integer part and/or fractional part MAY be omitted if equal to zero. Consequently, the literal `.` MUST be interpreted as `0.0`, and `-.` MUST be interpreted as `-0.0.` Leading and trailing zeros SHOULD be discarded by a parser.

Parsers SHOULD distinguish positive and negative zero if the runtime type permits the distinction.

Examples:
- `0.0`, `000.000`, `0.`, `.0` and `.` are all valid representations of the number `0.0`.
- `-0.0`, `-000.000`, `-0.`, `-.0` and `-.` are all valid representations of the number `-0.0`.
- `-0.5`, `-.5` and `-00.50` are all valid representations of the number `-0.5`.

#### Infinities
Infinity values MUST be encoded using one of two literals: `inf` for positive infinity and `-inf` for negative infinity. Infinity literals MUST be lowercase.

#### Not A Number
NaN values MUST be encoded using the literal nan. NaN literals MUST be lowercase.

#### Characters
Character literals MUST be enclosed in `'` single-quotes. A character literal MUST contain zero or one character. The empty character literal `''` MUST be interpreted as the character `0x00` (ASCII null).

Character literals MUST NOT contain the characters `0x09` (horizontal tab), `0x0A` (newline) and `0x0D` (carriage return). Escape sequences MAY be used; tabs and line feeds MUST be escaped using their respective [escape sequences](#14-escape-sequences). All other characters from the character set MAY appear unescaped.

**Note**: the `'` apostrophe MAY appear unescaped inside a character literal.

Examples: `'A'`, `'ç'`, `'''`, `'\n'`, `'\21FF\'`.

#### Strings
String literals MUST be enclosed in `"` double-quotes. Empty strings MAY appear.

String literals MUST NOT contain the characters `0x09` (horizontal tab), `0x0A` (newline) and `0x0D` (carriage return). Escape sequences are allowed; tabs, line feeds, `"` double-quotes and `\` backslashes must be escaped using their respective [escape sequences](#14-escape-sequences). All other characters from the character set MAY appear unescaped.

Example: `"This is a \"string\"!"`, `"¡No habló español!"`, `"\21FF\\tarrow"`, `"C:\\path\\to\\file"`.

#### Decimals
Decimal literals represent numeric values with significant fractional digits. They are intended for any use-case where preserving the exact decimal representation is important, such as when expressing monetary values. Parsers MUST preserve all fractional digits, including trailing zeros.

Decimal literals MUST start with `$` for positive values or `-$` for negative values, followed by an OPTIONAL integer value, an OPTIONAL `.` decimal point and an OPTIONAL fractional value. The integer and fractional values MUST consist of one or more decimal digits (`0`-`9`). Leading zeros SHOULD be discarded by a parser.

The integer part and/or fractional part MAY be omitted if equal to zero. The decimal point MAY be omitted if both the integer and fractional part are equal to zero. Consequently, `$0`, `$0.`, `$.0`, `$.` and `$` MUST all be interpreted as `$0.0`, and `-$0`, `-$0.`, `-$.0`, `-$.` and `-$` MUST all be interpreted as `-$0.0`.

Parsers SHOULD distinguish positive and negative zero if the runtime type permits the distinction.

Examples: `$123`, `$4.567`, `$.05`, `-$2`.

#### Colors
Colors literals MUST start with a `#` number sign, followed by the hexadecimal representation of the color. Four notations are supported:
- `#RGB`: short notation. When parsed, each digit MUST be duplicated to form the equivalent `#RRGGBB`. Example: `#800` MUST be interpreted as `#880000`.
- `#RGBA`: short notation with alpha. When parsed, each digit MUST be duplicated to form the equivalent `#RRGGBBAA`. Example: `#800F` MUST be interpreted as `#880000FF`.
- `#RRGGBB`: full notation without alpha. The alpha channel MUST be assumed to be `FF`.
- `#RRGGBBAA`: full notation with alpha.

Color literals MUST use uppercase hexadecimal digits (`0`–`9`, `A`–`F`). Parsers MUST interpret the values according to the rules above.

#### Times
Time literals represent absolute moments in time. They are intended to express date and/or time values, but do not represent durations or timespans.

Time literals MUST start with an `@` at sign, followed by the date/time. Three notations are supported:
- `@{year}-{month}-{day}_{hour}:{minute}:{second}`.
- `@{year}-{month}-{day}`. The time component MUST be assumed by a parser to be `0:0:0` (i.e. `12 A.M.`), but MAY be discarded when deserializing to a date-only type.
- `@{hour}:{minute}:{second}`. The date component MUST be assumed by a parser to be `0:1:1` (i.e. `January 1st, 0 A.D.`), but MAY be discarded when deserializing to a time-only type.

Each component has range and/or syntax rules that MUST be followed by a parser. Unless otherwise stated, they MUST be comprised of one of more decimal digits (`0`-`9`). Leading zeroes SHOULD be discarded by a parser.
- Year components MAY be prefixed with a `-` minus sign for dates before `January 1st, 0 A.D.`. After that MUST follow zero or more decimal digits (`0`-`9`). There is no limit on the value range; a parser MUST correctly interpret any integer value.
- Month components MUST be valid integer values in the range `1`-`12`.
- Day components MUST be valid integer values in the range `1`-`31`.
- Hour components MUST be valid integer values in the range `0`-`24`. The hour `24` is only allowed if the minute and second both equal `0`. A parser SHOULD maintain the distinction between `0` and `24` if possible.
- Minute components MUST be valid integer values in the range `0`-`59`.
- Second components MUST either be valid integer numbers or follow the format `[integer].[fractional]`. The integer part MUST be a valid between `0`-`60`. The value `60` is included to account for leap seconds; a parser SHOULD maintain the distinction between `0` and `60` if possible.

Time literals MAY represent dates or times that do not correspond to valid calendar dates (e.g., `@1994-2-31` is allowed). A parser SHOULD validate calendar correctness, but this is not enforced by the format.

#### Bytes
Bytes literals represent arbitrary data that cannot be efficiently expressed using another literal.

They MUST start with the prefix `b_`, followed by the data encoded in [RFC 4648 Base64](https://datatracker.ietf.org/doc/html/rfc4648), using the alphabet `A`-`Z`, `a`-`z`, `0`-`9`, `+`, `/` and `=`. The Base64 MUST have a length that is a multiple of 4; padding using `=` characters is **mandatory** to fulfill this rule. For example, the bytestring `00 02 04 07 09 0E 03` is represented by the bytes literal `b_AAIEBwkPAw==`.

An empty byte literal (representing zero bytes) MUST be written as `b_`.

#### References
Reference values are used to link to values that have been marked with an ID. They MUST start with an `&` ampersand, followed by the name of an ID (example: `&my_id`). This ID MUST exist elsewhere in the data.

Reference literals MUST NOT be used as the top-level value. Otherwise, they MAY appear anywhere inside collection literals. Cyclic references are allowed. References can appear before or after the definition of the ID they refer to, and may cross nested collections.

Reference literals MAY be annotated with type labels, but MUST NOT be annotated with IDs.

Reference literals MUST NOT contain whitespace. Escape sequences MAY be used; ASCII spaces, tabs, line feeds, `,` commas, `:` colons, `]` right square brackets,  `}` right curly braces, `>` right angular brackets, and `\` backslashes MUST be escaped using their respective [escape sequences](#14-escape-sequences). All other characters from the character set MAY appear unescaped.

### 2.3. Collections

#### Lists
Lists represent ordered collections of zero or more values.

A list MUST be enclosed in `[]` square brackets. Element values MUST be separated by `,` commas. Whitespace MAY appear before or after any element value or comma; parsers MUST ignore this whitespace.

Lists MAY contain values of any type, including other collections, and MAY mix multiple types of elements. Each list element MAY be annotated with metadata. Trailing commas MUST NOT appear (e.g. `[1,2,3,]` is invalid).

List elements MUST be interpreted in the order in which they appear.

Lists MAY be empty, which MUST be represented by the literal `[]`.

Example: `[]`, `[1,'2',"3"]`, `[['c', &ref], $1.00, (my_dict){}]`.

#### Dictionaries
Dictionaries are collections of key-value pairs.

A dictionary MUST be enclosed in `{}` curly braces. Key-value pairs MUST be separated by `,` commas, and keys and values MUST be separated by a `:` colon. Whitespace MAY appear before or after any key, value, colon, or comma; parsers MUST ignore this whitespace.

Keys and values MAY be of any type, including other collections. Dictionaries MAY mix multiple types of keys and values. To maximize generality, keys are NOT required to be unique by the format. Dictionary keys and values MAY be annotated with metadata. Trailing commas MUST NOT appear in a dictionary (e.g., `{"a":1,}` is invalid).

Empty dictionaries MUST be represented by the literal `{}`.

Example: `{"a":"abc",'b':"def",["c"]:"hij"}`.

#### Objects
Object literals represent a collection of identifier-value pairs. Objects function similarly to dictionaries but provide a more compact and stricter syntax intended for serializing struct-like data.

An object MUST be enclosed in `<>` angular brackets. Member pairs MUST be separated by , commas, and identifiers and values MUST be separated by a : colon. Whitespace MAY appear before or after any identifier, value, colon, or comma; parsers MUST ignore this whitespace. Trailing commas are NOT allowed (e.g., `<id:0,>` is invalid).

Identifiers MUST be unquoted strings that only consist of ASCII letters (`A`-`Z`, `a`-`z`), digits (`0`-`9`) and `_` underscores, and MUST NOT start with a number. They are not considered to be values, and MUST NOT be annotated with metadata. Identifiers are case-sensitive. Just like with dictionary keys, identifiers are NOT required to be unique.

Objects MAY contain values of any type, including other collections. Object member values MAY be annotated with type labels or IDs.

Empty objects MUST be represented by the literal `<>`.

Example: `<my_int:0,my_float:0.0,my_char:'A'>`.
