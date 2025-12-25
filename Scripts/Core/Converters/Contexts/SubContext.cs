namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A restricted context used in converter methods.
    /// </summary>
    public abstract class SubContext
    {
        /* Protected properties. */
        protected ConversionContext Context { get; private set; }
        protected ConverterRegistry Converters => Context.Converters;
        protected SymbolTable SymbolTable => Context.SymbolTable;
        protected NodeTypeTable NodeTypeTable => Context.NodeTypeTable;

        /* Constructors. */
        protected SubContext(ConversionContext context)
        {
            Context = context;
        }
    }
}