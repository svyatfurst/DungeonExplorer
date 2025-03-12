namespace DungeonExplorer
{
    public class Room
    {
        private string description;
        private string roomItem;

        public Room(string description, string item)
        {
            this.description = description;
            this.RoomItem = item;
        }

        public string RoomItem { get => roomItem; set => roomItem = value; }

        public string GetDescription()
        {
            return description;
        }

        public string GetRoomItem()
        {
            if(RoomItem == null)
            {
                return "none";
            }
            return RoomItem;
        }

        public void RemoveItem()
        {
            this.RoomItem = null;
        }
    }
}