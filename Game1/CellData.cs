using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    [Serializable]

    enum CellType { Hull, Cab, Thruster, Blaster, Cargo, Fill, Iorn, Gold, Uranium }

    [Serializable]

    class CellData
    {
        public float X { get; set; }
        public float Y { get; set; } 
        public CellType Type { get; set; }
        public bool Live { get; set; }

        public CellData(float x, float y, CellType type, bool live)
        {
            X = x;
            Y = y;
            Type = type;
            Live = live;
        }
    }
}
