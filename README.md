# Compact Serialized C# Data
A C# serialization/deserialization module, with support for built-in C# types, as well as common API types from the Godot and Unity game engines. It's primarily meant for engine-agnostic tool development.

The module uses a custom serialization format, called Compact Serialized C# Data (CSCD), a JSON-like format that supports type labels and a wider variety of data types. It's designed to be compact, general and relatively easy to parse. See the specification document [here](FormatSpecification.md).

## Type Support
Various types have built-in serializers.

### Built-In C# Types
- `bool`.
- All integer types: `sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`.
- All real types: `float`, `double`, `decimal`.
- All text types: `char`, `string`.
- Arrays.
- Any `enum` type (serialized as integers).
- Any `class` or `struct`. This will serialize all fields and properties that are public, non-static, non-readonly.
  -  Generic and nested types are supported.
  -  Polymorphic members are supported.
- Tuples.

### System
- `System`: `Half`, `DateTime`, `TimeSpan`, `Guid`, `Uri`.
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
- `Quaternion`
- `Plane`
- Rect types: `Rect2`, `Rect2I`, `Aabb`.
- Matrix types: `Transform2D`, `Basis`, `Transform3D`, `Projection`.
- Collection types: `Array`, `Array<T>`, `Dictionary`, `Dictionary<T,U>`.
