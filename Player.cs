using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    // Represents a player in the Dungeon Explorer game.
    public class Player : Creature
    {
        //Stores the player's inventory.
        public Inventory inventory = new Inventory();
        private int strengthUpgrade;
        public int StrengthUpgrade
        {
            get => strengthUpgrade; 
            set => strengthUpgrade = value;
        }
        private int maxHealth;
        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }
        //the weapon that is currently equipped by player, it originally is his fists
        public List<Weapon> equippedWeapon = new List<Weapon> { new Fists() };

        public Player(string name, int health) : base(health, 2, name)
        {
            maxHealth = health;
        }

        //Adds an item to the player's inventory.
        public void PickUpItem(Item item)
        {
            inventory.AddItem(item);
        }

        
    }
}
