using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    public class Player
    {
        public string Name { get ; private set; }
        public int Health { get; private set; }
        public Player(string name, int health) 
        {
            Name = name;
            Health = health;
        }
        public void PickUpItem(string item)
        {
            inventory.Add(item);
            Console.WriteLine($"{item} has been successfuly added to the inventory!");
        }
        public string InventoryContents()
        {
            return string.Join(", ", inventory);
        }
    }
}