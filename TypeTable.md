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
|C#|Serialized|Notes|
|-|-|-|
|Half|real||
|Index|int||
|Range|list||
|Version|string||
|Uri|string||
|Guid|binary||
|TimeSpan|time||
|DateTime|time||
|DateTimeOffset|list|
|DateOnly|time|C# 10 or higher|
|TimeOnly|time|C# 10 or higher|

### System.Collections.Generic
|C#|Serialized|
|-|-|
|List&lt;T&gt;|list/null|
|Stack&lt;T&gt;|list/null|
|Queue&lt;T&gt;|list/null|
|LinkedList&lt;T&gt;|list/null|
|HashSet&lt;T&gt;|list/null|
|Dictionary<T,U>|dictionary/null|
|KeyValuePair<T,U>|list/null|
|SortedSet&lt;T&gt;|list/null|
|SortedList<T,U>|dictionary/null|
|SortedDictionary<T,U>|dictionary/null|

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
*These types are only available when compiling in a Godot context.*

|C#|Serialized|
|-|-|
|Variant|*varies*|
|Resource|string/null|
|Array|list/null|
|Array&lt;T&gt;|list/null|
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