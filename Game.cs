using System;
using System.Collections.Generic;
using System.IO;
using System.Media;

namespace DungeonExplorer
{
    internal class Game
    {
        private Player player;
        private Room currentRoom;
        private string roomItem;
        private string roomMusic;

        public Game(string name, string room, string item, string music)
        {
            // Initialize the game with one room and one player
            player = new Player(name, 50);
            currentRoom = new Room(room);
            roomItem = item;
            roomMusic = music;
        }
        public void WriteCentered(string text)
        {
            int width = Console.WindowWidth;
            string[] lines = text.Split('\n');

            foreach (string line in lines)
            {
                string trimmedLine = line.TrimEnd();
                int leftPadding = Math.Max((width - trimmedLine.Length) / 2, 0);
                Console.WriteLine(new string(' ', leftPadding) + trimmedLine);
            }
        }
        public void Start()
        {
            // Change the playing logic into true and populate the while loop
            bool playing = true;
            this.WriteCentered(
                $"You woke up in the\n{currentRoom.GetDescription()}"
            );
            while (playing)
            {
                SoundPlayer player = new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, roomMusic));
                player.Play();
                // Code your playing logic here
                Console.WriteLine(
                    "What do you want to do?\n" +
                    "1 - Look around\n" +
                    "2 - View Inventory\n" +
                    "3 - View Stats"
                );
                int action = -1;
                string choice;
                do
                {
                    Console.Write("Your input: ");
                    choice = Console.ReadLine();
                    if (int.TryParse(choice, out action))
                    {
                        action = int.Parse(choice);
                    }
                    else
                    {
                        continue;
                    }
                } while (
                    !new List<int> { 1, 2, 3 }.Contains(action)
                );
            }
        }
    }
}