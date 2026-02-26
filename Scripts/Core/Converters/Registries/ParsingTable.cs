using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A reference type symbol table.
    /// </summary>
    public sealed class ParsingTable
    {
        /* Private properties. */
        private Dictionary<string, AddressNode> Addresses { get; } = new Dictionary<string, AddressNode>();
        private Dictionary<string, object> Objects { get; } = new Dictionary<string, object>();

        /* Public methods. */
        /// <summary>
        /// Add an address node to the parsing table.
        /// </summary>
        public void Add(AddressNode node)
        {
            if (Addresses.ContainsKey(node.Name))
                throw new ArgumentException($"Duplicate address '{node.Name}'.");

            Addresses[node.Name] = node;
        }

        /// <summary>
        /// Check whether or not a node with some address has already been parsed.
        /// </summary>
        public bool HasParsed(string address) => Objects.ContainsKey(address);

        /// <summary>
        /// Get the parsed object corresponding to some address.
        /// </summary>
        public object GetParsed(string address)
        {
            if (Objects.TryGetValue(address, out object node))
                return node;
            else
                throw new ArgumentException($"The address '{address}' did not exist.");
        }

        /// <summary>
        /// Set the parsed value of a node.
        /// </summary>
        public void SetParsed(string address, object obj)
        {
            if (!HasParsed(address))
                Objects[address] = obj;
            else
                throw new ArgumentException($"The address '{address}' has already been parsed.");
        }

        /// <summary>
        /// Get the address node correspondind to some address.
        /// </summary>
        public AddressNode GetNode(string address)
        {
            if (Addresses.TryGetValue(address, out AddressNode node))
                return node;
            else
                throw new ArgumentException($"The address '{address}' did not exist.");
        }

        /// <summary>
        /// Clear the parsing table to its default state.
        /// </summary>
        public void Clear()
        {
            Addresses.Clear();
            Objects.Clear();
        }
    }
}