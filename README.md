# Compact Serialized C# Data
A C# serialization/deserialization module, with support for built-in C# types and Unity and Godot structs.

The module uses a custom serialization format, called Compact Serialized C# Data (CSCD). It's designed to be compact, general and parsable, while maintaining decent readability. See the specification document [here](FormatSpecification.md).

## Type Support
The following built-in C# types can be serialized and deserialized:
- Booleans: `bool`.
- All integer types: `sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`.
- All real types: `float`, `double`, `decimal`.
- All text types: `char`, `string`.
- Enum types.
- Array types.
- Tuple types.
- `System.Collections.Generic` types: `List<T>`, `Dictionary<T,U>`.
- `System.Drawing.Color` struct.
- User-defined `class` and `struct` types. Generic types are supported.

When used in an Unity context, the following types have explicit support:
- All color types: `Color` and `Color32`.

When used in a Godot context, the following types have explicit support:
- `Variant`.
- String types: `StringName` and `NodePath`.
- `Color`.
- Vectors types: `Vector2`, `Vector3`, `Vector4`, `Vector2I`, `Vector3I`, `Vector4I`.
- Rect types: `Rect2`, `Rect2I`, `Aabb`.
- Matrix types: `Transform2D`, `Basis`, `Transform3D`, `Projection`.
- Collection types: `Array`, `Array<T>`, `Dictionary`, `Dictionary<T,U>`.