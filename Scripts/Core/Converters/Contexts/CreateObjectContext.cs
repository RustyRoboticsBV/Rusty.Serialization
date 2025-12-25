using System;
using System.Linq;
using System.Reflection;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    public class CreateObjectContext
    {
        /* Public properties. */
        public ConverterTypeRegistry ConverterTypes { get; private set; }
        public ConverterInstanceRegistry ConverterInstances { get; private set; }
        public ParsingTable ParsingTable { get; private set; }

        /* Constructors. */
        public CreateObjectContext(ConverterTypeRegistry converterTypes, ConverterInstanceRegistry instanceTypes, ParsingTable parsingTable)
        {
            ConverterTypes = converterTypes;
            ConverterInstances = instanceTypes;
            ParsingTable = parsingTable;
        }

        /* Public methods. */
        /// <summary>
        /// Deconvert a node into an element.
        /// </summary>
        public T CreateObject<T>(INode node) => (T)CreateObject(typeof(T), node);

        /// <summary>
        /// Deconvert a node into an element.
        /// </summary>
        public object CreateObject(Type expectedType, INode node)
        {
            if (node == null)
                throw new ArgumentException("Cannot deconvert null reference node values.");

            object obj;

            // Do nothing for reference nodes.
            if (node is RefNode refNode)
                return null;

            // Handle ID node.
            if (node is IdNode idNode)
            {
                string idName = idNode.Name;
                obj = CreateObject(expectedType, idNode.Value);
                ParsingTable.Add(idNode);
                ParsingTable.SetParsed(idName, obj);
            }

            // Unwrap type node.
            else if (node is TypeNode typeNode)
            {
                Type underlyingType = new TypeName(typeNode.Name).ToType();
                obj = CreateObject(underlyingType, typeNode.Value);
            }

            // Else, deconvert as-is.
            else
            {
                IConverter converter = ConverterInstances.Get(expectedType);
                if (converter == null)
                {
                    converter = ConverterTypes.Instantiate(expectedType);
                    ConverterInstances.Add(expectedType, converter);
                }

                obj = converter.CreateObject(node, this);
            }

            // If the object is of the correct type, return it.
            if (obj.GetType() == expectedType)
                return obj;

            // Else, check if a conversion operator is available.
            object converted = DoConversionOperator(expectedType, obj);
            if (converted.GetType() == expectedType)
                return converted;

            if (converted == null)
                throw new Exception("The converted value should never be null.");

            throw new InvalidCastException(
                $"Cannot deconvert node value {converted} of actualType {obj?.GetType()} to {expectedType}.");
        }

        /* Private methods. */
        /// <summary>
        /// Try to run an explicit or implicit conversion operator from some value to the target type.
        /// </summary>
        private static object DoConversionOperator(Type targetType, object value)
        {
            if (value == null)
                return null;

            Type sourceType = value.GetType();

            // Look for an implicit operator on the target type
            var method = targetType.GetMethods(
                BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m =>
                    (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                    m.ReturnType == targetType &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType.IsAssignableFrom(sourceType)
                );

            if (method != null)
                return method.Invoke(null, new[] { value });

            // Look for an implicit operator on the source type
            method = sourceType.GetMethods(
                BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m =>
                    (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                    m.ReturnType == targetType &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == sourceType
                );

            if (method != null)
                return method.Invoke(null, new[] { value });

            // No conversion found
            throw new InvalidCastException(
                $"No implicit conversion operator found from {sourceType} to {targetType}.");
        }
    }
}