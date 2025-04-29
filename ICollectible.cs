using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public interface ICollectible
    {
        string Name { get; set; }
        string Description { get; set; }
        string Type { get; set; }

        void Use(Player player);
    }
}
