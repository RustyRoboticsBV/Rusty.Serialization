using System.Numerics;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET quaternion converter.
    /// </summary>
    public class QuaternionConverter : VectorConverter<Quaternion, float>
    {
        /* Protected method. */
        protected override int GetLength() => 4;

        protected override float GetElementAt(ref Quaternion vector, int index)
        {
            return vector[index];
        }

        protected override void SetElementAt(ref Quaternion vector, int index, ref float element)
        {
            vector[index] = element;
        }
    }
}