using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Inventory
    {
        public List<Item> contents = new List<Item>();

        //Filling inventory with basics
        public Inventory() 
        {
            contents.Add(new Fists());
            contents.Add(new HealingPotion());
            contents.Add(new HealthUpgrade());
        }

        //Deleting the first item with the required name from the inventory
        public void Dispose(string name)
        {
            for(int i = 0; i < contents.Count; i++)
            {
                if (contents[i].Name == name)
                {
                    contents.RemoveAt(i);
                    break;
                }
                    
            }
        }

        //Adding new item if there are less than 8 items in inventory
        public void AddItem(Item item)
        {
            if(contents.Count < 8)
                contents.Add(item);
        }
    }
}
