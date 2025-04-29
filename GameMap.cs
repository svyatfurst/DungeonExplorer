using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class GameMap
    {
        private Random rnd = new Random();

        //ASCII arts for rooms
        private string[] rooms = {
                "   _________________________________________________________\r\n /|     -_-                                             _-  |\\\r\n/ |_-_- _                                         -_- _-   -| \\   \r\n  |                            _-  _--                      | \r\n  |                            ,                            |\r\n  |      .-'````````'.        '(`        .-'```````'-.      |\r\n  |    .` |           `.      `)'      .` |           `.    |          \r\n  |   /   |   ()        \\      U      /   |    ()       \\   |\r\n  |  |    |    ;         | o   T   o |    |    ;         |  |\r\n  |  |    |     ;        |  .  |  .  |    |    ;         |  |\r\n  |  |    |     ;        |   . | .   |    |    ;         |  |\r\n  |  |    |     ;        |    .|.    |    |    ;         |  |\r\n  |  |    |____;_________|     |     |    |____;_________|  |  \r\n  |  |   /  __ ;   -     |     !     |   /     `'() _ -  |  |\r\n  |  |  / __  ()        -|        -  |  /  __--      -   |  |\r\n  |  | /        __-- _   |   _- _ -  | /        __--_    |  |\r\n  |__|/__________________|___________|/__________________|__|\r\n /                                             _ -        lc \\\r\n/   -_- _ -             _- _---                       -_-  -_ \\",

                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣠⣤⣤⣤⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⢠⣴⣾⣷⠈⣿⣿⣿⣿⣿⡟⢀⣿⣶⣤⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⢠⣾⣷⡄⠻⣿⣿⣧⠘⣿⣿⣿⡿⠀⣾⣿⣿⠃⣰⣿⣶⣄⠀⠀⠀⠀\r\n⠀⠀⠀⣴⣿⣿⣿⡿⠆⠉⠉⠁⠀⠈⠉⠉⠁⠀⠙⠛⠃⢰⣿⣿⣿⣿⣷⡀⠀⠀\r\n⠀⠀⣼⣿⣿⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⢿⣿⣿⣿⣷⠀⠀\r\n⠀⠘⠛⠛⠛⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠻⠛⠛⠛⠃⠀\r\n⠀⢸⣿⣿⣿⡇⠀⠀⣀⣰⡄⠀⠀⠀⠀⠀⠀⠀⠀⢠⣶⣠⠀⠀⢰⣾⣿⣿⡇⠀\r\n⠀⢸⡿⠿⠿⠇⠀⢟⠉⠁⣳⠀⠀⠀⠀⠀⠀⠀⠀⣿⠈⠈⡿⠀⢸⣿⣿⣿⡇⠀\r\n⠀⣤⣤⣴⣶⡆⠀⢠⣀⡀⠁⠀⠀⠀⠀⠀⠀⠀⠀⠈⢀⣤⡀⠀⠘⠿⠿⠿⠇⠀\r\n⠀⣿⣿⣿⣿⣷⠀⢸⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢿⡇⠀⣶⣶⣶⣶⣶⠀\r\n⠀⣿⣿⣿⣿⣿⠀⠚⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠃⠀⣿⣿⣿⣿⣿⠀\r\n⠀⠛⠛⠛⠛⢛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠛⠛⠋⣁⠀\r\n⠀⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿⠀\r\n⠀⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿⠀\r\n⠀⠛⠛⠛⠛⠛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠛⠛⠛⠛⠛⠀",

                "_I_\r\n.~'_`~.\r\n/(  ,^ .~ ~. ^.  )\\\r\n\\ \\/ .^ |   ^. \\/ /\r\nY  /   |     \\  Y___.oOo.___ \r\n| Y    |      Y |           |           |\r\n| |    |      | |           |           |\r\n| |   _|___   | |           |           |\r\n| |  /____/|  | |           |           |\r\n|.| |   __/|__|.|           |           |\r\n|.| |   __/|  |.|          _|___________|_\r\n|:| |   __//  |:|         '^^^^^^^^^^^^^^^`\r\n|:| |_____/   |:|\r\n____|_|/          |_|_____________________________\r\n____]H[           ]H[_____________________________\r\n/             \\"
        };

        // Items to choose and put in room
        private Item[] items = {
                new HealingPotion(),
                new StrengthPotion(),
                new HealthUpgrade(),
                new Spear(),
                new Mace(),
                new Sword()
        };

        //Creating new game labyrynth
        private Room[][] map = new Room[10][];
        public Room[][] Map {
            get => map;
            set => map = value;
        }
        //Constructor
        public GameMap()
        {
            for (int y = 0; y < map.Length; y++)
            {
                Room[] newMap = new Room[10];
                //if there are none of halls to go lower than one will be generated randomly
                int hallsCount = 0;
                for (int x = 0; x < map.Length; x++)
                {
                    //There is 70 percent chance that it will be a wall
                    bool isHall = rnd.Next(100) < 70;
                    //Every second row is whole of rooms
                    newMap[x] = (y + 1) % 2 == 0 ? GenerateRoom(true) : isHall ? GenerateRoom(false) : GenerateRoom(true);
                    if(isHall)
                        hallsCount++;
                }
                if(hallsCount == 0)
                    newMap[rnd.Next(map.Length)] = GenerateRoom(true);
                map[y] = newMap;
            }
            //Creating first room and ensuring there are no monsters
            map[0][0] = GenerateRoom(true);
            map[0][0].monsters = new List<Monster>();
            //Creating an escape room at the bottom half of the labyrynth
            int[] randomEscape = { rnd.Next(5, 10), rnd.Next(0, 10)};
            map[randomEscape[0]][randomEscape[1]] = GenerateRoom(true);
            map[randomEscape[0]][randomEscape[1]].Description = "victory";
            
        }
        //Picking random monster to add to the room
        private Monster GenerateRandomMonster()
        {
            Monster randomMonster = new Monster(0, 0, "None");
            switch(rnd.Next(0, 3))
            {
                case 0:
                    randomMonster = new Ogre();
                    break;
                case 1:
                    randomMonster = new Vampire();
                    break;
                case 2:
                    randomMonster = new Slime();
                    break;
            }
            return randomMonster;
        }

        //Generating a random room
        private Room GenerateRoom(bool isValidRoom)
        { 
            //Creating an empty item
            Item item = new Item("None", "None", "None");
            string roomDescription;
            List < Monster > monster = new List<Monster>();

            if (isValidRoom)
            {
                for(int i = 0; i < rnd.Next(0, 4); i++)
                {
                    monster.Add(GenerateRandomMonster());
                }
                int roomSettings = rnd.Next(0, rooms.Length);
                item = (rnd.Next(0, 2) == 1) ? items[rnd.Next(0, items.Length)] : item;
                roomDescription = rooms[roomSettings];

            }
            else
            {
                item.Name = "None";
                roomDescription = "None";
            }

            Room newRoom = new Room(
                roomDescription,
                item
            );
            newRoom.monsters = monster;
            return newRoom;
        }

    }
}
