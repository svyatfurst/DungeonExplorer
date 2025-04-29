using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    //Class Monster inherits Creature class
    public class Monster : Creature
    {

        public Monster(int health, int attack, string name) : base(health, attack, name){}
        
    }
    //Subclasses Ogre, Vampire and Slime, they all inherit Monster class
    internal class Ogre : Monster
    {
        public Ogre() : base(6, 3, "Ogre") { }
    }

    internal class Vampire : Monster
    {
        public Vampire() : base(4, 1, "Vampire") { }
    }

    internal class Slime : Monster
    {
        public Slime() : base(2, 1, "Slime") { }
    }
}
