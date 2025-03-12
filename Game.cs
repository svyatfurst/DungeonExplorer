using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

namespace DungeonExplorer
{
    internal class Game
    {
        //Declaring class fields
        private Player player;
        private Room currentRoom;
        private string roomItem;
        private string roomMusic;

        //Class constructor
        public Game(string name, string room, string item, string music)
        {
            // Initialize the game with one room and one player
            player = new Player(name, 50);
            currentRoom = new Room(room, item);
            roomItem = item;
            roomMusic = music;
        }
        //Function to output text in the center of the console. Used for room description
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

            //Outputting the room description in the center
            this.WriteCentered(
                $"You woke up in the\n{currentRoom.GetDescription()}"
            );

            //Declaring the array varibale that stores all actions you can do in the game
            Action[] options = new Action[] {

                //Looking around(looking for items)
                () => {
                    string currentItem = currentRoom.GetRoomItem();
                    if(currentItem != "none"){
                        Console.WriteLine($"After some time you found {currentItem}\nAre you willing to take it?");
                        Console.WriteLine("1 - Yes\n2 - No");
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
                            !new List<int> { 1, 2}.Contains(action)
                        );
                        if(action == 1){
                            player.PickUpItem(currentItem);
                            currentRoom.RemoveItem();
                        }
                    }else
                        Console.WriteLine("There are no items left.");
                },
                //Viewing inventory
                () => {
                    Console.WriteLine($"Your inventory contens: {player.InventoryContents()}");
                },

                //Viewing health
                () => {
                    Console.WriteLine($"You currently posses {player.GetHealth()} health");
                },

                //Trying to stab yourself(slowly ending the game)
                () =>
                {
                    Random rnd = new Random();
                    if(rnd.Next(0, 2) == 1)
                    {
                        Console.WriteLine("You successfuly stabbed yourself, well done mate!");
                        player.TakeDamage(20);
                    }
                    else
                        Console.WriteLine("You missed, dumbass!");
                }
            };

            //Declaring the SoundPlayer instance to play the music
            SoundPlayer musicPlayer = new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, roomMusic));
            
            //Game starts
            while (playing)
            {
                //Music starts playing on the start of iteration
                musicPlayer.Play();

                // Code your playing logic here
                if (player.GetHealth() == 0)
                {
                    playing = false;
                    break;
                }

                Console.WriteLine(
                    "What do you want to do?\n" +
                    "1 - Look Around\n" +
                    "2 - View Inventory\n" +
                    "3 - View Stats\n" +
                    "4 - Stab Yourself" 
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
                    !new List<int> { 1, 2, 3, 4 }.Contains(action)
                );
                options[action - 1]();
            };
            Console.WriteLine("You died a horrible death...\nBut yo! Thanks for playing!");
        }
    }
}