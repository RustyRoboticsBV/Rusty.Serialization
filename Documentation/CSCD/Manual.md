# CSCD User Manual

This document describes the CSCD syntax and its different literals in an informal way.


## Metadata

### ID
IDs may be attached to any value literal, after which they can be targeted by a reference literal.

```
`my_id`
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
Scopes may be attached to object member names, in order to disambiguate shadowed variables in objects with a complex inheritance chain.

```
^my_base^
^my_namespace.my_base^
^^
```

### Offsets
Offsets may be attached to timestamp literals, and represent timezones (as UTC offsets).

```
|+5:30|			      |-5:30|
|+5|			      |-5|
|Z|
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
12345			      -12345
```

### Float
A floating-point number of unspecified precision. May consist of an integer, fractional and exponent part.

```
1.3e-5			      -1.3e-5
.3e-5			      -.3e-5
1e-5			      -1e-5
1.e-5			      -1.e-5
1.3			          -1.3
1.			          -1.
.3			          -.3
.			          -.
```

### Infinity
An infinity value. May be either positive or negative infinity.

```
inf			          -inf
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
A decimal value. Unlike with floats, trailing zeros have explicit meaning.

```
$1.00			      -$1.00
$1			          -$1
$.01			      -$.01
$40.			      -$40.
```

### Color
A color value. Alpha is optional. A CSS shorthand notation is also suppoted.

```
#FF0000FF
#F00F
#FF0000
#F00
#
```

### Timestamp
A timestamp value, representing a date and a time.

```
@2000/1/1,13:0:0@     @-2000/1/1,13:0:0@
@200/11/5@            @-200/11/5@
@24:0:0@
@@
```

### Duration
A duration value, representing a timespan in days, hours, minutes and seconds.

```
10d5h1m10s
-10d5h1m10s
23h5s
-23h5s
1s
-1s
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
mySymbol
*my symbol*
**
```

### Reference
A reference that points to an object annotated with an ID.

```
&my_id&
&&
```

## Collections

### Lists
A collection of elements. Elements may be any type of literal.

```
[1, 2.0, 'C']
[]
```

### Dictionaries
A collection of key-value pairs. Keys and values may by any type of literal, and duplicate keys are allowed.

```
{false: 1, 2.0: nan, 'C': "abc"}
{}
```

### Objects
A collection of name-value pairs. Names must be symbol literals, and may be annotated with a scope. Values may be any type of literal. Duplicate names are allowed.

```
<a:0, b: 2.0, c: 'C'>
<^base^a:0, a:1>
<>
```