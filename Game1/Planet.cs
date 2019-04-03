using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Planet
    {
        public Point Position { get; protected set; }
        public int Size { get; protected set; }
        public float Gravity { get; protected set; }
        public bool Dockable { get; protected set; }
        public List<Cell> ValuesList { get; set; }

        public Planet(Point position,int size, float gravity,bool dockable = true)
        {
            Position = position;
            Size = size;
            Gravity = gravity;
            Dockable = dockable;

            ValuesList = new List<Cell>();

            Cell goldVal = new Cell(CellType.Gold);
            goldVal.Value = 50;
            Cell ironVal = new Cell(CellType.Iorn);
            ironVal.Value = 15;
            Cell uraniumVal = new Cell(CellType.Uranium);
            uraniumVal.Value = 20;
            ValuesList.Add(goldVal);
            ValuesList.Add(ironVal);
            ValuesList.Add(uraniumVal);
        }

        public Planet(int x, int y, int size, float gravity)
        {
            Position = new Point(x, y);
            Size = size;
            Gravity = gravity;
            Dockable = true;

            ValuesList = new List<Cell>();
            
            Cell goldVal = new Cell(CellType.Gold);
            goldVal.Value = 50;
            Cell ironVal = new Cell(CellType.Iorn);
            ironVal.Value = 15;
            Cell uraniumVal = new Cell(CellType.Uranium);
            uraniumVal.Value = 20;
            ValuesList.Add(goldVal);
            ValuesList.Add(ironVal);
            ValuesList.Add(uraniumVal);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D sphere)
        {
            spriteBatch.Draw(sphere, new Rectangle(Position, new Point(Size, Size)), null, Color.Blue, 0f, new Vector2(sphere.Width/2, sphere.Height/2), SpriteEffects.None, 1f);
        }
    }
}
