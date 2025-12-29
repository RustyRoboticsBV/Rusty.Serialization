using System.Runtime.CompilerServices;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A base class for matrix converters.
    /// </summary>
    public abstract class MatrixConverter<MatrixT, ElementT> : VectorConverter<MatrixT, ElementT>
    {
        /* Protected methods. */
        /// <summary>
        /// Get the width of the matrix.
        /// </summary>
        protected abstract int GetWidth();

        /// <summary>
        /// Get an x coordinate.
        /// </summary>
        protected int GetX(int index) => index % GetWidth();
        /// <summary>
        /// Get an y coordinate.
        /// </summary>
        protected int GetY(int index) => index / GetWidth();

        /// <summary>
        /// Set the element at some index.
        /// </summary>
        protected sealed override void SetElementAt(ref MatrixT vector, int index, ref ElementT element)
        {
            SetElementAt(ref vector, GetX(index), GetY(index), ref element);
        }

        /// <summary>
        /// Get the element at some index.
        /// </summary>
        protected sealed override ElementT GetElementAt(ref MatrixT vector, int index)
        {
            return GetElementAt(ref vector, GetX(index), GetY(index));
        }

        /// <summary>
        /// Set the element at some index.
        /// </summary>
        protected abstract void SetElementAt(ref MatrixT matrix, int x, int y, ref ElementT element);

        /// <summary>
        /// Set the element at some index.
        /// </summary>
        protected abstract ElementT GetElementAt(ref MatrixT matrix, int x, int y);

    }
}