# Supported Type Table

Below you can view the types that have built-in serialization/deserialization support, and what literal types they will serialize to.

If an object of a type is serialized that is not in the table below, then the system will serialize using:
- Fields that are public and non-static.
- Properties that are public, non-static and non-readonly.
- Non-public fields and properties with the `[DataMember]` attribute.
- *Unity only*: Non-public fields with the `[SerializeField]` or `[SerializeReference]` attribute.
- *Godot only*: Non-public fields and properties with the `[Export]` attribute.
- **Note**: members with the `[NonSerialized]` attribute are never serialized.

## Built-In C# Types
|C#|.NET|Serialized|Notes|
|-|-|-|-|
|bool|System.Boolean|bool||
|sbyte|System.SByte|int||
|short|System.Int16|int||
|int|System.Int32|int||
|long|System.Int64|int||
|byte|System.Byte|int||
|ushort|System.UInt16|int||
|uint|System.UInt32|int||
|ulong|System.UInt64|int||
|float|System.Single|real||
|double|System.Double|real||
|decimal|System.Decimal|decimal||
|char|System.Char|char||
|string|System.String|string/null||
|T[]|System.Array|list/null||
|byte[]|System.Byte[]|binary/null||
|(...)|System.ValueTuple<...>|list||
|T?|System.Nullable&lt;T&gt;|*varies* or null|Depends on the underlying type|
|enum|System.Enum|symbol|list if annotated with `[Flags]`|
|struct|System.ValueType|object||
|class|System.Object|object/null||

## Other .NET Types
### System
|C#|Serialized|Notes|
|-|-|-|
|Type|string||
|DBNull|null||
|Half|real|.NET 5 or higher|
|Index|int||
|Range|list||
|Version|string||
|Uri|string||
|Guid|binary||
|TimeSpan|int||
|DateTime|time||
|DateTimeOffset|list|
|DateOnly|time|.NET 6 or higher|
|TimeOnly|time|.NET 6 or higher|
|Tuple|list/null||

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
|PriorityQueue<T,U>|dictionary/null|.NET 6 or higher|

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
|C#|Serialized|Notes|
|-|-|-|
|Rune|char|.NET 5 or higher|
|StringBuilder|string||
|Encoding|string||

## Godot Engine Types
*These types are only available when compiling in a Godot context.*

|C#|Serialized|Notes|
|-|-|-|
|Variant|*varies*|Depends on the stored value|
|Resource|object/null|Built-in resources serialize using their resource path instead|
|Array|list/null||
|Array&lt;T&gt;|list/null||
|Dictionary|dictionary/null||
|Dictionary<T,U>|dictionary/null||
|StringName|string||
|NodePath|string||
|Color|color||
|Vector2|list||
|Vector3|list||
|Vector4|list||
|Vector2I|list||
|Vector3I|list||
|Vector4I|list||
|Quaternion|list||
|Plane|list||
|Rect2|list||
|Rect2I|list||
|Aabb|list||
|Transform2D|list||
|Basis|list||
|Transform3D|list||
|Projection|list||

## Unity Engine Types
*These types are only available when compiling in a Unity context.*

|C#|Serialized|
|-|-|
|LayerMask|bool|
|RangeInt|list|
|Vector2|list|
|Vector2Int|list|
|Vector3|list|
|Vector3Int|list|
|Vector4|list|
|Quaternion|list|
|Plane|list|
|Color|color|
|Color32|color|
|Rect|list|
|RectInt|list|
|Bounds|list|
|BoundsInt|list|
|Matrix4x4|list|
|BoundingSphere|list|
|FrustumPlanes|list|
|Ray|list|
|Ray2D|list|
|KeyFrame|object|
|AnimationCurve|object|
