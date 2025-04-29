using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Item : ICollectible
    {
        private string _name;
        public string Name
        { 
            get => _name;
            set => _name = value;
        }
        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        //Type of the item, can be potion or weapon
        private string _type;
        public string Type
        {
            get => _type;
            set => _type = value;
        }

        public Item(string name, string description, string type)
        {
            _name = name;
            _description = description;
            _type = type;
        }

        //The method will be different for Wepon and Potion
        public virtual void Use(Player player) {}
    }
}
