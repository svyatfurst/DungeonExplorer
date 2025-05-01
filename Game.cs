using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;

namespace DungeonExplorer
{
    //Class for handling game logic
    public class Game
    {
        
        // Declaring class fields
        private GameMap map = new GameMap();
        public Player player;
        public Room currentRoom;
        private int[] playerPosition = new int[2];
        private Action[] consoleContents = new Action[3];
        private string consoleDescription;
        private string healthBar;
        private string currentOptions;
        private string defaultOptions = "\nWhat do you want to do?\n" +
                    "[1]Look Around\n" +
                    "[2]View Inventory\n" +
                    "[3]View Stats\n" +
                    "[4]Move\n" +
                    "[5]Stab Yourself";
        private HashSet<(int, int)> discoveredRooms = new HashSet<(int, int)>();
        private string path = AppDomain.CurrentDomain.BaseDirectory;
        SoundPlayer musicPlayer = new SoundPlayer();
        private bool fighting = false;
        public bool isFightingWeapon = false;
        private bool isFightingTool = false;

        //Constructor
        public Game(string name)
        {
            //Changing Output Encoding for enabling symbols and emojis
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            player = new Player(name, 10);
            currentRoom = map.Map[playerPosition[0]][playerPosition[1]];
            Random rnd = new Random();
            playerPosition[0] = 0;
            playerPosition[1] = 0;
            currentOptions = defaultOptions;
                
            consoleDescription = currentRoom.Description;
        }

        //Method for outputting the text in the center
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

        //Starts the game
        public void Start()
        {
            bool playing = true;

            
            // Outputting the room description in the center
            

            // Declaring the array variable that stores all possible actions
            Action[] options = new Action[] {

                // Look around for items
                lookAround,

                // View inventory
                viewInventory,

                // View health stats
                viewStats,

                //Move to other rooms
                move,

                // Attempt to stab yourself
                suicideAttempt
            };

            // Declaring the SoundPlayer instance to play background music
            

            // Game loop
            while (playing)
            {
                if (currentRoom.Description == "victory")
                    break;

                // Play music at the start of each iteration
                playMusic(ostMusic);

                // End the game if the player's health reaches zero
                if (player.GetHealth() == 0)
                {
                    playing = false;
                    break;
                }

                //Checking nearest rooms to add them in the field of view of the map
                for (int i = playerPosition[1] - 1; i < playerPosition[1] + 2; i++)
                {
                    for (int j = playerPosition[0] - 1; j < playerPosition[0] + 2; j++)
                    {
                        discoveredRooms.Add((i, j));
                    }
                }

                // Prompting the player for an action
                DrawInterface();

                char choice;
                do
                {
                    choice = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                } while (!new List<char> { '1', '2', '3', '4', '5'}.Contains(choice));

                // Execute selected action
                options[int.Parse(choice.ToString()) - 1]();
            };

            // End of game message
            if (player.GetHealth() == 0)
                consoleDescription = deathArt + "\n\nYou died a horrible death...\nBut yo! Thanks for playing!";
            else
            {
                //Victory
                consoleDescription = victoryArt;
                playMusic(victoryMusic);
            }
                
            
            DrawInterface();
        }

