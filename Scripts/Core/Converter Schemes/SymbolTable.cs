using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A reference type symbol table.
    /// </summary>
    public sealed class SymbolTable
    {
        /* Private properties. */
        private Dictionary<object, INode> Nodes { get; } = new();
        private Dictionary<object, ulong> Ids { get; } = new();

        private ulong NextId { get; set; }

        /* Public methods. */
        public override string ToString()
        {
            string str = "";
            foreach (var id in Ids)
            {
                if (str.Length > 0)
                    str += '\n';
                str += "- " + id.Value + ": " + id.Key;
            }
            return str;
        }

        /// <summary>
        /// Add an object and its corresponding node representation.
        /// </summary>
        public void Add(object obj, INode node)
        {
            if (obj.GetType().IsValueType)
                throw new ArgumentException($"Cannot add objects of value type '{obj.GetType()} to the symbol table!'");

            if (Nodes.ContainsKey(obj))
                throw new ArgumentException($"Object '{obj}' was already in the symbol table!");

            Nodes[obj] = node;
        }

        /// <summary>
        /// Check if an object has been registered in the symbol table.
        /// </summary>
        public bool HasObject(object obj)
        {
            return Nodes.ContainsKey(obj);
        }

        /// <summary>
        /// Return if an object has had an ID generated for it.
        /// </summary>
        public bool HasIdFor(object obj)
        {
            System.Console.WriteLine(obj + " has key? " + Ids.ContainsKey(obj));
            return Ids.ContainsKey(obj);
        }

        /// <summary>
        /// Get the ID of an object.
        /// </summary>
        public ulong GetOrCreateId(object obj)
        {
            if (obj.GetType().IsValueType)
                throw new ArgumentException($"Cannot add objects of value type '{obj.GetType()} to the symbol table!'");

            if (!Nodes.ContainsKey(obj))
                throw new ArgumentException($"Cannot get ID for unregistered object '{obj}'.");

            if (Ids.TryGetValue(obj, out ulong id))
                return id;

            // Create new ID.
            ulong newId = NextId;
            Ids[obj] = newId;
            NextId++;
            return newId;
        }

        /// <summary>
        /// Try to get the node corresponding to an object. Returns false if the object was not found.
        /// </summary>
        public INode GetNode(object obj)
        {
            return Nodes[obj];
        }

        /// <summary>
        /// Clear the symbol table to its default state.
        /// </summary>
        public void Clear()
        {
            Nodes.Clear();
            Ids.Clear();
            NextId = 0;
        }
    }
}