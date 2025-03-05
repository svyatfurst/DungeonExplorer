using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string[] rooms = { 
                "Ancient Ballrom.\n" +
                    "The haunted place with immense aura.\n" +
                    "Once it was filled with hundreds of people dancing and having,\n" +
                    "but now it's just a pathetic reminder of the ancient great civilisation",

                "Abandoned Laboratory.\n" +
                    "The place that gave birth to thousands of potions and monsters.\n" +
                    "Only a tiny fraction of that left",

                "Coronation Hall.\n" +
                    "The place where it all started...\n" +
                    "Enormous place that is now filled with nothing but darkness and emptiness." 
            };

            string[] items = {
                "potion",
                "sword",
                "shield",
                "bow"
            }

            Console.Write("Please, enter your nickname: ");
            string Name = Console.ReadLine();

            Console.Write(
                "What room do you want to enter?\n" +
                "1 - Ancient Ballroom\n" +
                "2 - Abandoned Laboratory\n" +
                "3 - Coronation Hall\n"
                );
            int room;
            do
            {
                Console.Write("Your input: ");
                room = int.Parse(Console.ReadLine());
            } while (
                !int.TryParse(Console.ReadLine(), out room)
                    ||
                !new List<int> { 1, 2, 3 }.Contains(room)
            );

            Random rnd = new Random();
            Game game = new Game(Name, rooms[room - 1], items[rnd.Next(0, items.Length)]);
            game.Start();
            Console.WriteLine("Waiting for your Implementation");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
