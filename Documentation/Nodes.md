# Nodes

A node tree data structure represent a C# object graph. A node tree should never contain any cycles.

The node layer recognizes the following nodes listed below. Each node corresponds to a CSCD format literal, and vice-versa.

### Metadata

- <img src="../Images/Icons/Address.svg" width="16">  **Address**: a reference address, used to mark shared or cyclic reference objects.
- <img src="../Images/Icons/Type.svg" width="16">  **Type**: a type label, used to disambiguify polymorphic types.
- <img src="../Images/Icons/Scope.svg" width="16">  **Scope**: a class/struct member scope, used to disambiguify shadowed fields and properties.
- <img src="../Images/Icons/Offset.svg" width="16">  **Offset**: a UTC time offset, used to add timezone information to a timestamp node.

### Primitives

- <img src="../Images/Icons/Null.svg" width="16">  **Null**: encodes a null value in an object graph.
- <img src="../Images/Icons/Boolean.svg" width="16">  **Bool**: encodes a boolean value. Can be either `true` or `false`.
- <img src="../Images/Icons/Integer.svg" width="16">  **Int**: encodes an integer value of arbitrary precision. Can be used for both signed and unsigned integers.
- <img src="../Images/Icons/Float.svg" width="16">  **Float**: encodes a floating-point value of arbitrary precision.
- <img src="../Images/Icons/Infinity.svg" width="16">  **Infinity**: encodes a positive or negative infinity value, such as `float.PositiveInfinity` and `double.NegativeInfinity`.
- <img src="../Images/Icons/NaN.svg" width="16">  **Nan**: encodes a NaN value, such as `float.NaN` and `double.NaN`.
- <img src="../Images/Icons/Character.svg" width="16">  **Char**: encodes a character value. May be any Unicode character.
- <img src="../Images/Icons/String.svg" width="16">  **String**: encodes a string value.
- <img src="../Images/Icons/Decimal.svg" width="16">  **Decimal**: encodes a decimal value of arbitrary precision, composed of a sign, mantissa and range. Unlike float nodes, it preserves trailing zeros after the decimal point.
- <img src="../Images/Icons/Color.svg" width="16">  **Color**: encodes a color value, composed of a red, green, blue and alpha byte.
- <img src="../Images/Icons/UID.svg" width="16">  **UID**: encodes a unique identifier as a 128-bit number.
- <img src="../Images/Icons/Timestamp.svg" width="16">  **Timestamp**: encodes a timestamp value, composed of a year, month, day, hour, minute and second. The year may be negative, the other terms must be positive.
- <img src="../Images/Icons/Duration.svg" width="16">  **Duration**: encodes a duration value, composed of a number of days, hours, minutes, seconds and a sign. All terms must be positive.
- <img src="../Images/Icons/Bytes.svg" width="16">  **Bytes**: encodes a byte array value; used to store arbitrary binary data.
- <img src="../Images/Icons/Symbol.svg" width="16">  **Symbol**: encodes enums and special constants (such as the mathematical constants `pi` and `e`).
- <img src="../Images/Icons/Reference.svg" width="16">  **Ref**: a reference to an address node. Used to avoid duplication of shared references, and cycles in cyclic references.

### Collections

- <img src="../Images/Icons/List.svg" width="16">  **List**: a collection node that contains a list of element nodes. Used to encode arrays-like or set-like types.
- <img src="../Images/Icons/Dictionary.svg" width="16">  **Dict**: a collection node that contains pairs of key/value nodes. Used to encode dictionary-like types.
- <img src="../Images/Icons/Object.svg" width="16">  **Object**: a collection node that contains pairs of member name strings and member value nodes. Used to encode arbitrary object instances.
