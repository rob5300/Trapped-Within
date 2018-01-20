using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    public class CraftableItem : Item
    {
        private static Dictionary<List<Type>, Type> CraftableItemsMap = new Dictionary<List<Type>, Type>();

        static CraftableItem()
        {
            CraftableItemsMap.Add(new List<Type> { typeof(KeyHead), typeof(KeyBody) }, typeof(Key));
        }

        public static Type GetTypeFromCraftingComponents(params Type[] componentitems)
        {
            foreach(KeyValuePair<List<Type>, Type> pair in CraftableItemsMap)
            {
                List<Type> match = pair.Key.Where(x => x.Equals(componentitems[0])).ToList();
                if (match.Count > 0)
                {
                    //This somehow works?
                    List<Type> remaining = pair.Key.ToArray().ToList();
                    remaining.Remove(match[0]);
                    if(remaining.Count != 0)
                    {
                        int matches = 0;
                        foreach (Type key in componentitems)
                        {
                            if (remaining.Contains(key))
                            {
                                matches++;
                            }
                        }
                        if (matches == remaining.Count)
                        {
                            //This is the correct pair for these components.
                            return pair.Value;
                        }
                    }
                }
            }
            return null;
        }

        public static Item GetItemInstanceFromCraftingComponents(params Type[] componentitems) {
            return GetItemInstanceFromType(GetTypeFromCraftingComponents(componentitems));
        }

        /// <summary>
        /// Called when a new instance of this CraftableItem is made, allowing the transfer of information from components.
        /// </summary>
        /// <param name="craftingComponents"></param>
        public virtual void OnItemCrafted(List<CraftingComponent> craftingComponents)
        {
            
        }

        public CraftableItem()
        {
            
        }
    }
}
