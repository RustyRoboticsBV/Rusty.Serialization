namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A restricted context used in converter methods.
    /// </summary>
    public abstract class SubContext
    {
        /* Protected properties. */
        protected Converters Context { get; private set; }
        protected ConverterRegistry Converters => Context.ConverterRegistry;
        protected SymbolTable SymbolTable => Context.SymbolTable;
        protected NodeTypeTable NodeTypeTable => Context.NodeTypeTable;
        protected ParsingTable ParsingTable => Context.ParsingTable;

        /* Constructors. */
        protected SubContext(Converters context)
        {
            Context = context;
        }
    }
}