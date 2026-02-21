# CSCD Format Specification

## Introduction
This formal specification document describes the syntax of an object graph serialization format called *Compact Serialized C# Data* (CSCD). CSCD is a human-readable, compact, and unambiguous format capable of fully expressing arbitrary object graphs.

The format is syntactically self-describing and does not require an external schema for structural parsing. While it is *syntactically* structured like a tree, it *semantically* represents a graph. Values may be annotated with optional type and ID metadata, enabling parsers to reconstruct objects with their original types and preserve reference/pointer links.

CSCD literals are more general than C# types. For example, all signed and unsigned integer primitives, regardless of precision, are represented using a single integer literal. Some common composite data types, such as timestamps and colors, have dedicated compact literal forms in order to reduce verbosity.

The main design goals are generality, compactness and unambiguous parsing. While usable in any programming language, it was designed with the .NET framework in mind, where object graphs may contain polymorphic types, shared or cyclic references, and shadowed members. CSCD is primarily intended to be machine-generated, but human-inspectable. Though it can be authored by hand, this requires intimate knowledge of the target runtime and is generally not recommended.

This document will first describe some format-wide syntax rules. After that, it will describe each literal type and their syntaxes, starting with metadata, followed by primitives and finally collections.

The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT", "SHOULD", "SHOULD NOT", "RECOMMENDED",  "MAY", and "OPTIONAL" in this document are to be interpreted as described in [RFC 2119](https://datatracker.ietf.org/doc/html/rfc2119) and [RFC 8174](https://datatracker.ietf.org/doc/html/rfc8174).

