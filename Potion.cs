using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    //Class potion, inherits Item class
    internal class Potion : Item
    {
        //Upgrades
        int strengthUpgrade;
        int healthUpgrade;
        int heal;
        public Potion(string name, string description, int strengthUpgrade, int healthUpgrade, int heal) : base(name, description, "potion")
        {
            this.strengthUpgrade = strengthUpgrade;
            this.healthUpgrade = healthUpgrade;
            this.heal = heal;
        }
        //Method use, overrides original Use method from Item, changes Player's stats
        public override void Use(Player player)
        {
            player.StrengthUpgrade += strengthUpgrade;
            player.MaxHealth += healthUpgrade;
            player.inventory.Dispose(Name);
            if((player.Health + heal) < player.MaxHealth)
            {
                player.Health += heal;
            }
            else
            {
                player.Health = player.MaxHealth;
            }
        }

        
    }
    //Different kinds of potion, they all ingerit Potion class
    internal class HealingPotion : Potion
    {
        public HealingPotion() : base("Healing Potion", "Restores 5 hearts", 0, 0, 5) { }
    }

    internal class StrengthPotion : Potion
    {
        public StrengthPotion() : base("Strength Potion", "Adds extra 2 damage points to each attack", 2, 0, 0) { }
    }

    internal class HealthUpgrade : Potion
    {
        public HealthUpgrade() : base("Health Increase Potion", "Gives you 2 extra hearts", 0, 2, 0) { }
    }
}
