using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A codec that converts between object graphs and a node tree.
    /// </summary>
    public abstract class ObjectCodec
    {
        /* Internal properties. */
        internal ConverterRegistry Converters { get; private set; }

        internal CollectTypesContext CollectTypesContext { get; private set; }
        internal CreateNodeContext CreateNodeContext { get; private set; }
        internal PopulateNodeContext PopulateNodeContext { get; private set; }
        internal CreateObjectContext CreateObjectContext { get; private set; }
        internal PopulateObjectContext PopulateObjectContext { get; private set; }

        /* Constructors. */
        public ObjectCodec()
        {
            Converters = new ConverterRegistry();

            CollectTypesContext = new CollectTypesContext(this);
            CreateNodeContext = new CreateNodeContext(this);
            PopulateNodeContext = new PopulateNodeContext(this);
            CreateObjectContext = new CreateObjectContext(this);
            PopulateObjectContext = new PopulateObjectContext(this);
        }

        /* Public methods. */
        /// <summary>
        /// Convert an object to a node tree.
        /// </summary>
        public SemanticTree Convert<T>(T obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deconvert a node tree to an object of some type.
        /// </summary>
        public object Deconvert(Type rootType, SemanticTree tree)
        {
            // Create type-annotated tree.
            TypedTree typedTree = new TypedTree(tree);
            CollectTypesContext.TypedTree = typedTree;
            CollectTypesContext.Collect(tree.SyntaxTree.Root, rootType);

            // Create object.
            CreateObjectContext.TypedTree = typedTree;
            return CreateObjectContext.CreateObject(tree.SyntaxTree.Root, rootType);
        }

        /// <summary>
        /// Deconvert a node tree to an object of some type.
        /// </summary>
        public T Deconvert<T>(SemanticTree tree) => (T)Deconvert(typeof(T), tree);
    }
}