# CSCD User Manual

This document describes the CSCD syntax and its different literals in an informal way.

## Format Markers

### Header
An optional marker that can be used to mark a string as being CSCD. It must be the first token in the document.

```
~CSCD~
```

### Footer
An optional marker that can be used to mark the end of a string of CSCD. It must be the last token in the document.

```
~/CSCD~
```

## Comments
Comments may appear anywhere in the CSCD string (except before the header and after the footer, if present). Comments are discarded when parsed.

```
;; Comment text. ;;
```

## Whitespace
Whitespace may appear between format markers, comments, interpunction and literals. ASCII spaces, horizontal tabs, line feeds and carriage returns may be used.

## Metadata

### Address
Addresses may be attached to any value literal, after which they can be targeted by a reference literal.

```
`my_address`
``
```

### Type
Type hints may be attached to any value literal, which can help the runtime disambiguate polymorphic types.

```
(my_type)
(my_namespace.my_type)
()
```

### Scope
Scopes hints may be attached to object member names, in order to disambiguate shadowed variables in objects with a complex inheritance chain.

```
^my_base^
^my_namespace.my_base^
^^
```

### Offset
Offsets may be attached to timestamp literals, and represent timezones (as UTC offsets).

```
|+5:30|			        |-5:30|             Hour and minute offset.
|+5|			        |-5|                Hour-only. Minute is assumed to be 00.
|Z|                                         Shorthand for '+00:00' (Greenwich Mean Time).
```

## Primitives

### Null
A null value.

```
null
```

### Bool
A boolean. Must be either true or false.

```
true
false
```

### Int
An integer number of unspecified precision.

```
12345			        -12345
```

### Float
A floating-point number of unspecified precision. May consist of an integer, fractional and exponent part. Each part may be omitted; omitted parts are assumed to be `0`.

```
1.3e-5			        -1.3e-5             Full notation.
.3e-5			        -.3e-5              Omitted integer.
1e-5			        -1e-5               Omitted fractional.
1.e-5			        -1.e-5              Omitted fractional.
1.3			            -1.3                Omitted exponent.
1.			            -1.                 Omitted fractional and exponent.
.3			            -.3                 Omitted integer and exponent.
.			            -.                  Omitted integer, fractional and exponent.
```

### Infinity
An infinity value. May be either positive or negative infinity.

```
inf			            -inf
```

### NaN
A not-a-number value.

```
nan
```

### Char
A character value.

```
'A'
'\1F4a9;'
'''
''
```

### String
A string value.

```
"ABC"
"Smile: \1F60A;"
"\""
""
```

### Decimal
A decimal value. Unlike with floats, trailing zeros have explicit meaning and should be maintained by a parser.

```
$1.00			        -$1.00              Integer with two fractional digits.
$1			            -$1                 No fractional digits.
$.01			        -$.01               Shorthand for '0.01'.
$40.			        -$40.               Shorthand for '40.0'.
$.                      -$.                 Shorthand for '0.0".
$                       -$                  Shorthand for '0'.
```

### Color
A color value, written as a hexcode. Alpha is optional, and is assumed to be `FF` when omitted. A CSS shorthand notation is also supported.

```
#FF0000FF                                   Red (full notation)
#0F0F                                       Green (short notation)
#0000FF                                     Blue (no alpha)
#FF0                                        Yellow (no alpha, short notation)
#                                           Transparent black
```

### UID
A unique identifier value, written as a 128-bit hexadecimal number in the format XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX (8-4-4-4-12). The dashes and leading zeros may be omitted.

```
%3f4e1a9c-8d3b-24e4-c8f7-b3a8d5c1e9f2       Full notation.
%3f4e1a9c8d3b24e4c8f7b3a8d5c1e9f2           Omitted dashes.
%1-23456789abcd                             Shorthand for '00000000-0000-0000-0001-23456789abcd'.
%123456789abcd                              Shorthand with omitted dashes.
```

### Timestamp
A timestamp value, representing a date and a time.

```
@2000/1/1,13:0:0@       @-2000/1/1,13:0:0@  Date and time.
@200/11/5@              @-200/11/5@         Date only. Time is assumed to be 00:00:00.
@24:0:0@                                    Time only. Date is assumed to be 01/01/01.
@@                                          Shorthand for 01/01/01, 00:00:00.
```

### Duration
A duration value, representing a timespan expressed in several units. The order must be days, hours, minutes and seconds. Units may be omitted; omitted units are assumed to equal `0`.

```
10d5h1m10s              -10d5h1m10s         Full notation with all units.
1h15m30s                -1h15m30s           One unit omitted.
23h5s                   -23h5s              Two units omitted.
1000d                   -1000d              Three units omitted.
```

### Bytes
A bytestring in Base64, representing arbitrary byte arrays.

```
!SGVsbG8sIHdvcmxkIQ
!
```

### Symbol
A symbol, meant for enums and named constants.

```
mySymbol                                    Bare notation.
*my symbol*                                 Delimited notation.
**                                          Empty symbol.
```

### Reference
A reference that points to an object annotated with an address.

```
&my_address&
&&
```

## Collections

### List
A collection of elements. Elements may be any type of literal.

```
[1, 2, 3, 4, 5]                             List of integers.
[1, 2.0, 'C']                               List of mixed content.
[]                                          Empty list.
```

### Dictionary
A collection of key-value pairs. Keys and values may by any type of literal, and duplicate keys are allowed.

```
{"a": 0, "b": 1, "c": 2}                    Dictionary with string keys and integer values.
{false: 1, 2.0: nan, 'C': "abc"}            Dictionary with mixed keys and values.
{}                                          Empty dictionary.
```

### Object
A collection of name-value pairs. Names must be symbol literals, and may be annotated with a scope. Values may be any type of literal. Duplicate names are allowed.

```
<a:0, b: 2.0, c: 'C'>
<^base^a:0, a:1>
<>
```