        //Function, template for drawing interface every time
        public void DrawInterface()
        {
            Console.Clear(); //Clearing the console

            // Centered description
            WriteCentered(consoleDescription);
            Console.WriteLine("\n");

            // Making map
            List<string> mapRows = new List<string>();
            for (int i = 0; i < map.Map.Length; i++)
            {
                string mapRow = "";
                for (int j = 0; j < map.Map[i].Length; j++)
                {
                    if (!discoveredRooms.Contains((j, i)))
                        mapRow += " ?";
                    else if (i == playerPosition[0] && j == playerPosition[1])
                        mapRow += " ▲";
                    else if (map.Map[i][j].Description == "victory")
                        mapRow += " 🍷";
                    else if (map.Map[i][j].Description != "None")
                        mapRow += "  ";
                    
                    else
                        mapRow += " █";
                }
                mapRows.Add(mapRow);
            }

            // Making healthbar
            int health = player.GetHealth();
            string hearts = "";
            for (int i = 0; i < player.MaxHealth; i++)
            {
                if (i < player.Health)
                    hearts += "❤";
                else
                    hearts += "♡";
            }

            // Making Options
            string[] optionLines = currentOptions.Split('\n');

            int mapWidth = mapRows.Any() ? mapRows.Max(row => row.Length) : 0;
            int optionWidth = optionLines.Any() ? optionLines.Max(line => line.Length) : 0;
            int healthWidth = ("Health: " + hearts).Length;

            int leftPartWidth = 30; // Width of the map
            int rightPartWidth = Math.Max(optionWidth, healthWidth) + 10;
            int interfaceWidth = leftPartWidth + rightPartWidth + 3;

            int screenWidth = Console.WindowWidth;
            int paddingLeft = Math.Max((screenWidth - interfaceWidth) / 2, 0);

            int maxLines = Math.Max(mapRows.Count, 1 + optionLines.Length);

            // Upper frame
            Console.WriteLine(new string(' ', paddingLeft) + "+" + new string('-', interfaceWidth - 2) + "+");

            // Content
            for (int i = 0; i < maxLines; i++)
            {
                string leftContent = "";
                string rightContent = "";

                if (i < mapRows.Count)
                    leftContent = mapRows[i].PadRight(leftPartWidth);
                else
                    leftContent = new string(' ', leftPartWidth);

                if (i == 0)
                    rightContent = $"Health: {hearts}";
                else if (i - 1 < optionLines.Length)
                    rightContent = optionLines[i - 1];

                string combinedContent = leftContent + " " + rightContent.PadRight(rightPartWidth);

                // Outputting the frame
                Console.WriteLine(new string(' ', paddingLeft) + "| " + combinedContent.PadRight(interfaceWidth - 4) + " |");
            }

            // Lower frame
            Console.WriteLine(new string(' ', paddingLeft) + "+" + new string('-', interfaceWidth - 2) + "+");
        }

        //looking for items in the room
        public void lookAround()
        {
            Item currentItem = currentRoom.GetRoomItem();
            //If the item exists and in the room outout the item and suggest to take it, otherwise output that item is missing
            if (currentItem != null && currentItem.Name != "None")
            {
                consoleDescription += $"\nAfter some time you found {currentItem.Name}";
                currentOptions = "Are you willing to take it?\n[1]Yes\n[2]No";
                char choice;
                do
                {
                    DrawInterface();
                    choice = Console.ReadKey().KeyChar;
                } while (!new List<char> { '1', '2' }.Contains(choice));
                if (choice == '1')
                {
                    player.PickUpItem(currentItem);
                    currentRoom.RemoveItem();
                }
            }
            else
            {
                consoleDescription += "\nThere are no items left.";
                currentOptions = "Press any key to continue...";
                DrawInterface();
                Console.ReadKey();
            }
            currentOptions = defaultOptions;
            consoleDescription = currentRoom.Description;
        }

        //Method for handling managing the inventory
        public void viewInventory()
        {
            List<Item> inventory = new List<Item> ();
            //If the inventory is being looked at while fighting, separate it in items and weapons
            if (fighting)
            {
                if(isFightingTool)
                    inventory = player.inventory.contents.Where(e => e.Type == "potion").ToList();
                else if(isFightingWeapon)
                    inventory = player.inventory.contents.Where(e => e.Type == "weapon").ToList();
            }
            else
            {
                inventory = player.inventory.contents;
            }
            currentOptions = "What item do you need?\n";
            int i = 0;
            int[] currentPage = new int[] { 0, 3 };
            for(i = 0; i < inventory.Count; i++)
            {
                currentOptions += $"[{i + 1}]{inventory[i].Name}\n";
            }
            i += 1;
            currentOptions += $"[{i}]Exit";
            char choice;
            do
            {
                DrawInterface();
                choice = Console.ReadKey().KeyChar;
            } while (!(int.Parse(choice.ToString()) > 0) && int.Parse(choice.ToString()) <= i);
            if(int.Parse(choice.ToString()) != i)
            {
                Item item = inventory[int.Parse(choice.ToString()) - 1];
                consoleDescription += $"\n\n{item.Name}";
                currentOptions = $"What do you want to do?\n[1]";
                currentOptions += (item.Type == "potion") ? "Use" : "Equip";
                currentOptions += "\n[2]Exit";
                char choice1;
                do
                {
                    DrawInterface();
                    choice1 = Console.ReadKey().KeyChar;
                } while (!new Char[] { '1', '2'}.Contains(choice1));
                if (choice1 == '1')
                {
                    item.Use(player);
                    DrawInterface();
                }

            }
            currentOptions = defaultOptions;
            consoleDescription = currentRoom.Description;
        }

