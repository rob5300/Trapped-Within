using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    public class KeyHead : CraftingComponent
    {
        public string CraftedKeyId = "Default";

        public KeyHead()
        {
            Name = "Key Head";
            Description = "The head of a key. The rest has broken off.";
            Equipable = false;
        }

        public KeyHead(string craftedKeyId) : this()
        {
            CraftedKeyId = craftedKeyId;
        }
    }
}
