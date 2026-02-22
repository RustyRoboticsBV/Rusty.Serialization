using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A reference type symbol table.
    /// </summary>
    public sealed class SymbolTable
    {
        /* Private properties. */
        private Dictionary<object, INode> Nodes { get; } = new();
        private Dictionary<object, ulong> Addresses { get; } = new();

        private ulong NextAddress { get; set; }

        /* Public methods. */
        public override string ToString()
        {
            string str = "";
            foreach (var address in Addresses)
            {
                if (str.Length > 0)
                    str += '\n';
                str += "- " + address.Value + ": " + address.Key;
            }
            return str;
        }

        /// <summary>
        /// Add an object and its corresponding node representation. The object must be a reference type.
        /// </summary>
        public void Add(object obj)
        {
            if (obj.GetType().IsValueType)
                throw new ArgumentException($"Cannot add objects of value type '{obj.GetType()} to the symbol table!'");

            if (Nodes.ContainsKey(obj))
                throw new ArgumentException($"Object '{obj}' was already in the symbol table!");

            Nodes[obj] = null;
        }

        /// <summary>
        /// Check if an object has been registered in the symbol table.
        /// </summary>
        public bool HasObject(object obj)
        {
            return Nodes.ContainsKey(obj);
        }

        /// <summary>
        /// Return if an object has had an address generated for it.
        /// </summary>
        public bool HasAddressFor(object obj)
        {
            if (obj == null)
                return false;
            return Addresses.ContainsKey(obj);
        }

        /// <summary>
        /// Create an address for an object. The object must be a reference type.
        /// </summary>
        public ulong CreateAddressFor(object obj)
        {
            if (obj.GetType().IsValueType)
                throw new ArgumentException($"Cannot add objects of value type '{obj.GetType()} to the symbol table!'");

            // Create address.
            ulong newAddress = NextAddress;
            Addresses[obj] = newAddress;
            NextAddress++;
            return newAddress;
        }

        /// <summary>
        /// Get the address of an object.
        /// </summary>
        public ulong GetAddressFor(object obj)
        {
            if (Addresses.TryGetValue(obj, out ulong address))
                return address;

            throw new ArgumentException($"No address available for object '{obj}'.");
        }

        /// <summary>
        /// Set the node for an object.
        /// </summary>
        public void SetNode(object obj, INode node)
        {
            Nodes[obj] = node;
        }

        /// <summary>
        /// Try to get the node corresponding to an object.
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
            Addresses.Clear();
            NextAddress = 0;
        }
    }
}