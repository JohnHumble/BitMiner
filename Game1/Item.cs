using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Item
    {
        public CellType Type { get; protected set; }
        public int Amount { get; protected set; }

        public Item()
        {
            Type = CellType.Fill;
            Amount = 0;
        }

        public Item(CellType type, int amount)
        {
            Type = type;
            Amount = amount;
        }

        public void Add(int amount)
        {
            Amount += amount;
        }
    }
}
