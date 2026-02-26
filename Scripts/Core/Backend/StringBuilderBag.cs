using System;
using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization.Core.Codecs
{
    public class StringBuilderBag
    {
        /* Fields. */
        private List<StringBuilder> Free { get; set; } = new(1);
        private List<StringBuilder> Rented { get; } = new();

        private readonly object @lock = new object();

        /* Public methods. */
        /// <summary>
        /// Rent a string builder from the bag.
        /// </summary>
        public StringBuilder Rent(int minCapacity = 0)
        {
            lock (@lock)
            {
                if (Free.Count == 0)
                {
                    StringBuilder sb = new StringBuilder(minCapacity);
                    Rented.Add(sb);
                    return sb;
                }
                else
                {
                    StringBuilder renting = Free[Free.Count - 1];
                    renting.Clear();
                    if (renting.Capacity < minCapacity)
                        renting.Capacity = minCapacity;
                    Free.RemoveAt(Free.Count - 1);
                    Rented.Add(renting);
                    return renting;
                }
            }
        }

        /// <summary>
        /// Return a string builder into the bag.
        /// </summary>
        public void Return(StringBuilder sb)
        {
            lock (@lock)
            {
                if (!Rented.Remove(sb))
                    throw new ArgumentException($"The string builder {sb} was not rented from this bag.");
                Free.Add(sb);
            }
        }

        /// <summary>
        /// Delete all free string builders. Rented string builders are not deleted.
        /// </summary>
        public void Clear()
        {
            lock (@lock)
            {
                Free = new List<StringBuilder>();
            }
        }
    }
}