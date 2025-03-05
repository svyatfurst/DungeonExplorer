using System;
using System.Collections.Generic;
using System.Media;

namespace DungeonExplorer
{
    internal class Game
    {
        private Player player;
        private Room currentRoom;
        private string roomItem;

        public Game(string name, string room, string item)
        {
            // Initialize the game with one room and one player
            player = new Player(name, 50);
            currentRoom = new Room(room);
            roomItem = item;
        }
        public void Start()
        {
            // Change the playing logic into true and populate the while loop
            bool playing = true;
            int choice;
            Console.WriteLine(
                $"You woke up in the {currentRoom.GetDescription()}"
            );
            while (playing)
            {
                // Code your playing logic here
                Console.Write(
                    "What do you want to do?\n" +
                    "1 - Look around\n" +
                    "2 - View Inventory\n" +
                    "3 - View Stats"
                );
                do
                {
                    Console.Write("Your input: ");
                    choice = int.Parse(Console.ReadLine());
                } while (
                !int.TryParse(Console.ReadLine(), out choice)
                    ||
                !new List<int> { 1, 2, 3 }.Contains(choice)
            );
            }
        }
    }
}