        //Method for viewing platers characteristics
        public void viewStats()
        {
            currentOptions = $"\nYour Health: {player.Health}\nAttack(With {player.equippedWeapon[0].Name}): {player.equippedWeapon[0].Attack + player.StrengthUpgrade}";
            currentOptions += "\nPress any key to continue...";
            DrawInterface();
            Console.ReadKey();
            currentOptions = defaultOptions;
            consoleDescription = currentRoom.Description;
        }

        //Method for handling player movement
        public void move()
        {
            List<string> possibleDirections = new List<string>();

            //Checking what direction player can move 
            if (playerPosition[0] - 1 >= 0 && playerPosition[0] - 1 < map.Map.Length && map.Map[playerPosition[0] - 1][playerPosition[1]].Description != "None")
            {
                possibleDirections.Add("UP");
            }

            if (playerPosition[0] + 1 < map.Map.Length && map.Map[playerPosition[0] + 1][playerPosition[1]].Description != "None")
            {
                possibleDirections.Add("DOWN");
            }

            if (playerPosition[1] - 1 >= 0 && playerPosition[1] - 1 < map.Map[playerPosition[0]].Length && map.Map[playerPosition[0]][playerPosition[1] - 1].Description != "None")
            {
                possibleDirections.Add("LEFT");
            }

            if (playerPosition[1] + 1 < map.Map[playerPosition[0]].Length && map.Map[playerPosition[0]][playerPosition[1] + 1].Description != "None")
            {
                possibleDirections.Add("RIGHT");
            }

            possibleDirections.Add("EXIT");

            List<char> optionList = new List<char>();
            //Outputting options for moving and handling input
            currentOptions = "Where do you want to go?\n";
            for (int i = 0; i < possibleDirections.Count; i++)
            {
                currentOptions += $"[{i + 1}]{possibleDirections[i]}\n";
                optionList.Add(Char.Parse((i + 1).ToString()));
            }
            char choice;
            do
            {
                DrawInterface();
                choice = Console.ReadKey().KeyChar;
            } while (!optionList.Contains(choice));
            switch (possibleDirections[int.Parse(choice.ToString()) - 1])
            {
                case "UP":
                    playerPosition[0] -= 1;
                    break;
                case "DOWN":
                    playerPosition[0] += 1;
                    break;
                case "LEFT":
                    playerPosition[1] -= 1;
                    break;
                case "RIGHT":
                    playerPosition[1] += 1;
                    break;
            }
            currentRoom = map.Map[playerPosition[0]][playerPosition[1]];
            if (currentRoom.monsters.Count > 1)
                fight();
            consoleDescription = currentRoom.Description;
            currentOptions = defaultOptions;
        }

