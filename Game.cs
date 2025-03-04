using System;
using System.Collections.Generic;
using System.Media;

namespace DungeonExplorer
{
    internal class Game
    {
        private Player player;
        private Room currentRoom;

        public Game(string name, string room)
        {
            // Initialize the game with one room and one player
            player = new Player(name, 100);
            currentRoom = new Room(room);
    }
        private void fight()
        {

        }
        public void Start()
        {
            // Change the playing logic into true and populate the while loop
            bool playing = true;
            while (playing)
            {
                // Code your playing logic here
                Console.WriteLine("HUI");
            }
        }
    }
}