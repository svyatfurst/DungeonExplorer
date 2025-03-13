using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents the main game logic for Dungeon Explorer.
    /// </summary>
    internal class Game
    {
        // Declaring class fields
        private Player player;
        private Room currentRoom;
        private string roomItem;
        private string roomMusic;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="room">The name of the initial room.</param>
        /// <param name="item">The item present in the room.</param>
        /// <param name="music">The background music file for the room.</param>
        public Game(string name, string room, string item, string music)
        {
            player = new Player(name, 50);
            currentRoom = new Room(room, item);
            roomItem = item;
            roomMusic = music;
        }

        /// <summary>
        /// Outputs text centered in the console.
        /// Used for displaying the room description.
        /// </summary>
        /// <param name="text">The text to display.</param>
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

        /// <summary>
        /// Starts the game loop, allowing the player to interact with the game world.
        /// </summary>
        public void Start()
        {
            bool playing = true;

            // Outputting the room description in the center
            this.WriteCentered($"You woke up in the\n{currentRoom.GetDescription()}");

            // Declaring the array variable that stores all possible actions
            Action[] options = new Action[] {

                // Look around for items
                () => {
                    string currentItem = currentRoom.GetRoomItem();
                    if(currentItem != "none"){
                        Console.WriteLine($"After some time you found {currentItem}\nAre you willing to take it?");
                        Console.WriteLine("1 - Yes\n2 - No");
                        string choice;
                        do
                        {
                            Console.Write("Your input: ");
                            choice = Console.ReadLine();
                        } while (!new List<string> { "1", "2"}.Contains(choice));
                        if(choice == "1"){
                            player.PickUpItem(currentItem);
                            currentRoom.RemoveItem();
                        }
                    } else {
                        Console.WriteLine("There are no items left.");
                    }
                },

                // View inventory
                () => {
                    Console.WriteLine($"Your inventory contents: {player.InventoryContents()}");
                },

                // View health stats
                () => {
                    Console.WriteLine($"You currently possess {player.GetHealth()} health");
                },

                // Attempt to stab yourself
                () => {
                    Random rnd = new Random();
                    if(rnd.Next(0, 2) == 1)
                    {
                        Console.WriteLine("You successfully stabbed yourself, well done mate!");
                        player.TakeDamage(20);
                    }
                    else
                    {
                        Console.WriteLine("You missed, dumbass!");
                    }
                }
            };

            // Declaring the SoundPlayer instance to play background music
            SoundPlayer musicPlayer = new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, roomMusic));

            // Game loop
            while (playing)
            {
                // Play music at the start of each iteration
                musicPlayer.Play();

                // End the game if the player's health reaches zero
                if (player.GetHealth() == 0)
                {
                    playing = false;
                    break;
                }

                // Prompting the player for an action
                Console.WriteLine(
                    "What do you want to do?\n" +
                    "1 - Look Around\n" +
                    "2 - View Inventory\n" +
                    "3 - View Stats\n" +
                    "4 - Stab Yourself"
                );

                string choice;
                do
                {
                    Console.Write("Your input: ");
                    choice = Console.ReadLine();
                } while (!new List<string> { "1", "2", "3", "4" }.Contains(choice));

                // Execute selected action
                options[int.Parse(choice) - 1]();
            };

            // End of game message
            Console.WriteLine("You died a horrible death...\nBut yo! Thanks for playing!");
        }
    }
}
