using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    public sealed class CreateObjectContext : Context
    {
        /* Public properties. */
        public Dictionary<INode, object> Objects { get; } = new Dictionary<INode, object>();
        public TypedTree TypedTree { get; set; }

        /* Constructors. */
        public CreateObjectContext(ObjectCodec codec) : base(codec) { }

        /* Public methods. */
        public object CreateObject(INode node, Type type)
        {
            object obj = null;

            // Handle ref nodes.
            if (node is RefNode @ref)
            {
                AddressNode refAddress = TypedTree.SemanticTree.GetAddress(@ref);
                if (Objects.TryGetValue(refAddress, out object addressed))
                    obj = addressed;
                else
                    obj = CreateObject(refAddress, TypedTree.GetUnderlyingType(@ref));
                Objects.Add(node, obj);
            }

            // Handle address nodes.
            else if (node is AddressNode address)
            {
                obj = CreateObject(address.Child, type);
                Objects.Add(node, obj);
            }

            // Handle type nodes.
            // TODO: Handle type aliasses.
            else if (node is TypeNode typeNode)
            {
                type = new TypeName(typeNode.Name);
                obj = CreateObject(typeNode.Child, type);
                Objects.Add(node, obj);
            }

            // Handle other metadata nodes.
            else if (node is IMetadataNode metadata)
            {
                obj = CreateObject(metadata.Child, type);
                Objects.Add(node, obj);
            }

            // Handle other nodes.
            else
            {
                Converter converter = Codec.Converters.Get(type);
                obj = converter.CreateObject(node, this);
                Objects.Add(node, obj);
                obj = converter.PopulateObject(node, obj, Codec.PopulateObjectContext);
                Objects[node] = obj;
            }

            return obj;
        }
    }
}