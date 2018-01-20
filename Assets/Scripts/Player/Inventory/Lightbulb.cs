
using UnityEngine;

namespace Items
{
    public class Lightbulb : Item
    {

        public Lightbulb()
        {
            //Name = "Light bulb";
            EntityGameObject = Resources.Load<GameObject>("Items/Light bulb");
            Equipable = true;
        }

        public override void OnItemUse(GameObject heldItem, Player player, bool successful)
        {
            if (successful)
            {
                player.inventory.RemoveItem(this);
            }
        }
    }
}
