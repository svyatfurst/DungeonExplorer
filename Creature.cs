using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Creature : IDamageable
    {
        //Creating protected fields, they will be inheritted and used in child classes
        protected int _health;
        public int Health
        {
            get => _health;
            set => _health = value;
        }

        protected int _attack;
        public int Attack 
        { 
            get => _attack;
            set => _attack = value;
        }

        protected string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public bool isAlive = true;

        public Creature(int health, int attack, string name)
        {
            _health = health;
            _attack = attack;
            _name = name;
        }

        public Creature TakeDamage(int damage)
        {
            _health -= damage;
            if(_health <= 0)
            {
                _health = 0;
                isAlive = false;
            }
            return this;
        }

        public int GetHealth()
        {
            return Health;
        }
    }
}
