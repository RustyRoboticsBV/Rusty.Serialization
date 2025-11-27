#if GODOT
using Godot;
using Godot.Collections;
using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters.Gd;

/// <summary>
/// A Godot.Variant converter.
/// </summary>
public sealed class VariantConverter : ValueConverter<Variant, INode>
{
    /* Protected methods. */
    protected override INode Convert(Variant obj, Context context)
    {
        // Convert underlying value (this automatically wraps it in a type node).
        switch (obj.VariantType)
        {
            case Variant.Type.Nil:
                return new NullNode();
            case Variant.Type.Bool:
                return ConvertElement(typeof(Variant), obj.AsBool(), context);
            case Variant.Type.Int:
                return ConvertElement(typeof(Variant), obj.AsInt32(), context);
            case Variant.Type.Float:
                return ConvertElement(typeof(Variant), obj.AsDouble(), context);
            case Variant.Type.String:
                return ConvertElement(typeof(Variant), obj.AsString(), context);
            case Variant.Type.StringName:
                return ConvertElement(typeof(Variant), obj.AsStringName(), context);
            case Variant.Type.NodePath:
                return ConvertElement(typeof(Variant), obj.AsNodePath(), context);
            case Variant.Type.Color:
                return ConvertElement(typeof(Variant), obj.AsColor(), context);
            case Variant.Type.Vector2:
                return ConvertElement(typeof(Variant), obj.AsVector2(), context);
            case Variant.Type.Vector3:
                return ConvertElement(typeof(Variant), obj.AsVector3(), context);
            case Variant.Type.Vector4:
                return ConvertElement(typeof(Variant), obj.AsVector4(), context);
            case Variant.Type.Vector2I:
                return ConvertElement(typeof(Variant), obj.AsVector2I(), context);
            case Variant.Type.Vector3I:
                return ConvertElement(typeof(Variant), obj.AsVector3I(), context);
            case Variant.Type.Vector4I:
                return ConvertElement(typeof(Variant), obj.AsVector4I(), context);
            case Variant.Type.Rect2:
                return ConvertElement(typeof(Variant), obj.AsRect2(), context);
            case Variant.Type.Rect2I:
                return ConvertElement(typeof(Variant), obj.AsRect2I(), context);
            case Variant.Type.Aabb:
                return ConvertElement(typeof(Variant), obj.AsAabb(), context);
            case Variant.Type.Transform2D:
                return ConvertElement(typeof(Variant), obj.AsTransform2D(), context);
            case Variant.Type.Basis:
                return ConvertElement(typeof(Variant), obj.AsBasis(), context);
            case Variant.Type.Transform3D:
                return ConvertElement(typeof(Variant), obj.AsTransform3D(), context);
            case Variant.Type.Projection:
                return ConvertElement(typeof(Variant), obj.AsProjection(), context);
            case Variant.Type.Array:
                return ConvertElement(typeof(Variant), obj.AsGodotArray(), context);
            case Variant.Type.Dictionary:
                return ConvertElement(typeof(Variant), obj.AsGodotDictionary(), context);
            case Variant.Type.Object:
                return ConvertElement(typeof(Variant), obj.AsGodotObject(), context);
            case Variant.Type.PackedByteArray:
                return ConvertElement(typeof(Variant), obj.AsByteArray(), context);
            case Variant.Type.PackedInt32Array:
                return ConvertElement(typeof(Variant), obj.AsInt32Array(), context);
            case Variant.Type.PackedInt64Array:
                return ConvertElement(typeof(Variant), obj.AsInt64Array(), context);
            case Variant.Type.PackedFloat32Array:
                return ConvertElement(typeof(Variant), obj.AsFloat32Array(), context);
            case Variant.Type.PackedFloat64Array:
                return ConvertElement(typeof(Variant), obj.AsFloat64Array(), context);
            case Variant.Type.PackedStringArray:
                return ConvertElement(typeof(Variant), obj.AsStringArray(), context);
            case Variant.Type.PackedColorArray:
                return ConvertElement(typeof(Variant), obj.AsColorArray(), context);
            case Variant.Type.PackedVector2Array:
                return ConvertElement(typeof(Variant), obj.AsVector2Array(), context);
            case Variant.Type.PackedVector3Array:
                return ConvertElement(typeof(Variant), obj.AsVector3Array(), context);
            case Variant.Type.PackedVector4Array:
                return ConvertElement(typeof(Variant), obj.AsVector4Array(), context);
            default:
                throw new ArgumentException($"Unsupported variant type {obj.VariantType}.");
        }
    }

    protected override Variant Deconvert(INode node, Context context)
    {
        return FromNode(node);
    }

    /* Private methods. */
    private static Variant FromNode(INode node)
    {
        switch (node)
        {
            case NullNode:
                return new();
            case BoolNode b:
                return Variant.From(b.Value);
            case IntNode integer:
                if (integer.Value < 0)
                    return Variant.From((long)integer.Value);
                else
                    return Variant.From((ulong)integer.Value);
            case RealNode real:
                return Variant.From((double)real.Value);
            case CharNode character:
                return Variant.From(character.Value);
            case StringNode str:
                return Variant.From(str.Value);
            case ColorNode color:
                return Variant.From(new Color(
                    color.R * 255f,
                    color.G * 255f,
                    color.B * 255f,
                    color.A * 255f));
            case TimeNode time:
                return Variant.From(time.Value.ToString());
            case BinaryNode binary:
                return Variant.From(binary.Value);
            case ListNode list:
                Array<Variant> array = new();
                for (int i = 0; i < list.Elements.Length; i++)
                {
                    array.Add(FromNode(list.Elements[i]));
                }
                return Variant.From(array);
            case DictNode dict:
                Dictionary<Variant, Variant> pairs = new();
                for (int i = 0; i < dict.Pairs.Length; i++)
                {
                    Variant key = FromNode(dict.Pairs[i].Key);
                    Variant value = FromNode(dict.Pairs[i].Value);
                    pairs.Add(key, value);
                }
                return Variant.From(pairs);
            case ObjectNode obj:
                Dictionary<string, Variant> members = new();
                for (int i = 0; i < obj.Members.Length; i++)
                {
                    string key = obj.Members[i].Key;
                    Variant value = FromNode(obj.Members[i].Value);
                    members.Add(key, value);
                }
                return Variant.From(members);
            case TypeNode type:
                return FromNode(type.Value);
            default:
                throw new ArgumentException($"Unknown node type '{node.GetType()}'.");
        }
    }
}
#endif