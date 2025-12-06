#if RUSTY_DEBUG
using System;

namespace Rusty.Serialization.Testing
{
    /// <summary>
    /// Unit test input object.
    /// </summary>
    public readonly struct UnitTestInput<T>
    {
        /* Fields. */
        private readonly object value;

        /* Public properties. */
        public static UnitTestInput<T> Null => new((object)null);
        public static UnitTestInput<T> Throw => new(Testing.Throw.Exception);

        public T Value => value is T typed ? typed : default;
        public Exception Exception => value is Exception ex ? ex : null;
        public bool IsException => value is Throw || value is Exception;
        public Type Type => value.GetType();

        /* Constructors. */
        private UnitTestInput(object obj)
        {
            value = obj;
        }

        public UnitTestInput(T obj) : this((object)obj) { }
        public UnitTestInput(Throw obj) : this((object)obj) { }
        public UnitTestInput(Exception ex) : this((object)ex) { }

        /* Conversion operators. */
        public static implicit operator T(UnitTestInput<T> input) => input.Value;
        public static implicit operator UnitTestInput<T>(T exception) => new(exception);
        public static implicit operator UnitTestInput<T>(Throw exception) => new(exception);

        /* Comparison operators. */
        public static bool operator ==(UnitTestInput<T> a, UnitTestInput<T> b) => a.Equals(b);
        public static bool operator !=(UnitTestInput<T> a, UnitTestInput<T> b) => !a.Equals(b);

        /* Public methods. */
        public override readonly string ToString() => value?.ToString() ?? "";
        public override readonly bool Equals(object obj) => obj is UnitTestInput<T> typed && Equals(typed);
        public readonly bool Equals(UnitTestInput<T> obj)
        {
            if (value is Array arrA && obj.value is Array arrB)
            {
                if (arrA.Length != arrB.Length)
                    return false;
                for (int i = 0; i < arrA.Length; i++)
                {
                    object a = arrA.GetValue(i);
                    object b = arrB.GetValue(i);
                    if (!a.Equals(b))
                        return false;
                }
                return true;
            }
            else
                return value.Equals(obj.value);
        }
        public readonly override int GetHashCode() => value != null ? value.GetHashCode() : 0;
    }
}
#endif