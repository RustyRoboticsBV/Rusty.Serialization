#if GODOT
using Godot;
using Godot.Collections;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;
using ArgumentException = System.ArgumentException;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot string name converter.
    /// </summary>
    public class VariantConverter : Converter<Variant, INode>
    {
        /* Protected method. */
        protected override INode CreateNode(Variant obj, CreateNodeContext context)
        {
            switch (obj.VariantType)
            {
                case Variant.Type.Nil:
                    return new NullNode();
                case Variant.Type.Bool:
                    return context.CreateNode(typeof(Variant), (bool)obj);
                case Variant.Type.Int:
                    return context.CreateNode(typeof(Variant), (int)obj);
                case Variant.Type.Float:
                    return context.CreateNode(typeof(Variant), (float)obj);
                case Variant.Type.String:
                    return context.CreateNode(typeof(Variant), (string)obj);
                case Variant.Type.Vector2:
                    return context.CreateNode(typeof(Variant), (Vector2)obj);
                case Variant.Type.Vector2I:
                    return context.CreateNode(typeof(Variant), (Vector2I)obj);
                case Variant.Type.Rect2:
                    return context.CreateNode(typeof(Variant), (Rect2)obj);
                case Variant.Type.Rect2I:
                    return context.CreateNode(typeof(Variant), (Rect2I)obj);
                case Variant.Type.Vector3:
                    return context.CreateNode(typeof(Variant), (Vector3)obj);
                case Variant.Type.Vector3I:
                    return context.CreateNode(typeof(Variant), (Vector3I)obj);
                case Variant.Type.Transform2D:
                    return context.CreateNode(typeof(Variant), (Transform2D)obj);
                case Variant.Type.Vector4:
                    return context.CreateNode(typeof(Variant), (Vector4)obj);
                case Variant.Type.Vector4I:
                    return context.CreateNode(typeof(Variant), (Vector4I)obj);
                case Variant.Type.Plane:
                    return context.CreateNode(typeof(Variant), (Plane)obj);
                case Variant.Type.Quaternion:
                    return context.CreateNode(typeof(Variant), (Quaternion)obj);
                case Variant.Type.Aabb:
                    return context.CreateNode(typeof(Variant), (Aabb)obj);
                case Variant.Type.Basis:
                    return context.CreateNode(typeof(Variant), (Basis)obj);
                case Variant.Type.Transform3D:
                    return context.CreateNode(typeof(Variant), (Transform3D)obj);
                case Variant.Type.Projection:
                    return context.CreateNode(typeof(Variant), (Projection)obj);
                case Variant.Type.Color:
                    return context.CreateNode(typeof(Variant), (Color)obj);
                case Variant.Type.StringName:
                    return context.CreateNode(typeof(Variant), (StringName)obj);
                case Variant.Type.NodePath:
                    return context.CreateNode(typeof(Variant), (NodePath)obj);
                case Variant.Type.Object:
                    return context.CreateNode(typeof(Variant), (GodotObject)obj);
                case Variant.Type.Dictionary:
                    return context.CreateNode(typeof(Variant), (Dictionary)obj);
                case Variant.Type.Array:
                    return context.CreateNode(typeof(Variant), (Array)obj);
                case Variant.Type.PackedByteArray:
                    return context.CreateNode(typeof(Variant), (byte[])obj);
                case Variant.Type.PackedInt32Array:
                    return context.CreateNode(typeof(Variant), (int[])obj);
                case Variant.Type.PackedInt64Array:
                    return context.CreateNode(typeof(Variant), (long[])obj);
                case Variant.Type.PackedFloat32Array:
                    return context.CreateNode(typeof(Variant), (float[])obj);
                case Variant.Type.PackedFloat64Array:
                    return context.CreateNode(typeof(Variant), (double[])obj);
                case Variant.Type.PackedStringArray:
                    return context.CreateNode(typeof(Variant), (string[])obj);
                case Variant.Type.PackedVector2Array:
                    return context.CreateNode(typeof(Variant), (Vector2[])obj);
                case Variant.Type.PackedVector3Array:
                    return context.CreateNode(typeof(Variant), (Vector3[])obj);
                case Variant.Type.PackedColorArray:
                    return context.CreateNode(typeof(Variant), (Color[])obj);
                case Variant.Type.PackedVector4Array:
                    return context.CreateNode(typeof(Variant), (Vector4[])obj);
                default:
                    throw new ArgumentException("Cannot serialize Variants with type " + obj.VariantType);
            }
        }

        protected override Variant CreateObject(INode node, CreateObjectContext context)
        {
            throw new ArgumentException("Variant's create object method should never be reached.");
        }
    }
}
#endif