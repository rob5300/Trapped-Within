namespace Items
{
    public class ItemSlot
    {
        public int Number;
        public Item Item;

        public ItemSlot(int slotnumber)
        {
            Number = slotnumber;
            Item = null;
        }
    }
}
