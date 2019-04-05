using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class BuildManager
    {
        Unit player;
        public CellType Selected { get; set; }
        bool rightClick;
        bool leftClick;
        Vector2 mouseVec;

        public BuildManager(Unit player, CellType defType = CellType.Hull)
        {
            this.player = player;
            Selected = defType;
            rightClick = false;
            leftClick = false;
            mouseVec = new Vector2(0, 0);
        }

        public void GetMouse(Vector2 mouseVec, bool leftClick, bool rightClick)
        {
            this.mouseVec = mouseVec;
            this.leftClick = leftClick;
            this.rightClick = rightClick;
        }

        public int building(int credits, Planet planet)
        {
            player.cellSet(2, 0);
            Cell over = player.CellIntercect(mouseVec, 2);

            if (over != null)
            {
                int cost = Tool.getValue(Selected, planet);
                if (leftClick && !over.Live && credits >= cost)
                {
                    over.Type = Selected;
                    over.reset();
                    over.Live = true;
                    return cost;
                }
                if (rightClick && over.Live)
                {
                    over.Live = false;
                    return -cost;
                }
            }
            return 0;
        }
    }
}
