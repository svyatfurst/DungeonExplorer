using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    public class Player
    {
        public string Name { get ; private set; }
        public int Health { get; private set; }
        private List<string> inventory = new List<string>();

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
        public void TakeDamage(int damage)
        {
            if(Health - damage > 0)
                Health -= damage;
            else
                Health = 0;

            Console.WriteLine($"You received {damage} damage. Now you have {Health}");
        }
        public int GetHealth()
        {
            return Health;
        }
    }
}