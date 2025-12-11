using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A reference type symbol table.
    /// </summary>
    public sealed class ParsingTable
    {
        /* Private properties. */
        private Dictionary<string, IdNode> Ids { get; } = new();
        private Dictionary<string, object> Objects { get; } = new();

        /* Public methods. */
        /// <summary>
        /// Add an ID node to the parsing table.
        /// </summary>
        public void Add(IdNode node)
        {
            if (Ids.ContainsKey(node.Name))
                throw new ArgumentException($"Duplicate ID '{node.Name}'.");

            Ids[node.Name] = node;
            Objects[node.Name] = null;
        }

        /// <summary>
        /// Check whether or not a node with some ID has already been parsed.
        /// </summary>
        public bool HasParsed(string id) => GetParsed(id) != null;

        /// <summary>
        /// Get the parsed object corresponding to some ID.
        /// </summary>
        public object GetParsed(string id)
        {
            if (Objects.TryGetValue(id, out object node))
                return node;
            else
                throw new ArgumentException($"The id '{id}' did not exist.");
        }

        /// <summary>
        /// Set the parsed value of a node.
        /// </summary>
        public void SetParsed(string id, object obj)
        {
            if (!HasParsed(id))
                Objects[id] = obj;
            else
                throw new ArgumentException($"The id '{id}' has already been parsed.");
        }

        /// <summary>
        /// Get the ID node correspondind to some ID.
        /// </summary>
        public IdNode GetNode(string id)
        {
            if (Ids.TryGetValue(id, out IdNode node))
                return node;
            else
                throw new ArgumentException($"The id '{id}' did not exist.");
        }

        /// <summary>
        /// Clear the parsing table to its default state.
        /// </summary>
        public void Clear()
        {
            Ids.Clear();
            Objects.Clear();
        }
    }
}