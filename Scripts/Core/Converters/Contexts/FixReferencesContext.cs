using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    public class FixReferencesContext
    {
        /* Public properties. */
        public ConverterTypeRegistry ConverterTypes { get; private set; }
        public ConverterInstanceRegistry ConverterInstances { get; private set; }
        public ParsingTable ParsingTable { get; private set; }

        /* Constructors. */
        public FixReferencesContext(ConverterTypeRegistry converterTypes, ConverterInstanceRegistry instanceTypes, ParsingTable parsingTable)
        {
            ConverterTypes = converterTypes;
            ConverterInstances = instanceTypes;
            ParsingTable = parsingTable;
        }

        /* Public methods. */
        /// <summary>
        /// Fix the missing reference values of an object.
        /// </summary>
        public object FixReferences(object obj, INode node)
        {
            Type type = obj.GetType();

            // Handle ID node.
            if (node is IdNode idNode)
                return FixReferences(obj, idNode.Value);

            // Unwrap type node.
            else if (node is TypeNode typeNode)
                return FixReferences(obj, typeNode.Value);

            // Else, fix as-is.
            else
            {
                IConverter converter = ConverterInstances.Get(type);
                if (converter == null)
                {
                    converter = ConverterTypes.Instantiate(type);
                    ConverterInstances.Add(type, converter);
                }

                if (converter is ICompositeConverter composite)
                    return composite.FixReferences(obj, node, this);
                else
                    return obj;
            }
        }

        /// <summary>
        /// Get the reference type object corresponding to an ID.
        /// </summary>
        public object GetReference(string name)
        {
            return ParsingTable.GetParsed(name);
        }
    }
}