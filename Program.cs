using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using DungeonExplorer.Tests;


namespace DungeonExplorer
{
    /// <summary>
    /// The main entry point of the Dungeon Explorer game.
    /// </summary>
    internal class Program
    {
        //The main method that starts the game and runs the checks.
        static void Main(string[] args)
        {
            //Running tests
            DebugTests.StartTest();


            // Getting username input from the user.
            Console.Write("Please, enter your nickname: ");
            string Name = Console.ReadLine();


            // Creating a new instance of the Game class with the selected parameters.
            Game game = new Game(Name);

            // Starting the game.
            game.Start();

            Console.WriteLine("Waiting for your Implementation");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
