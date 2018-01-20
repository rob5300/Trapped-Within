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
            Name = "KeyHead";
            Equipable = false;
        }

        public KeyHead(string craftedKeyId) : this()
        {
            CraftedKeyId = craftedKeyId;
        }
    }
}
