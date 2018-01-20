using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Lightbulb : Item
    {
        public void Reset()
        {
            Name = "Lightbulb";
        }

        public override void OnInteract(Player player)
        {
            player.inventory.AddItem(new Items.Lightbulb());
            Destroy(gameObject);
        }
    }
}
