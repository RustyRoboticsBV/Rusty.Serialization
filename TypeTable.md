# Supported Type Table

Below you can view the types that have built-in serialization/deserialization support, and what literal types they will serialize to.

## Built-In C# Types
|C#|.NET|Serialized|
|-|-|-|
|bool|System.Boolean|bool|
|sbyte|System.SByte|int|
|short|System.Int16|int|
|int|System.Int32|int|
|long|System.Int64|int|
|byte|System.Byte|int|
|ushort|System.UInt16|int|
|uint|System.UInt32|int|
|ulong|System.UInt64|int|
|float|System.Single|real|
|double|System.Double|real|
|decimal|System.Decimal|real|
|char|System.Char|char|
|string|System.String|string/null|
|array|T[]|list/null|
|byte[]|System.Byte[]|binary/null|
|tuple|System.ValueTuple<...>|list|
|nullable|System.Nullable&lt;T&gt;|*varies*|
|enum|System.Enum|int|
|struct|System.ValueType|object|
|class|System.Object|object/null|

## Other .NET Types
### System
|C#|Serialized|Notes|
|-|-|-|
|Type|string||
|DBNull|null||
|Half|real||
|Index|int||
|Range|list||
|Version|string||
|Uri|string||
|Guid|binary||
|TimeSpan|time||
|DateTime|time||
|DateTimeOffset|list|
|Tuple<...>|list/null|
|DateOnly|time|C# 10 or higher|
|TimeOnly|time|C# 10 or higher|

### System.Collections
|C#|Serialized|
|-|-|
|BitArray|binary/null|

### System.Collections.Generic
|C#|Serialized|Notes|
|-|-|-|
|List&lt;T&gt;|list/null||
|Stack&lt;T&gt;|list/null||
|Queue&lt;T&gt;|list/null||
|LinkedList&lt;T&gt;|list/null||
|HashSet&lt;T&gt;|list/null||
|Dictionary<T,U>|dictionary/null||
|KeyValuePair<T,U>|list/null||
|SortedSet&lt;T&gt;|list/null||
|SortedList<T,U>|dictionary/null||
|SortedDictionary<T,U>|dictionary/null||
|PriorityQueue<T,U>|dictionary/null|C# 10 or higher|

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
|Rune|char|
|StringBuilder|string|
|Encoding|string|

## Godot Engine Types
*These types are only available when compiling in a Godot context.*

|C#|Serialized|
|-|-|
|Variant|*varies*|
|Resource|object/null|
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

The following resource types are serialized using their resource paths:
|Resource|
|-|
|Godot.PackedScene|
|Godot.Sky|
|Godot.Environment|
|Godot.Texture|
|Godot.Image|
|Godot.SpriteFrames|
|Godot.Material|
|Godot.PhysicsMaterial|
|Godot.Mesh|
|Godot.MeshLibrary|
|Godot.NavigationMesh|
|Godot.Font|
|Godot.Theme|
|Godot.Animation|
|Godot.AnimationLibrary|
|Godot.AudioStream|
|Godot.Script|
|Godot.Shader|