        //Handling fighting with monsters
        public void fight()
        {
            //For viewInventory method, to see if the inventoryield needs to be separated into two
            fighting = true;

            //Changing to fighting music
            playMusic(dangerMusic);

            //outputting options and art for fighting
            consoleDescription = dangerArt;
            string fightOptions = "What do you want to do?\n[1]Attack\n[2]View items\n[3]View Weapons\n[4]Check Stats\n[5]Attack different monster";
            //Do the next until there are no monsters in the room
            while (currentRoom.monsters.Count > 0)
            {
                if (player.Health <= 0)
                    break;
                currentOptions = "Who do you want to attack?\n";
                for (int i = 0; i < currentRoom.monsters.Count; i++)
                {
                    Monster currentMonster = currentRoom.monsters[i];
                    currentOptions += $"[{i + 1}]{currentMonster.Name} Health: {currentMonster.Health} Attack: {currentMonster.Attack}\n";
                }
                DrawInterface();
                char choice;
                do
                {
                    DrawInterface();
                    choice = Console.ReadKey().KeyChar;
                } while (!(char.IsDigit(choice) && (int.Parse(choice.ToString()) > 0 && int.Parse(choice.ToString()) <= currentRoom.monsters.Count)));
                Monster fightingMonster = currentRoom.monsters[int.Parse(choice.ToString()) - 1];

                currentOptions = fightOptions;
                do
                {
                    DrawInterface();
                    choice = Console.ReadKey().KeyChar;
                } while (!new Char[] { '1', '2', '3', '4', '5' }.Contains(choice));
                //Attack
                if (choice == '1')
                {
                    if (new Random().Next(0, 101) < player.equippedWeapon[0].Accuracy)
                        fightingMonster.TakeDamage(player.equippedWeapon[0].Attack + player.StrengthUpgrade);
                    if (fightingMonster.Health <= 0)
                        currentRoom.removeMonster(fightingMonster);
                    player.TakeDamage(fightingMonster.Attack);
                    continue;
                }
                //View Items
                else if (choice == '2')
                {
                    isFightingWeapon = false;
                    isFightingTool = true;
                    viewInventory();
                    currentOptions = fightOptions;
                    consoleDescription = dangerArt;
                    continue;
                }
                //View Weapons
                else if (choice == '3')
                {
                    isFightingWeapon = true;
                    isFightingTool = false;
                    viewInventory();
                    currentOptions = fightOptions;
                    consoleDescription = dangerArt;
                    continue;
                }
                //Check stats
                else if (choice == '4')
                {
                    viewStats();
                    currentOptions = fightOptions;
                    consoleDescription = dangerArt;
                    continue;
                }
                //View different monster
                else if (choice == '5')
                    continue;
            }
            fighting = false;
            isFightingTool = false;
            isFightingWeapon = false;
        }
        //Suicide attampt to end the game
        public void suicideAttempt()
        {
            Random rnd = new Random();
            if (rnd.Next(0, 2) == 1)
            {
                player.TakeDamage(2);
            }
        }


