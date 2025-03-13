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

        /// <summary>
        /// Stores the item available in the room.
        /// </summary>
        private string roomItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        /// <param name="description">The description of the room.</param>
        /// <param name="item">The item present in the room.</param>
        public Room(string description, string item)
        {
            this.description = description;
            this.RoomItem = item;
        }

        /// <summary>
        /// Gets or sets the item present in the room.
        /// </summary>
        public string RoomItem { get => roomItem; set => roomItem = value; }

        /// <summary>
        /// Retrieves the description of the room.
        /// </summary>
        /// <returns>A string representing the room's description.</returns>
        public string GetDescription()
        {
            return description;
        }

        /// <summary>
        /// Retrieves the item present in the room. If no item is available, returns "none".
        /// </summary>
        /// <returns>A string representing the room's item.</returns>
        public string GetRoomItem()
        {
            if (RoomItem == null)
            {
                return "none";
            }
            return RoomItem;
        }

        /// <summary>
        /// Removes the item from the room.
        /// </summary>
        public void RemoveItem()
        {
            this.RoomItem = null;
        }
    }
}