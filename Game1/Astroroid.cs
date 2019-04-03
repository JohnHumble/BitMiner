using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Astroroid : Unit
    {
        float rotSpeed;
        Random rand;

        public float Mass { get; protected set; }

        public Astroroid(int size, int cellSize, Random rand)
        {
            Size = size;
            this.CellSize = cellSize;
            center = new Vector2(size / 2 - .5f, size / 2 - .5f);
            this.rand = rand;
            rotation = 0;
            buildAstroroid();
        }

        public Astroroid(int size, int cellSize, Vector2 loc, Random rand,Planet planet = null)
        {
            Size = size;
            this.CellSize = cellSize;
            center = new Vector2(size / 2 - .5f, size / 2 - .5f);
            this.rand = rand;
            location = loc;
            rotation = 0;
            buildAstroroid(planet);
        }

        protected void buildAstroroid(Planet planet = null)
        {
            cells = new List<Cell>();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    const int BEVEL = 1;
                    if ((j <= BEVEL || j >= Size - BEVEL) && (i <= BEVEL || i >= Size - BEVEL))
                    {
                        int chance = rand.Next(0, 10);
                        if (chance % 20 == 0)
                            makeCell(i, j);
                    }
                    else
                    {
                        makeCell(i, j);
                    }
                }
            }

            float velx = 0;
            float vely = 0;
            if (planet == null)
            {
                velx = (float)(rand.NextDouble() - .5) * 1f;
                vely = (float)(rand.NextDouble() - .5) * 1f;
            }
            else
            {
                float angle = Tool.angleTo(location, planet.Position);
                angle -= 1.57079f; // P/2
                float dis = Tool.distance2(location, planet.Position);
                float speed = (float)(rand.NextDouble() - .5) * 100000f/dis;
                velx = (float)Math.Cos(angle) * speed;
                vely = (float)Math.Sin(angle) * speed;
            }
            
            velocity = new Vector2(velx, vely);
            rotSpeed = (float)(rand.NextDouble() - .5) * 0.1357f;
        }

        protected void makeCell(int i, int j)
        {
            bool live = rand.Next() % 10 != 0;

            CellType ctype = getCellType();
            Cell tmp = new Cell(i - center.X, j - center.Y, 
                                  (i * CellSize) - CellSize / 2 + location.X, 
                                  (j * CellSize) + location.Y,
                                  ctype, live);
            Mass += tmp.Mass;

            cells.Add(tmp);
        }

        protected CellType getCellType()
        {
            int choice = rand.Next(0, 100);
            if (choice < 50)
            {
                return CellType.Fill;
            }
            if (choice < 80)
            {
                return CellType.Iorn;
            }
            if (choice < 90)
            {
                return CellType.Uranium;
            }
            else
            {
                return CellType.Gold;
            }
        }

        public bool testHit(Vector2 loc, int damage, PickupManager pickUps)
        {
            Cell hit = CellIntercect(loc,1,.4f);
            if (hit != null)
            {
                if (hit.Live)
                {
                    PickUp drop = hit.subHealthPickup(damage,(int)(CellSize - 2));
                    if (drop != null && hit.Type != CellType.Fill)
                    {
                        float offset = (float)Math.Atan2(hit.X, hit.Y);
                        float velx = (float)Math.Cos(rotSpeed + offset);
                        float vely = (float)Math.Sin(rotSpeed + offset);
                        drop.Velocity = new Vector2(velx, vely) + velocity;
                        pickUps.Add(drop);
                    }
                    return true;
                }
            }
            return false;
        }

        public void Update()
        {
            UpdateMovement();
            clearDead();
            rotation += rotSpeed;
        }

    }
}
