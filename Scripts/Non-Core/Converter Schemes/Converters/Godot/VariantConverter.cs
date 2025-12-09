#if GODOT
using Godot;
using Godot.Collections;
using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Variant converter.
    /// </summary>
    public sealed class VariantConverter : ValueConverter<Variant, INode>
    {
        /* Protected methods. */
        protected override INode ConvertValue(Variant obj, IConverterScheme scheme, SymbolTable table)
        {
            // Convert underlying value (this automatically wraps it in a type node).
            switch (obj.VariantType)
            {
                case Variant.Type.Nil:
                    return new NullNode();
                case Variant.Type.Bool:
                    return ConvertNested(typeof(Variant), obj.AsBool(), scheme);
                case Variant.Type.Int:
                    return ConvertNested(typeof(Variant), obj.AsInt32(), scheme);
                case Variant.Type.Float:
                    return ConvertNested(typeof(Variant), obj.AsDouble(), scheme);
                case Variant.Type.String:
                    return ConvertNested(typeof(Variant), obj.AsString(), scheme);
                case Variant.Type.StringName:
                    return ConvertNested(typeof(Variant), obj.AsStringName(), scheme);
                case Variant.Type.NodePath:
                    return ConvertNested(typeof(Variant), obj.AsNodePath(), scheme);
                case Variant.Type.Color:
                    return ConvertNested(typeof(Variant), obj.AsColor(), scheme);
                case Variant.Type.Vector2:
                    return ConvertNested(typeof(Variant), obj.AsVector2(), scheme);
                case Variant.Type.Vector3:
                    return ConvertNested(typeof(Variant), obj.AsVector3(), scheme);
                case Variant.Type.Vector4:
                    return ConvertNested(typeof(Variant), obj.AsVector4(), scheme);
                case Variant.Type.Vector2I:
                    return ConvertNested(typeof(Variant), obj.AsVector2I(), scheme);
                case Variant.Type.Vector3I:
                    return ConvertNested(typeof(Variant), obj.AsVector3I(), scheme);
                case Variant.Type.Vector4I:
                    return ConvertNested(typeof(Variant), obj.AsVector4I(), scheme);
                case Variant.Type.Quaternion:
                    return ConvertNested(typeof(Variant), obj.AsQuaternion(), scheme);
                case Variant.Type.Plane:
                    return ConvertNested(typeof(Variant), obj.AsPlane(), scheme);
                case Variant.Type.Rect2:
                    return ConvertNested(typeof(Variant), obj.AsRect2(), scheme);
                case Variant.Type.Rect2I:
                    return ConvertNested(typeof(Variant), obj.AsRect2I(), scheme);
                case Variant.Type.Aabb:
                    return ConvertNested(typeof(Variant), obj.AsAabb(), scheme);
                case Variant.Type.Transform2D:
                    return ConvertNested(typeof(Variant), obj.AsTransform2D(), scheme);
                case Variant.Type.Basis:
                    return ConvertNested(typeof(Variant), obj.AsBasis(), scheme);
                case Variant.Type.Transform3D:
                    return ConvertNested(typeof(Variant), obj.AsTransform3D(), scheme);
                case Variant.Type.Projection:
                    return ConvertNested(typeof(Variant), obj.AsProjection(), scheme);
                case Variant.Type.Array:
                    return ConvertNested(typeof(Variant), obj.AsGodotArray(), scheme);
                case Variant.Type.Dictionary:
                    return ConvertNested(typeof(Variant), obj.AsGodotDictionary(), scheme);
                case Variant.Type.Object:
                    return ConvertNested(typeof(Variant), obj.AsGodotObject(), scheme);
                case Variant.Type.PackedByteArray:
                    return ConvertNested(typeof(Variant), obj.AsByteArray(), scheme);
                case Variant.Type.PackedInt32Array:
                    return ConvertNested(typeof(Variant), obj.AsInt32Array(), scheme);
                case Variant.Type.PackedInt64Array:
                    return ConvertNested(typeof(Variant), obj.AsInt64Array(), scheme);
                case Variant.Type.PackedFloat32Array:
                    return ConvertNested(typeof(Variant), obj.AsFloat32Array(), scheme);
                case Variant.Type.PackedFloat64Array:
                    return ConvertNested(typeof(Variant), obj.AsFloat64Array(), scheme);
                case Variant.Type.PackedStringArray:
                    return ConvertNested(typeof(Variant), obj.AsStringArray(), scheme);
                case Variant.Type.PackedColorArray:
                    return ConvertNested(typeof(Variant), obj.AsColorArray(), scheme);
                case Variant.Type.PackedVector2Array:
                    return ConvertNested(typeof(Variant), obj.AsVector2Array(), scheme);
                case Variant.Type.PackedVector3Array:
                    return ConvertNested(typeof(Variant), obj.AsVector3Array(), scheme);
                case Variant.Type.PackedVector4Array:
                    return ConvertNested(typeof(Variant), obj.AsVector4Array(), scheme);
                default:
                    throw new ArgumentException($"Unsupported variant type {obj.VariantType}.");
            }
        }

        protected override Variant DeconvertValue(INode node, IConverterScheme scheme, ParsingTable table)
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
}
#endif