using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Unit
    {
        public int Size { get; protected set; } 
        public int CellSize { get; protected set; }

        protected List<Cell> cells;
        protected Vector2 location, center, velocity;
        public Vector2 Acceleration { get; protected set; }
        protected float rotation;
        public bool Live { get; protected set; }

        public Vector2 Locaion { get { return location; } set{ location = value; } }

        public Unit()
        {
            Size = 0;
            CellSize = 0;
            velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 0);
            CellSize = 8;
            center = new Vector2(Size * .5f - .5f, Size * .5f - .5f);
        }
        
        public Unit(int size, int cellSize)
        {
            this.Size = size;
            this.CellSize = cellSize;
            velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 0);
            center = new Vector2(size * .5f - .5f, size * .5f - .5f);
            initializeCells(false);
        }

        protected void initializeCells(bool live)
        {
            cells = new List<Cell>();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Cell tmp = new Cell(i - center.X, j - center.Y, (i * CellSize) - CellSize / 2 + location.X, (j * CellSize) + location.Y);
                    tmp.Live = live;
                    cells.Add(tmp);
                }
            }
        }

        public void UpdateMovement()
        {
            velocity += Acceleration;
            location += velocity;
            cellSet();
            clearDead();
        }

        public void setAcceleration(Vector2 accel)
        {
            Acceleration = accel;
        }
        public void addAcceleration(Vector2 accel)
        {
            Acceleration += accel;
        }

        public void cellSet()
        {
            cellSet(1, rotation);
        }

        public void cellSet(int scale, float rot)
        {
            foreach (var cell in cells)
            {
                double offsetRot = Math.Atan2(cell.Y, cell.X);
                double distance = Math.Sqrt(cell.X * cell.X + cell.Y * cell.Y) * CellSize * scale;

                cell.LocX = location.X + (float)(Math.Cos(rot - offsetRot) * distance);
                cell.LocY = location.Y + (float)(Math.Sin(rot - offsetRot) * distance);
            }
        }

        public void clearDead()
        {
            for (int i = 0; i < cells.Count(); i++)
            {
                if (!cells[i].Live)
                {
                    cells.RemoveAt(i);
                    i--;
                }
            }
            
            Live = cells.Count > 0;
        }


        public void restGrid(int size)
        {
            if (cells.Count == 0)
            {
                cells.Add(new Cell(center.X, center.Y, location.X, location.Y));
            }
            Cell cell = cells[0];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    
                    Cell tmp = new Cell(i - center.X, j - center.Y, (i * CellSize) - CellSize / 2 + location.X, (j * CellSize) + location.Y);
                    cells.Add(tmp);
                }
            }
        }

        public void expand(int size)
        {
            if (cells.Count == 0)
            {
                cells.Add(new Cell(center.X, center.Y, location.X, location.Y));
            }

            cellSet(1, 0f);
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var cell in cells)
            {
                if (cell.X < minX)
                {
                    minX = cell.X;
                }
                if (cell.Y < minY)
                {
                    minY = cell.Y;
                }

                if (cell.X > maxX)
                {
                    maxX = cell.X;
                }
                if (cell.Y > maxY)
                {
                    maxY = cell.Y;
                }
            }
            float x = minX - (int)((size - maxX + minX)/2);
            float y = minY - (int)((size - maxY + minY)/2);


            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Vector2 test = new Vector2(x + i, y + j);

                    if (empty(test))
                    {
                        cells.Add(new Cell(test.X, test.Y, (x * CellSize) - CellSize / 2 + location.X, (y * CellSize) + location.Y));
                    }
                }
            }
                
        }

        public void setCenter()
        {
            clearDead();

            float avgX = 0;
            float avgY = 0;
            foreach (var cell in cells)
            {
                avgX += cell.X;
                avgY += cell.Y;
            }
            avgX /= cells.Count();
            avgY /= cells.Count();

            center = new Vector2(avgX, avgY);

            foreach (var cell in cells)
            {
                cell.X -= avgX;
                cell.Y -= avgY;
            }

            Size = (int)Math.Max(avgX, avgY) * 2;
        }

        private bool empty(Vector2 test)
        {
            foreach (var cellother in cells)
            {
                if (Tool.distance(test, new Vector2(cellother.X,cellother.Y)) < 0.5f)
                {
                    return false;
                }
            }
            return true;
        }

        public void RenderUnit(SpriteBatch spriteBatch, Texture2D fill)
        {
            foreach (var cell in cells)
            {
                if (cell.Live)
                {
                    Rectangle RenderRec = new Rectangle((int)(location.X), (int)(location.Y), CellSize, CellSize);
                    Vector2 origin = new Vector2(-cell.X + 0.5f, cell.Y + 0.5f);

                    spriteBatch.Draw(fill, RenderRec, null, cell.GetColor(), rotation, origin, SpriteEffects.None, 0f);
                    // spriteBatch.Draw(fill, new Vector2((float)cell.LocX, (float)cell.LocY), Color.Red);
                }
            }
        }

        /// <summary>
        /// This method returns a cell if the vector loc is close to it.
        /// </summary>
        /// <param name="x"> x quardnant </param>
        /// <param name="y"> y quardnat </param>
        /// <returns> The cell that covers the point, null in there is no cell. </returns>
        public Cell CellIntercect(Vector2 loc, int scale = 1, float buff = 0.5f)
        {
            foreach (var cell in cells)
            {
                if (Tool.distance(cell.Location(), loc) <= CellSize * buff  * scale)
                {
                    return cell;
                }
            }
            return null;
        }

        public List<Cell> GetCellList()
        {
            return cells;
        }
    }
}
