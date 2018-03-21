using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    public class KeyBody : CraftingComponent
    {
        public KeyBody()
        {
            Name = "Key Body";
            Description = "The body of a key. It's too short to be used in a lock.";
            Equipable = false;
        }
    }
}
