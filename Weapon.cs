using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    //Class Weapon, inherits Item class, has its unique fields
    public class Weapon : Item
    {
        private int _attack;
        public int Attack
        {
            get => _attack; 
            set => _attack = value;
        }
        private int _accuracy;
        public int Accuracy
        {
            get => _accuracy;
            set => _accuracy = value;
        }
        public Weapon(string name, string description, int attack, int accuracy) : base(name, description, "weapon")
        {
            _attack = attack;
            _accuracy = accuracy;
        }

        //Overriding original method Use
        public override void Use(Player player)
        {
            player.equippedWeapon = new List<Weapon> { this };
        }

        
    }
    //Child weapon classes
    internal class Spear : Weapon
    {
        public Spear() : base("Spear", "Precise and Furious!", 4, 90) { }
    }

    internal class Mace : Weapon
    {
        public Mace() : base("Mace", "Powerful, but not precise!", 6, 40) { }
    }

    internal class Sword : Weapon
    {
        public Sword() : base("Sword", "More powerful than spear, but less precise!", 5, 80) { }
    }

    internal class Fists : Weapon
    {
        public Fists() : base("Fists", "Old friends!", 3, 60) { }
    }
}
