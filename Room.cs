using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a room in the Dungeon Explorer game.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Stores the room's description.
        /// </summary>
        private string description;

        public string Description 
        { 
            get => description; 
            set => description = value; 
        }
        
        /// <summary>
        /// Stores the item available in the room.
        /// </summary>
        private Item roomItem;

        public Item RoomItem { 
            get => roomItem; 
            set => roomItem = value; 
        }


        public List<Monster> monsters = new List<Monster>();


        public Room(string description, Item item)
        {
            this.description = description;
            this.roomItem = item;
        }

        //Retrieves the item present in the room. If no item is available, returns "none".
        public Item GetRoomItem()
        {
            return roomItem;
        }

        // Removes the item from the room.
        public void RemoveItem()
        {
            this.RoomItem = null;
        }

        //Removes the first monster that equals to input
        public void removeMonster(Monster monsterToRemove)
        {
            for(int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i] == monsterToRemove)
                {
                    monsters.RemoveAt(i);
                    break;
                }
            }
        }
    }
}