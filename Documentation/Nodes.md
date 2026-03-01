# Node Documentation

## Introduction

UCS uses of an [*abstract syntax tree (AST)*](https://en.wikipedia.org/wiki/Abstract_syntax_tree) during serialization and deserialization, as an intermediate step between the runtime C# object graph and the target data format (e.g. JSON, XML, CSV or CSCD). The AST represents the original C# object graph, with all the information necessary for unambiguous parsing.

Using this intermediate step decouples runtime type handling from data format handling. This means that:
- Support for additional formats can be added without touching type converters. Format codecs only need to understand the AST.
- Support for additional types can be added without having to write conversions to every supported format. Type converters only need to handle specific AST nodes.

A node tree should never contain any cycles. Shared and cyclic reference links are instead represented with address/reference nodes.

## Node Types

UCS recognizes 23 different node types, which are listed below. Each node corresponds to a [CSCD format literal](../README.md#compact-serialized-c-data), and vice-versa.

### Metadata

Metadata nodes contain a value and a single child node. They are meant for annotating other nodes with additional data. There are four types:

- 📌 **Address**: a reference address, used to mark shared or cyclic reference objects. It contains an address name and a child node, which may be of any type except for reference nodes.
- 🏷 **Type**: a type label, used to disambiguify polymorphic types. It contains a type name and a child node.
- 🎯 **Scope**: a class/struct member scope, used to disambiguify shadowed fields and properties. It contains a scope name and a child symbol node. They may only be used inside object nodes as member names.
- 🌐 **Offset**: a UTC time offset, used to add timezone information to a timestamp node. It contains an offset value and a child timestamp node.

### Collections

Collection nodes act as groupings of child nodes. They exist to preserve object structure. There are three types:

- 📜 **List**: a collection node that contains a list of element nodes. Used to encode arrays-like or set-like types. Elements may be nodes of any type (except for scopes).
- 📖 **Dictionary**: a collection node that contains pairs of key/value nodes. Used to encode dictionary-like types. Both keys and values may be nodes of any type (except for scopes).
- 📦 **Object**: a collection node that contains pairs of member name/value nodes. Used to encode arbitrary object instances. Member names must either be a symbol or scope node; member values may be nodes of any type.

### Primitives

Primitive nodes are leaf nodes. They contain a value and do not have any child nodes. There are sixteen types:

- ⬜️ **Null**: encodes a null value.
- ✅️ **Boolean**: encodes a boolean value. Can be either `true` or `false`.
- 🔢  **Integer**: encodes an integer value of arbitrary precision. Can be used for both signed and unsigned integers.
- ➗ **Float**: encodes a floating-point value of arbitrary precision, consisting of a sign, an integer part, a fractional part and an exponent part. The integer, fractional and exponent parts must all be positive values.
- 💲 **Decimal**: encodes a decimal value of arbitrary precision, composed of a sign, mantissa and range. Unlike float nodes, it preserves trailing zeros after the decimal point.
- ♾️ **Infinity**: encodes a positive or negative infinity value, such as `float.PositiveInfinity` and `double.NegativeInfinity`.
- 🚫 **Nan**: encodes a NaN (not-a-number) value, such as `float.NaN` and `double.NaN`.
- 🆎 **Character**: encodes a character value. May be any Unicode character.
- ✒️ **String**: encodes a string value.
- 🔟 **Bytes**: encodes a byte array value; used to store arbitrary binary data.
- 🎨 **Color**: encodes a color value, composed of a red, green, blue and alpha byte.
- 🪪 **UID**: encodes a unique identifier as a 128-bit number.
- 🗓️ **Timestamp**: encodes a timestamp value, composed of a year, month, day, hour, minute and second. The year may be negative, the other terms must be positive.
- ⌛ **Duration**: encodes a duration value, composed of a number of days, hours, minutes, seconds and a sign. All terms must have a positive value.
- ⭐ **Symbol**: encodes enums, named constants (such as the mathematical constants `pi` and `e`) and object member names. It contains a symbol name.
- 🔗 **Reference**: a reference to an address node. Used to avoid duplication of shared references, and cycles in cyclic references. It contains an address name.
