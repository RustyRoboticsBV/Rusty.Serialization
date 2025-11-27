# Supported Type Table

Below you can view the types that have built-in serialization/deserialization support, and what literal types they will serialize to.

## Built-In C# Types
|C#|Serialized|
|-|-|
|bool|bool|
|sbyte|int|
|short|int|
|int|int|
|long|int|
|byte|int|
|ushort|int|
|uint|int|
|ulong|int|
|float|real|
|double|real|
|decimal|real|
|char|char|
|string|string/null|
|array|list/null|
|byte[]|binary/null|
|tuple|list|
|nullable|*varies*|
|enum|int|
|struct|object|
|class|object/null|

## Other .NET Types
### System
|C#|Serialized|
|-|-|
|Half|real|
|Version|string|
|Uri|string|
|Guid|binary|
|DateTime|time|
|TimeSpan|time|

### System.Collections.Generic
|C#|Serialized|
|-|-|
|List<T>|list/null|
|Stack<T>|list/null|
|Queue<T>|list/null|
|LinkedList<T>|list/null|
|HashSet<T>|list/null|
|Dictionary<T,U>|dictionary/null|
|KeyValuePair<T,U>|list/null|

### System.Numerics
|C#|Serialized|
|-|-|
|BigInteger|int|
|Complex|list|
|Vector2|list|
|Vector3|list|
|Vector4|list|
|Quaternion|list|
|Plane|list|
|Matrix3x2|list|
|Matrix4x4|list|

### System.Drawing
|C#|Serialized|
|-|-|
|Color|color|
|Point|list|
|PointF|list|
|Size|list|
|SizeF|list|
|Rectangle|list|
|RectangleF|list|

### System.Text
|C#|Serialized|
|-|-|
|StringBuilder|string|

## Godot Engine Types

|C#|Serialized|
|-|-|
|Variant|*varies*|
|Resource|string/null|
|Array|list/null|
|Array<T>|list/null|
|Dictionary|dictionary/null|
|Dictionary<T,U>|dictionary/null|
|StringName|string|
|NodePath|string|
|Color|color|
|Vector2|list|
|Vector3|list|
|Vector4|list|
|Vector2I|list|
|Vector3I|list|
|Vector4I|list|
|Quaternion|list|
|Plane|list|
|Rect2|list|
|Rect2I|list|
|Aabb|list|
|Transform2D|list|
|Basis|list|
|Transform3D|list|
|Projection|list|