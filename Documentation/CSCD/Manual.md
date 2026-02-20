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

```
null
```

### Bool

```
true
false
```

### Int

```
12345			      -12345
```

### Float

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

```
inf			          -inf
```

### NaN

```
nan
```

### Char

```
'A'
'\1F4a9;'
'''
''
```

### String

```
"ABC"
"Smile: \1F60A;"
"\""
""
```

### Decimal

```
$1.00			      -$1.00
$1			          -$1
$.01			      -$.01
$40.			      -$40.
```

### Color

```
#FF0000FF
#F00F
#FF0000
#F00
#
```

### Timestamp

```
@2000/1/1,13:0:0@     @-2000/1/1,13:0:0@
@200/11/5@            @-200/11/5@
@24:0:0@
@@
```

### Duration

```
10d5h1m10s
-10d5h1m10s
23h5s
-23h5s
1s
-1s
```

### Bytes

```
!SGVsbG8sIHdvcmxkIQ
!
```

### Symbol

```
mySymbol
*my symbol*
**
```

### Reference

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