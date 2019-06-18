using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    [Serializable]

    class UnitData
    {
        public int Count { get; set; }
        public int CellSize { get; set; }

        public CellData[] cells;

        public UnitData(Unit unit)
        {
            LoadUnit(unit);
        }

        protected void LoadUnit(Unit unit)
        {
            var unitCells = unit.GetCellList();
            Count = unitCells.Count;
            CellSize = unit.CellSize;
            cells = new CellData[Count];
            
            for (int i= 0; i < Count; i++)
            {
                CellData next = new CellData(unitCells[i]);
                cells[i] = next;
            }
        }
    }
}
