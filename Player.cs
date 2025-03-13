using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a player in the Dungeon Explorer game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets the player's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the player's current health.
        /// </summary>
        public int Health { get; private set; }

        /// <summary>
        /// Stores the player's inventory.
        /// </summary>
        private List<string> inventory = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="health">The initial health of the player.</param>
        public Player(string name, int health)
        {
            Name = name;
            Health = health;
        }

        /// <summary>
        /// Adds an item to the player's inventory.
        /// </summary>
        /// <param name="item">The item to be picked up.</param>
        public void PickUpItem(string item)
        {
            inventory.Add(item);
            Console.WriteLine($"{item} has been successfully added to the inventory!");
        }

        /// <summary>
        /// Retrieves the contents of the player's inventory as a comma-separated string.
        /// </summary>
        /// <returns>A string representing the player's inventory contents.</returns>
        public string InventoryContents()
        {
            return string.Join(", ", inventory);
        }

        /// <summary>
        /// Reduces the player's health when taking damage.
        /// </summary>
        /// <param name="damage">The amount of damage taken.</param>
        public void TakeDamage(int damage)
        {
            if (Health - damage > 0)
                Health -= damage;
            else
                Health = 0;

            Console.WriteLine($"You received {damage} damage. Now you have {Health} health.");
        }

        /// <summary>
        /// Gets the player's current health.
        /// </summary>
        /// <returns>The player's current health.</returns>
        public int GetHealth()
        {
            return Health;
        }
    }
}
