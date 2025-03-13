using System;
using System.Diagnostics;

namespace DungeonExplorer.Tests
{
    /// <summary>
    /// A class containing test methods for validating functionality within the DungeonExplorer game.
    /// </summary>
    public class DebugTests
    {
        /// <summary>
        /// Starts the tests by running individual tests for Player, Room, and Game classes.
        /// </summary>
        public static void StartTest()
        {
            TestPlayer();
            TestRoom();
            TestGame();
            Console.Clear();
            Console.WriteLine("All tests passed successfully!");
        }

        /// <summary>
        /// Tests the behavior of the Player class.
        /// </summary>
        private static void TestPlayer()
        {
            // Creating a player instance
            Player player = new Player("Hero", 100);

            // Testing player properties
            Debug.Assert(player.Name == "Hero", "Player name should be 'Hero'");
            Debug.Assert(player.Health == 100, "Player health should start at 100");

            // Testing player actions
            player.PickUpItem("Sword");
            Debug.Assert(player.InventoryContents().Contains("Sword"), "Player should have picked up a sword");

            // Testing damage calculation
            player.TakeDamage(20);
            Debug.Assert(player.Health == 80, "Player health should be 80 after taking 20 damage");
        }

        /// <summary>
        /// Tests the behavior of the Room class.
        /// </summary>
        private static void TestRoom()
        {
            // Creating a room instance
            Room room = new Room("Dark Cave", "Torch");

            // Testing room description and item
            Debug.Assert(room.GetDescription() == "Dark Cave", "Room description should be 'Dark Cave'");
            Debug.Assert(room.GetRoomItem() == "Torch", "Room item should be 'Torch'");

            // Testing item removal
            room.RemoveItem();
            Debug.Assert(room.GetRoomItem() == "none", "Room item should be removed");
        }

        /// <summary>
        /// Tests the behavior of the Game class.
        /// </summary>
        private static void TestGame()
        {
            // Creating a game instance
            Game game = new Game("TestPlayer", "Test Room", "Test Item", "test.wav");

            // Testing game instance creation
            Debug.Assert(game != null, "Game instance should be created successfully");
        }
    }
}