        public void playMusic(string music)
        {
            try
            {
                musicPlayer.Stop();
                musicPlayer = new SoundPlayer(Path.Combine(path, music));
                musicPlayer.Play();
            }catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //ASCII ARTS and music paths
        public string deathArt = "⠀⠀⠀⠀⠀⢀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⡄⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⢀⡿⣷⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⠄⠀⠀⠀\r\n⠀⠀⠀⠀⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡆⠀⠀⠀\r\n⠀⠀⠀⢠⣿⣇⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⠀⠀⠀\r\n⠀⠀⠀⠀⣻⣿⣿⣿⣿⡿⡿⣿⣿⣿⣿⣿⣿⣿⣿⣯⣟⣿⣿⣿⣿⣷⣭⠀⠀⠀\r\n⠀⠀⠀⠀⣻⣿⠟⠛⠉⠁⠈⠉⠻⢿⣿⣿⣿⡟⠛⠂⠉⠁⠈⠉⠁⠻⣿⠀⠀⠀\r\n⠀⠀⠀⠀⢾⠀⠀⣠⠄⠻⣆⠀⠈⠠⣻⣿⣟⠁⠀⠀⠲⠛⢦⡀⠀⠠⠁⠀⠀⠀\r\n⠀⠀⠀⠀⢱⣄⡀⠘⠀⠸⠉⠀⠀⢰⣿⣷⣿⠂⢀⠀⠓⡀⠞⠀⢀⣀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠠⣿⣷⣶⣶⣶⣾⣿⠀⠸⣿⣿⣿⣶⣿⣧⣴⣴⣶⣶⣿⡟⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⢿⣿⣿⣿⣿⣿⣏⠇⠄⣿⣿⣿⣿⣿⣿⣿⣿⣿⣟⣾⠁⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⢺⣿⣿⣿⣿⣟⡿⠂⠈⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠑⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠈⣿⣿⣿⣿⣿⠀⠀⠈⠿⣿⣿⣿⣿⣿⣿⣿⣿⠁⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠄⢻⣿⣿⣿⡗⠀⠀⠀⠀⠈⠀⢨⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⡞⠷⠿⠿⠀⠀⠀⠀⢀⣘⣤⣿⣿⣿⣿⣿⡏⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠼⠉⠀⠀⠀⠀⠀⠚⢻⠿⠟⠓⠛⠂⠉⠉⠁⠀⡁⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣼⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⣿⡿⡀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⢾⠻⠌⣄⡁⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣀⣀⣀⡠⡲⠞⡁⠈⡈⣿⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⠘⠛⠻⢯⠟⠩⠀⠀⢠⣣⠈⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠄⠂⣰⣧⣾⠶⠀⠀⠀⠀⠀⠀⠀\n\n!";
        public string dangerArt = "⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠿⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⢉⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠁⠀⠀⠀⠀⠹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡅⠀⠀⠀⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⠀⠀⢠⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣄⠀⠄⠀⣠⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⣀⣉⣁⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣈⡏⠹⡟⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠛⢻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⡿⣿⣦⡀⠀⠈⣿⠛⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠟⠛⠃⣰⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣷⣿⠘⣿⣤⡀⢀⡀⠀⠀⠉⠻⡟⣿⣿⣿⡿⢿⣿⣿⣿⣿⣿⣿⣿⡿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠟⠋⢉⣁⣡⣤⣴⡾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⣾⣹⣿⣾⢿⣆⣠⠀⠀⢁⠈⣿⡿⠀⠈⣿⠟⠀⠈⠹⣿⡏⠀⠀⠈⣿⡟⠁⠸⣿⠃⣼⣏⢀⡆⣴⣿⣿⡟⡅⣿⢱⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣹⣿⡿⢿⣶⣷⣼⣷⣟⠁⠀⠀⡟⠀⠀⠀⠀⣽⡇⠀⠀⠀⠨⠄⠀⠀⢙⡀⢃⣸⣾⣿⣼⣿⢸⣧⣷⣿⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⣿⣷⡈⣷⡈⣻⣿⣼⣦⣀⣰⠃⠀⠀⠀⠀⠙⠁⠀⡀⠀⠊⣠⣀⣤⣾⣷⡌⣯⣿⢟⡟⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡺⠇⢹⣿⣿⣿⡿⣿⣤⣀⣀⣤⣤⣴⣄⠀⠃⣄⣀⢿⡿⠛⠿⣿⠙⣿⣷⣼⣧⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣿⣾⠹⣿⡿⠁⢹⡃⠈⠉⣿⠁⠊⡿⠀⠈⣿⠀⢸⡇⠀⠀⢿⡄⠹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⣿⠁⣠⣾⠇⠀⢠⣧⠀⢠⡇⠀⢀⣿⡀⢸⣇⠀⢠⣸⣷⣤⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣿⣿⣦⣠⣾⣿⣶⣾⣷⣤⣸⣿⣿⣾⣿⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿\n\nYou were attacked!!!";
        public string victoryArt = "                      d*##$.\r\n zP\"\"\"\"\"$e.           $\"    $o\r\n4$       '$          $\"      $\r\n'$        '$        J$       $F\r\n 'b        $k       $&gt;       $\r\n  $k        $r     J$       d$\r\n  '$         $     $\"       $~\r\n   '$        \"$   '$E       $\r\n    $         $L   $\"      $F ...\r\n     $.       4B   $      $$$*\"\"\"*b\r\n     '$        $.  $$     $$      $F\r\n      \"$       R$  $F     $\"      $\r\n       $k      ?$ u*     dF      .$\r\n       ^$.      $$\"     z$      u$$$$e\r\n        #$b             $E.dW@e$\"    ?$\r\n         #$           .o$$# d$$$$c    ?F\r\n          $      .d$$#\" . zo$&gt;   #$r .uF\r\n          $L .u$*\"      $&amp;$$$k   .$$d$$F\r\n           $$\"            \"\"^\"$$$P\"$P9$\r\n          JP              .o$$$$u:$P $$\r\n          $          ..ue$\"      \"\"  $\"\r\n         d$          $F              $\r\n         $$     ....udE             4B\r\n          #$    \"\"\"\"` $r            @$\r\n           ^$L        '$            $F\r\n             RN        4N           $\r\n              *$b                  d$\r\n               $$k                 $F\r\n               $$b                $F\r\n                 $\"\"               $F\r\n                 '$                $\r\n                  $L               $\r\n                  '$               $\r\n                   $               $\n\nYou won!";
        public string dangerMusic = "danger.wav";
        public string ostMusic = "ost.wav";
        public string victoryMusic = "ballroom.wav";
    }
}
