namespace Items
{
    public class SymbolBook : Item
    {
        public Symbol SymbolType;

        public SymbolBook(Symbol symbol)
        {
            SymbolType = symbol;
            Name = SymbolType + " Symbol Book";
            Description = "A book with a " + SymbolType + " on it.";
            CanDrop = true;
            Equipable = true;
        }
    }

    public enum Symbol
    {
        Sun,
        Moon,
        Earth
    }
}
