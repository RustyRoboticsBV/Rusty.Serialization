# Compact Serialized C# Data
A C# serialization/deserialization module, with support for built-in C# types, as well as common API types from the Godot and Unity game engines.

The module uses a custom serialization format, called Compact Serialized C# Data (CSCD). It's designed to be compact, general and parsable, while maintaining decent readability. See the specification document [here](FormatSpecification.md).

## Type Support
The following built-in C# types can be serialized and deserialized:
- `bool`.
- All integer types: `sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`.
- All real types: `float`, `double`, `decimal`.
- All text types: `char`, `string`.
- Arrays.
- Any `enum` type (serialized as integers).
- Any `class` or `struct`. This will serialize all fields and properties that are public, non-static, non-readonly.
  -  Generic and nested types are supported.
  -  Polymorphic member types are supported.
- Tuples.

### System Library
The following `System` types have explicit support:
- `System`: `DateTime`, `TimeSpan`, `Guid`, `Uri`.
- `System.Collections.Generic`: `List<T>`, `LinkedList<T>`, `Stack<T>`, `Queue<T>`, `HashSet<T>`, `Dictionary<T,U>`, `KeyValuePair<T,U>`.
- `System.Numerics`: `BigInteger`, `Vector2`, `Vector3`, `Vector4`, `Quaternion`, `Plane`, `Matrix3x2`, `Matrix4x4`.
- `System.Drawing`: `Color`, `Point`, `PointF`, `Rectangle`, `RectangleF`, `Size`, `SizeF`.
- `System.Text`: `StringBuilder`.

### Unity
When used in an Unity context, the following types have explicit support:
- All color types: `Color` and `Color32`.

### Godot
When used in a Godot context, the following types have explicit support:
- `Variant`.
- `Resource` (serializes as a resource path).
- String types: `StringName` and `NodePath`.
- `Color`.
- Vectors types: `Vector2`, `Vector3`, `Vector4`, `Vector2I`, `Vector3I`, `Vector4I`.
- Rect types: `Rect2`, `Rect2I`, `Aabb`.
- Matrix types: `Transform2D`, `Basis`, `Transform3D`, `Projection`.
- Collection types: `Array`, `Array<T>`, `Dictionary`, `Dictionary<T,U>`.