## Table Of Contents
- [1. General Formatting](#1-general-formatting)
  - [1.1 Character Set](#11-character-set)
  - [1.2 Top-Level Value](#12--top-level-value)
  - [1.3 Format Markers](#13-format-markers)
  - [1.4 Whitespace](#14-whitespace)
  - [1.5 Comments](#15-comments)
  - [1.6 Escape Sequences](#16-escape-sequences)
    - [Unicode Escape Sequences](#unicode-escape-sequences)
  - [1.7 Invalid Input](#17-invalid-input)
- [2. Literals](#2-literals)
  - [2.1 Metadata](#21-metadata)
    - [IDs](#ids)
    - [Types](#types)
    - [Scopes](#scopes)
    - [Offsets](#offsets)
  - [2.2 Primitives](#22-primitives)
    - [Null](#null)
    - [Booleans](#booleans)
    - [Integers](#integers)
    - [Floats](#floats)
    - [Infinities](#infinities)
    - [Not-A-Number](#not-a-number)
    - [Characters](#characters)
    - [Strings](#strings)
    - [Decimals](#decimals)
    - [Colors](#colors)
    - [Timestamps](#timestamps)
    - [Durations](#durations)
    - [Bytes](#bytes)
    - [UIDs](#uids)
    - [Symbols](#symbols)
    - [References](#references)
  - [2.3 Collections](#23-collections)
    - [Lists](#lists)
    - [Dictionaries](#dictionaries)
    - [Objects](#objects)

## 1. General Formatting
### 1.1 Character Set
A valid string of CSCD MUST consist only of characters whose Unicode code points are in one of the following ranges:

|Range|Description|
|-|-|
|`0x09`|Horizontal tab|
|`0x0A`|Line feed|
|`0x0D`|Carriage return|
|`0x20`|Space|
|`0x21`-`0x7E`|`!` to `~`|
|`0xA1`-`0xAC`|`¡` to `¬`|
|`0xAE`-`0xFF`|`®` to `ÿ`|

For clarity: this table excludes the characters `0x00`-`0x08`, `0x0B`-`0x0C`, `0x0E`-`0x1F` (most C0 control codes), `0x7F` (delete), `0xAD` (soft hyphen), `0x80`-`0x9F` (C1 control codes) and `0xA0` (non-breaking space).

Characters outside these ranges MUST not appear directly in serialized text. They MUST be represented using [escape sequences](#unicode-escape-sequences) instead.

Parsers MUST reject input strings containing code points outside of the allowed character set before processing escape sequences. After escape processing, parsers MUST accept any valid Unicode code point allowed by the target platform.

CSCD enforces this restricted character set to ensure that serialized files remain editor-safe and diffable across all platforms and environments.

**Note**: The specification places no restrictions on how the text is stored or transmitted. Implementations may use any encoding (UTF-8, UTF-16, ISO-8859-1, etc.) as long as the serialized text, when interpreted as code points, obeys the rules above.

### 1.2  Top-Level Value
A serialized CSCD string MUST contain exactly one top-level value, which forms the root of the serialized representation. Parsers MUST interpret this top-level value as the entry point for deserialization. The top-level value MAY be annotated with metadata. Values that support nested content MAY contain nested values.

### 1.3 Format Markers

#### Header
Strings of CSCD MAY be marked as such by adding the OPTIONAL header text `~CSCD~` before the top-level value,  its metadata and any leading comments.

If present, the header MUST appear before the top-level value and MAY be preceded only by whitespace. It MUST NOT appear more than once and MUST NOT be preceded by comments or any literal.

Parsers that handle multiple formats MUST use this marker to recognize an input as CSCD. Parsers that only handle CSCD SHOULD NOT require the marker; serializers SHOULD always emit it regardless.

The sequence `~CSCD~` has special meaning only when it appears before the top-level value. If the same sequence appears elsewhere (including inside of delimited literals) it MUST be treated as ordinary literal content.

#### Footer
The OPTIONAL footer text `~/CSCD~` MAY be used to denote the end of a string of CSCD after the top-level value and any trailing comments.

If present, the footer MUST appear after the top-level value and MAY be followed only be whitespace. It MUST NOT appear more than once and MUST NOT be preceded by comments or any literal.

Parsers MAY use it to detect the end of an embedded string of CSCD. Parsers that handle non-embedded CSCD SHOULD NOT require the marker; serializers SHOULD emit it regardless.

The sequence `~/CSCD~` has special meaning only when it appears after the top-level value. If the same sequence appears elsewhere (including inside of delimited literals) it MUST be treated as ordinary literal content.

### 1.4 Whitespace
Whitespace MAY appear between literals and punctuation (`, : [ ] { } < >`) for formatting purposes. Whitespace MAY also appear before and after the top-level value, and even before the header format marker and after the footer format marker. Unless otherwise stated, whitespace MUST NOT break up literals that are multiple characters long.

Serializers SHOULD NOT emit whitespace in situations where readability is not important.

### 1.5 Comments
Comments MUST start and end with `;;` two semicolons (e.g. `;; Comment text ;;`). They MAY appear between literals and punctuation (`, : [ ] { } < >`), as well as before and after the top-level value. Serializers SHOULD NOT preserve comments on reserialization. Comments MUST NOT be nested, but MAY consist of multiple lines.

Comments MUST NOT be recognized inside delimited literals. Within such literals, substrings that match the comment syntax MUST be treated as literal content.

### 1.6 Escape Sequences
Escape sequences MAY appear in literals that support them. Literals MUST NOT allow escape sequences unless explicitly stated otherwise in their definition.

The following escape sequences MUST be recognized by parsers if they appear in a literal that allows escape sequences:

|Character      |Code point |Escape sequence|   |Character      |Code point |Escape sequence|
|---------------|-----------|---------------|---|---------------|-----------|---------------|
|Tab            |`0x09`     |`\t`           |   |`(`            |`0x28`     |`\(`           |
|Line Feed      |`0x0A`     |`\n`           |   |`)`            |`0x29`     |`\)`           |
|Carriage Return|`0x0D`     |`\r`           |   |`*`            |`0x2A`     |`\*`           |
|`"`            |`0x22`     |`\"`           |   |`\`            |`0x5C`     |`\\`           |
|`&`            |`0x26`     |`\&`           |   |`^`            |`0x5E`     |`\^`           |
|`'`            |`0x27`     |`\'`           |   |`` ` ``        |`0x60`     |`` \` ``       |

Parsers MUST recognize and correctly interpret all escape sequences from this table when they appear in a literal that allows escape sequences. Invalid escape sequences MUST be rejected by a parser.

#### Unicode Escape Sequences
In addition to the table above, literals that allow for escape sequences MAY contain Unicode escape sequences of the form `\...;`.
- Unicode escape sequences MUST start with a `\` backslash and end with a `;` semicolon.
- `...` MUST consist of one or more uppercase hexadecimal digits (`0`-`9`, `A`-`F`).
- The numeric value represented by `...` MUST be in the range `0x0` to `0x10FFFF`.
- Leading zeroes SHOULD be discarded.
- Parsers MUST replace each Unicode escape sequence with the corresponding Unicode code point when interpreting the literal content.

Serializers SHOULD emit Unicode escape sequences using the minimal number of digits necessary to represent the code point. For example, `\B;` SHOULD be used instead of `\000B;`.

### 1.7 Invalid Input
A parser MUST detect and handle invalid input. Upon encountering invalid input, a parser SHOULD reject the input and terminate deserialization.

The exact mechanism for rejection (e.g., exception, error code, logging) is implementation-defined and not legislated by this document, however parsers MUST NOT continue deserialization in a way that could produce an undefined or inconsistent object graph.

## 2. Data Types
CSCD supports two categories of value literals: primitives and collections. Additionally, values MAY be annotated with metadata.

A valid serialized CSCD string MUST contain exactly one top-level value. This top-level value MAY be any type of literal except reference literals, as parsers cannot resolve a reference without an accompanying ID. The top-level value MAY be annotated with metadata.

### 2.1. Metadata

#### IDs
IDs MAY be placed before any concrete value, type or offset literal, and are written as a name between `` ` `` backticks. Values annotated with an ID can be referenced elsewhere using a [reference literal](#references). IDs MUST NOT be applied to reference literals. IDs MUST be globally unique and are case-sensitive.

The following characters MUST NOT appear in ID literals and MUST instead be represented with [escape sequences](#16-escape-sequences):

|Character      |Code point |   |Character      |Code point |
|---------------|-----------|---|---------------|-----------|
|Tab            |`0x09`     |   |`\`            |`0x5C`     |
|Line feed      |`0x0A`     |   |`` ` ``        |`0x60`     |
|Carriage return|`0x0D`     |   |               |           |

All other characters from the character set MAY appear unescaped.

Serializers SHOULD emit IDs only when a single object is referenced multiple times or when a cyclic reference exists.

Examples: `` `my_referenced_int`5``, `` `my_referenced_object`<a:0,b:"abc">``, `` `my_referenced_typed_value`  (MyStringType)"abcdefg"``

#### Types
Type labels MAY be placed before any concrete value or offset literal, and are written as a name between `()` parentheses. Type labels act as hints for parsers, indicating what kind of object was originally serialized. The format does not interpret or validate type names; it is up to the parser to map a type name to the corresponding runtime type.

The following characters MUST NOT appear in type literals and MUST instead be represented with [escape sequences](#16-escape-sequences):

|Character      |Code point |   |Character      |Code point |
|---------------|-----------|---|---------------|-----------|
|Tab            |`0x09`     |   |`)`            |`0x29`     |
|Line feed      |`0x0A`     |   |`\`            |`0x5C`     |
|Carriage return|`0x0D`     |   |               |           |

All other characters from the character set MAY appear unescaped.

Type labels MUST be immediately followed by a concrete value or offset literal. They MUST NOT be followed by an ID or another type. Type labels MAY appear inside collections.

Serializers SHOULD only emit type labels when necessary to disambiguate the type of a value.

Examples: `(i32)`, `(dict<str,str>)`, `(my_object)`, `(my_namespace.my_class<int>.my_struct<list<f64>>[])`.

#### Scopes
Scopes MAY be placed before any [object member name](#objects), and are written as a name between `^` Carets (e.g. `^my_scope^`). They act as hints within objects to determine which base class a member belongs to, resolving ambiguities caused by shadowed variables. The format does not interpret or validate scope names; it is up to the parser to map a scope name to the corresponding runtime base class.

The following characters MUST NOT appear in scope literals and MUST instead be represented with [escape sequences](#16-escape-sequences):

|Character      |Code point |   |Character      |Code point |
|---------------|-----------|---|---------------|-----------|
|Tab            |`0x09`     |   |`\`            |`0x5C`     |
|Line feed      |`0x0A`     |   |`^`            |`0x5E`     |
|Carriage return|`0x0D`     |   |               |           |

All other characters from the character set MAY appear unescaped.

Serializers SHOULD emit scopes only when an object contains multiple members with the same name.

Examples: `<^my_scope^my_member_name:"my_member_value>"`, `<^scope\^^a:0,a:1>`

#### Offsets
Offsets MAY be placed before any [timestamp](#timestamps) literal, and represent a time offset relative to UTC+0 (Greenwich Mean Time). Offsets MUST NOT appear before any other literal type, and MUST be followed by a timestamp. When attached to a timestamp, they mark that timestamp as being associated with that offset.

Three notations are supported:
- `|+{hours}:{minutes}|` or `|-{hours}:{minutes}|`: full notation.
- `|+{hours}|` or `|-{hours}|`: omitted minutes. The minutes MUST be interpreted as `0`.
- `|Z|` or `||`: shorthand of `|+00:00|`.

When present, the hours and minutes MUST be comprised of one or more digits (`0`-`9`). The hours MUST be in the range `0`-`23`, the minutes MUST be in the range `0`-`59`. 

Parsers MUST preserve offset information if the runtime type allows for it.

Examples: `|-2:30| @2000/5/1,13:00:00@`, `|+5| @1830/11/10@`, `|Z| @09:45:10@`.

### 2.2. Primitives

#### Null
Null values MUST be encoded using the literal `null`. Null values MUST be lower-case. Null literals MAY be annotated with type labels like any other value.

#### Booleans
Boolean literals represent the value 'true' or 'false'.

Boolean values MUST be encoded using one of the literals `true` or `false`. Boolean literals MUST be lowercase.

#### Integers
Integer literals represent integral numeric values.

Integer literals MUST consist of one or more decimal digits (`0`-`9`). An optional leading `-` minus sign MAY be used to indicate a negative value. Leading zeros SHOULD be discarded by a parser.

Parsers SHOULD distinguish positive and negative zero if the runtime type permits the distinction.

Examples:
- `1` and `001` are both valid representations of the number `1`.
- `-50` and `-00050` are both valid representations of the number `-50`.

#### Floats
Float literals represent real numeric values.

Several notations MAY be used:
- `[-]{integer}.{fractional}e[-]{exponent}`: full notation; an OPTIONAL sign, followed by a REQUIRED integer part, followed by a REQUIRED `.` decimal point, followed by a REQUIRED fractional part, followed by a REQUIRED `e`, followed by an OPTIONAL `-` minus sign, followed by a REQUIRED exponent part.
- `[-].{fractional}e[-]{exponent}`: omitted integer notation. The integer part MUST be interpreted as `0`.
- `[-]{integer}[.]e[-]{exponent}`: omitted fractional notation. The fractional part MUST be interpreted as `0`. The `.` decimal point is OPTIONAL.
- `[-]{integer}.{fractional}`: omitted exponent notation. The exponent part MUST be interpreted as `0`.
- `[-].{fractional}`: omitted integer and exponent notation. The integer and exponent parts MUST be interpreted as `0`.
- `[-]{integer}.`: omitted fractional and exponent notation. The fractional and exponent parts MUST be interpreted as `0`.
- `[-].`: omitted integer, fractional and exponent notation. The integer, fractional and exponent parts MUST be interpreted as `0`.

The integer, fractional and exponent parts (when included) MUST consist of one or more decimal digits (`0`-`9`). Leading zeros MAY appear in the integer and exponent parts, but MUST be discarded by a parser. Trailing zeros MAY appear in the fractional part, but MUST be discarded by a parser.

Parsers SHOULD distinguish positive and negative zero if the runtime type permits the distinction.

Serializers SHOULD generally avoid emitting the exponent notation except for numbers with a large number of zeros (e.g. `1.e10` instead of `10000000000.0`).

Examples:
- `0.0`, `000.000`, `0.`, `.0`, `.`, `0.0e0` and `.e000` are all valid representations of the number `0.0`.
- `-0.0`, `-000.000`, `-0.`, `-.0`, `-.` and `-.e0` are all valid representations of the number `-0.0`.
- `-0.5`, `-.5`, `-00.50` and `-5.0e-1` are all valid representations of the number `-0.5`.

#### Infinities
Infinity values MUST be encoded using one of two literals: `inf` for positive infinity and `-inf` for negative infinity. Infinity literals MUST be lowercase.

#### Not A Number
NaN values MUST be encoded using the literal nan. NaN literals MUST be lowercase.

#### Characters
Character literals MUST be enclosed in `'` single-quotes. A character literal MUST contain zero or one character (escape sequences count as one character). The empty character literal `''` MUST be interpreted as the character `0x00` (ASCII null, also represented as `'\0;'`).

The following characters MUST NOT appear in character literals and MUST instead be represented with [escape sequences](#16-escape-sequences):

|Character      |Code point |   |Character      |Code point |
|---------------|-----------|---|---------------|-----------|
|Tab            |`0x09`     |   |Carriage return|`0x0D`     |
|Line feed      |`0x0A`     |   |               |           |

All other characters from the character set MAY appear unescaped, including the `'` apostrophe.

Examples: `'A'`, `'ç'`, `'''`, `'\n'`, `'\21FF;'`.

#### Strings
String literals MUST be enclosed in `"` double-quotes. Empty strings MAY appear.

The following characters MUST NOT appear in string literals and MUST instead be represented with [escape sequences](#16-escape-sequences):

|Character      |Code point |   |Character      |Code point |
|---------------|-----------|---|---------------|-----------|
|Tab            |`0x09`     |   |`"`            |`0x22`     |
|Line feed      |`0x0A`     |   |`\`            |`0x5C`     |
|Carriage return|`0x0D`     |   |               |           |

All other characters from the character set MAY appear unescaped.

Example: `"This is a \"string\"!"`, `"¡No habló español!"`, `"\21FF;\tarrow"`, `"C:\\path\\to\\file"`.

#### Decimals
Decimal literals represent numeric values with significant fractional digits. They are intended for any use-case where preserving the exact decimal representation is important, such as when expressing monetary values. Parsers MUST preserve all fractional digits, including trailing zeros.

Five notations are supported:
- `[-]${integer}.{fractional}`: Full notation.
- `[-]$.{fractional}`: omitted integer part. The integer part MUST be interpreted as `0`.
- `[-]${integer}.`: omitted fractional part. The fractional part MUST be interpreted as `0` (one zero).
- `[-]$.`: omitted integer and fractional part. The integer and fractional parts MUST be interpreted as `0` (one zero).
- `[-]${integer}`: integral decimal with zero fractional digits.
- `[-]$`: integer decimal with zero fractional digits. The integer part MUST be interpreted as `0` (one zero).

The integer and fractional parts MUST consist of zero or more decimal digits (`0`-`9`). Leading zeros MAY appear in the integer part, but MUST be discarded by a parser.

Parsers SHOULD distinguish positive and negative zero if the runtime type permits the distinction.

Examples: `$123`, `$4.567`, `$.05`, `-$2`, `-$.`, `$`.

#### Colors
Colors literals MUST start with a `#` number sign, followed by the hexadecimal representation of the color. Five notations are supported:
- `#RRGGBBAA`: full notation with alpha.
- `#RRGGBB`: full notation without alpha. The alpha channel MUST be assumed to be `FF`.
- `#RGBA`: short notation with alpha. When parsed, each digit MUST be duplicated to form the equivalent `#RRGGBBAA`. Example: `#800F` MUST be interpreted as `#880000FF`.
- `#RGB`: short notation. When parsed, each digit MUST be duplicated to form the equivalent `#RRGGBB`. Example: `#800` MUST be interpreted as `#880000`.
- `#`: MUST be interpreted as `#00000000`.

Color literals MUST use uppercase hexadecimal digits (`0`-`9`, `A`-`F`). Parsers MUST interpret the values according to the rules above.

#### Timestamps
Timestamp literals represent absolute moments in time. They are intended to express date and/or time values. The timestamp literal exists primarily to provide a dedicated, canonical form for date/time types and discourage ad-hoc solutions using strings or object literals.

Timestamp literals MUST start and end with an `@` at symbol, with the date/time in-between. Four notations are supported:
- `@{year}/{month}/{day},{hour}:{minute}:{second}@`: full notation.
- `@{year}/{month}/{day}@`: date-only notation. The time MUST be interpreted as `0:0:0` (i.e. `12 A.M.`), but MAY be discarded when deserializing to a date-only type.
- `@{hour}:{minute}:{second}@`: time-only notation. The date MUST be interpreted as `1/1/1` (i.e. `January 1st, 1 A.D.`), but MAY be discarded when deserializing to a time-only type.
- `@@`: omitted date and time parts. It MUST be interpreted as the literal `@1/1/1,0:0:0@` (i.e. `January 1st, 1 A.D. at 12 A.M.`).

Each component has range and/or syntax rules that MUST be followed by a parser. Unless otherwise stated, they MUST be comprised of one of more decimal digits (`0`-`9`). Leading zeroes SHOULD be discarded by a parser.
- Year components MAY be prefixed with a `-` minus sign for dates before `January 1st, 1 A.D.`. After that MUST follow zero or more decimal digits (`0`-`9`). There is no limit on the value range; a parser MUST correctly interpret any integer value. The year `0` MUST NOT be used.
- Month components MUST be valid integer values in the range `1`-`12`.
- Day components MUST be valid integer values in the range `1`-`31`.
- Hour components MUST be valid integer values in the range `0`-`24`. The hour `24` is MUST NOT be allowed unless the minute and second both equal `0`. A parser SHOULD maintain the distinction between `0` and `24` if possible.
- Minute components MUST be valid integer values in the range `0`-`59`.
- Second components MUST either be valid [integer](#integers) or [float](#floats). If an integer, it must be in the range `0`-`60`; if a float, the integral part must be in the range `0`-`60`. The value `60` is included to account for leap seconds; a parser SHOULD maintain the distinction between `0` and `60` if possible. Trailing zeros SHOULD be discarded by a parser.

A parser SHOULD validate calendar correctness (i.e. rejecting `@1994/2/31@`), but this is not strictly enforced by the format.

Examples: `@2000/10/16,15:11:03.001@`, `@-500/2/7@`, `@07:30:00@`.

#### Durations
Duration literals represent relative time durations. They provide a canonical form of expressing a timespan.

Duration literals MUST consist of 1-4 terms:
- `{days}d`: the number of days, which MUST be a valid integer larger than or equal to `0`.
- `{hours}h`: the number of hours, which MUST be a valid integer in the range `0`-`23`.
- `{minutes}m`: the number of minutes, which MUST be a valid integer in the range `0`-`59`.
- `{seconds}s`: the number of seconds, which MUST be a valid [integer](#integers) or [float](#floats). If an integer, it must be in the range `0`-`59`; if a float, the integral part must be in the range `0`-`59`. Trailing zeros SHOULD be discarded by a parser.

Additionally, the first character may be a `-` minus sign for negative durations (e.g. `-30s`).

Terms that equal 0 MAY be omitted, though at least 1 term MUST remain. For durations of zero seconds, minutes, hours and days, any of the following MAY be used: `0d`, `0h`, `0m` or `0s`.

The term order MUST be days, hours, minutes and seconds (i.e. `5s10m` is invalid).

Examples: `5d1s`, `23h`, `-.s`, `100d10h59m0s`, `50m1e-5s`.

#### Bytes
Bytes literals represent arbitrary data that cannot be efficiently expressed using another literal.

They MUST start with the prefix `!`, followed by the data encoded in [RFC 4648 Base64](https://datatracker.ietf.org/doc/html/rfc4648), using the alphabet `A`-`Z`, `a`-`z`, `0`-`9`, `+`, `/` and `=`.

Padding using `=` MAY be used, but this is not enforced; parsers MUST handle bytes literals without padding by assuming trailing `=` padding characters. For example, the bytestring `00 02 04 07 09 0E 03` can be represented by the bytes literals `!AAIEBwkPAw` and `!AAIEBwkPAw==`.

An empty byte literal (representing zero bytes) MUST be written as `!`.

#### UIDs
UID literals represent a 128-bit identifier.

They MUST start with the prefix `%`, followed by the format `XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX` (8-4-4-4-12), where each `X` MUST be a lowercase hex digit (`0`-`9`, `a`-`f`). The `-` dashes MAY be omitted. Leading zeros MAY also be omitted (e.g. `%1-23456789` is equivalent to `00000000-0000-0000-0001-23456789`); the literal `%` MUST be interpreted a hash with 32 zeros.

UID literals are NOT REQUIRED to be valid according to [RFC 4122](https://www.rfc-editor.org/rfc/rfc4122) - unless the runtime type enforces UUID validity.

Examples: `%69988773-1484-832f-9fe1-a711cf10115f`, `%6998bd06ed3083338d8f142c0f7e52f5`, `%111`, `%`.

#### Symbols
Symbol literals represent named constants, identifiers, or enum values. They provide a semantic, human-readable alternative to when representing values whose internal numeric representations may vary or whose meaning is best captured by a stable name. Symbols are primarily intended for use with enumerations or well-known static constants. The format has no knowledge about the meaning of a symbol; it is up to the parser to properly resolve a symbol into a runtime value. 

Two symbol syntaxes MUST be recognized by parsers: delimited and bare.

A delimited symbol literal MUST start and end with `*` asterisk characters, with the symbol name in-between. They are case-sensitive. Delimited symbols MAY be empty, which MUST be represented using the literal `**`.

If a symbol starts with an ASCII letter (`A`-`Z`, `a`-`z`) or an `_` underscore, and contains only ASCII letters (`A`-`Z`, `a`-`z`), digits (`0`-`9`) and underscores (`_`), then the enclosing `*` asterisks may be omitted. For example, `_abc123` is equivalent to `*_abc123*`. The names `null`, `true`, `false`, `nan` and `inf` are reserved keywords and cannot be used as bare symbol names - they MUST be enclosed in `*` asterisks.

Symbol literals MAY be annotated with an ID and type label.

The following characters MUST NOT appear in `*` asterisk-delimited symbol literals and MUST instead be represented with [escape sequences](#16-escape-sequences):

|Character      |Code point |   |Character      |Code point |
|---------------|-----------|---|---------------|-----------|
|Tab            |`0x09`     |   |`*`            |`0x2A`     |
|Line feed      |`0x0A`     |   |`\`            |`0x5C`     |
|Carriage return|`0x0D`     |   |               |           |

All other characters from the character set MAY appear unescaped. Bare symbol literals MUST NOT contain escape sequences.

Serializers SHOULD emit the bare symbol form if the name is an allowed bare symbol name.

#### References
Reference values are used to link to values that have been marked with an ID. They MUST start and end with an `&` ampersand, with the name of an ID between them (example: `&my_id&`). This ID MUST exist elsewhere in the data.

Reference literals MUST NOT be used as the top-level value. Otherwise, they MAY appear anywhere inside collection literals. Cyclic references are allowed. References can appear before or after the definition of the ID they refer to, and may cross nested collections.

Reference literals MAY be annotated with type labels, but MUST NOT be annotated with IDs.

The following characters MUST NOT appear in reference literals and MUST instead be represented with [escape sequences](#16-escape-sequences):

|Character      |Code point |   |Character      |Code point |
|---------------|-----------|---|---------------|-----------|
|Tab            |`0x09`     |   |`&`            |`0x26`     |
|Line feed      |`0x0A`     |   |`\`            |`0x5C`     |
|Carriage return|`0x0D`     |   |               |           |

All other characters from the character set MAY appear unescaped.

It is up to parsers to decide whether a reference should be deserialized as a pointer, a reference, a copy, etc.

### 2.3. Collections

#### Lists
Lists represent collections of zero or more values. They MAY be used to represent ordered, unordered, resizable and fixed-size collections.

A list MUST be enclosed in `[]` square brackets. Element values MUST be separated by `,` commas. Whitespace MAY appear before or after any element value or comma; parsers MUST ignore this whitespace.

Lists MAY contain values of any type, including other collections, and MAY mix multiple types of elements. Each list element MAY be annotated with metadata. Trailing commas MUST NOT appear in lists (e.g. `[1,2,3,]` is invalid).

List elements MUST be interpreted in the order in which they appear.

Lists MAY be empty, which MUST be represented by the literal `[]`.

Examples: `[]`, `[1,'2',"3"]`, `[['c', &ref&], $1.00, (my_dict){}]`.

#### Dictionaries
Dictionaries are collections of key-value pairs.

A dictionary MUST be enclosed in `{}` curly braces. Key-value pairs MUST be separated by `,` commas, and keys and values MUST be separated by a `:` colon. Whitespace MAY appear before or after any key, value, colon, or comma; parsers MUST ignore this whitespace.

Keys and values MAY be of any type, including other collections. Dictionaries MAY mix multiple types of keys and values. To maximize generality, keys are NOT required to be unique by the format. Dictionary keys and values MAY be annotated with metadata. Trailing commas MUST NOT appear in a dictionary (e.g., `{"a":1,}` is invalid).

Dictionaries MAY be empty, which MUST be represented by the literal `{}`.

Example: `{"a":"abc",'b':"def",["c"]:"hij"}`.

#### Objects
Object literals represent a collection of name-value pairs, meant to model structured records.

An object MUST be enclosed in `<>` angular brackets. Member pairs MUST be separated by `,` commas, and names and values MUST be separated by a `:` colon. Trailing commas MUST NOT appear in an object (e.g., `<id:0,>` is invalid).

Member names MUST be symbols (delimited or bare). To maximize generality, member names are NOT required to be unique by the format. Member names MUST NOT be annotated with a type label and/or ID, but they MAY be annotated with a scope.

Member values MAY be any literal type, including other collections. Member values MAY be annotated with a type labels and/or ID, but MUST NOT be annotated with a scope.

Objects MAY be empty, which MUST be represented by the literal `<>`.

Example: `<my_int:0,my_float:0.0,my_char:'A',^my_base_class^*my_stríng*:"abc">`.