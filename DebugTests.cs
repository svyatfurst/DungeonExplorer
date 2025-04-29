using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DungeonExplorer.Tests
{
    public class DebugTests
    {
        public static Game game;
        public static GameMap gameMap;
        public static Inventory inventory;

        public static void SetUp()
        {
            game = new Game("Player");
            gameMap = new GameMap();
            inventory = new Inventory();
        }

        public static void TestGameInitialization()
        {
            Debug.Assert(game != null, "Game instance should not be null");
            Debug.Assert(game.player != null, "Player instance should not be null");
            Debug.Assert(game.currentRoom != null, "Current room should not be null");
        }

        public static void TestInventory()
        {
            var newItem = new HealingPotion();
            inventory.AddItem(newItem);
            Debug.Assert(inventory.contents.Contains(newItem), "Item should be added to inventory");
            for (int i = 0; i < 8; i++)
            {
                inventory.AddItem(new HealingPotion());
            }
            var newItem1 = new HealthUpgrade();
            inventory.AddItem(newItem1);
            Debug.Assert(!inventory.contents.Contains(newItem1), "Item should not be added to full inventory");
            var itemToRemove = new HealingPotion();
            inventory.AddItem(itemToRemove);
            inventory.Dispose("HealingPotion");
            Debug.Assert(!inventory.contents.Contains(itemToRemove), "Item should be removed from inventory");
        }

        public static void TestPlayer()
        {
            Player player = new Player("TestPlayer", 100);
            Debug.Assert(player != null, "Player instance should be created successfully");
            Debug.Assert(player.Health == 100, "Player health should be initialized to 100");
            Debug.Assert(player.Attack == 2, "Player attack should be initialized to 2");
            Debug.Assert(player.Name == "TestPlayer", "Player name should be 'TestPlayer'");

            var healingPotion = new HealingPotion();
            player.PickUpItem(healingPotion);
            Debug.Assert(player.inventory.contents.Contains(healingPotion), "HealingPotion should be in player inventory");

            player.StrengthUpgrade = 5;
            Debug.Assert(player.StrengthUpgrade == 5, "Player's strength upgrade should be set to 5");

            player.MaxHealth = 200;
            Debug.Assert(player.MaxHealth == 200, "Player's max health should be set to 200");
        }

        public static void TestMonster()
        {
            Monster monster = new Monster(100, 10, "TestMonster");
            Debug.Assert(monster != null, "Monster instance should be created successfully");
            Debug.Assert(monster.Health == 100, "Monster health should be initialized to 100");
            Debug.Assert(monster.Attack == 10, "Monster attack should be initialized to 10");
            Debug.Assert(monster.Name == "TestMonster", "Monster name should be 'TestMonster'");
        }

        public static void TestRoom()
        {
            Item roomItem = new HealingPotion();
            Room room = new Room("Test Room", roomItem);
            Monster monster = new Monster(50, 5, "TestMonster");
            Monster nonExistentMonster = new Monster(30, 3, "NonExistentMonster");
            Debug.Assert(room != null, "Room instance should be created successfully");
            Debug.Assert(room.Description == "Test Room", "Room description should be 'Test Room'");
            Debug.Assert(room.GetRoomItem() == roomItem, "Room item should match the initialized item");
            room.RemoveItem();
            Debug.Assert(room.GetRoomItem() == null, "Room item should be null after removal");
            room.monsters.Add(monster);
            room.removeMonster(monster);
            Debug.Assert(!room.monsters.Contains(monster), "Monster should be removed from room");
            room.monsters.Add(monster);
            room.removeMonster(nonExistentMonster);
            Debug.Assert(room.monsters.Contains(monster), "Non-existent monster should not be removed from the room");
        }

        public static void TestItemInitialization()
        {
            Item item = new Item("Healing Potion", "Restores 50 HP", "Potion");
            Debug.Assert(item != null, "Item instance should be created successfully");
            Debug.Assert(item.Name == "Healing Potion", "Item name should be 'Healing Potion'");
            Debug.Assert(item.Description == "Restores 50 HP", "Item description should be 'Restores 50 HP'");
            Debug.Assert(item.Type == "Potion", "Item type should be 'Potion'");
        }

        public static void TestWeapon()
        {
            Weapon weapon = new Sword();
            Debug.Assert(weapon != null, "Weapon instance should be created successfully");
            Debug.Assert(weapon.Name == "Sword", "Weapon name should be 'Sword'");
            Debug.Assert(weapon.Description == "More powerful than spear, but less precise!", "Weapon description should match");
            Debug.Assert(weapon.Type == "weapon", "Weapon type should be 'weapon'");
            Debug.Assert(weapon.Attack == 5, "Weapon attack should be 5");
            Debug.Assert(weapon.Accuracy == 80, "Weapon accuracy should be 80");
            Player player = new Player("TestPlayer", 100);
            weapon.Use(player);
            Debug.Assert(player.equippedWeapon.Count == 1, "Player should have one weapon equipped");
            Debug.Assert(player.equippedWeapon[0] == weapon, "Equipped weapon should be the sword");
        }

        public static void TestGameMap()
        {
            GameMap gameMap = new GameMap();
            Room[][] map = gameMap.Map;

            Debug.Assert(map.Length == 10, "Game map should have 10 rows");

            foreach (var row in map)
            {
                Debug.Assert(row.Length == 10, "Each row should have 10 columns");
            }

            int validRooms = 0;
            bool escapeRoomFound = false;

            foreach (var row in map)
            {
                foreach (var room in row)
                {
                    if (room.Description != "None")
                    {
                        validRooms++;
                    }
                    if (room.Description == "victory")
                    {
                        escapeRoomFound = true;
                    }
                }
            }

            Debug.Assert(validRooms > 0, "There should be at least one valid room");
            Debug.Assert(escapeRoomFound, "There should be at least one escape room with 'victory'");
        }


        public static void StartTest()
        {
            Console.Clear();
            Console.WriteLine("Running tests...");

            SetUp();
            TestGameInitialization();
            TestInventory();
            TestPlayer();
            TestMonster();
            TestRoom();
            TestItemInitialization();
            TestWeapon();
            TestGameMap();

            Console.WriteLine("All tests passed successfully!");
        }
    }
}
