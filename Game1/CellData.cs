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
        public float LocX { get; set; }
        public float LocY { get; set; }

        public CellType Type { get; set; }
        public bool Live { get; set; }

        public CellData(float x, float y, float locX, float locY, CellType type, bool live)
        {
            X = x;
            Y = y;
            LocX = locX;
            LocY = locY;
            Type = type;
            Live = live;
        }

        public CellData(Cell cell)
        {
            X = cell.X;
            Y = cell.Y;
            LocX = cell.LocX;
            LocY = cell.LocY;
            Type = cell.Type;
            Live = cell.Live;
        }
    }
}
