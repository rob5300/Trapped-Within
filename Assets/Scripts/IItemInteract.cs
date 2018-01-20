using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{

    /// <summary>
    /// Recieve event call when object is interacted with using an Item. This can be triggered even if the collider on the GameObject has isTrigger set to true.
    /// </summary>
    public interface IItemInteract
    {
        /// <summary>
        /// Called when this object is interacted with while the player is holding an Item.
        /// </summary>
        /// <param name="item">The item that was used to interact with.</param>
        /// <param name="player">The player that interacted</param>
        /// <returns>If the interaction was successful. Passed to the Item held.</returns>
        bool OnItemInteract(Item item, Player player);

    }
}
