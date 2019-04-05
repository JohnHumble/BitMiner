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

            setValues();
        }

        public Planet(int x, int y, int size, float gravity)
        {
            Position = new Point(x, y);
            Size = size;
            Gravity = gravity;
            Dockable = true;

            ValuesList = new List<Cell>();

            setValues();
        }

        private void setValues()
        {
            ValuesList = new List<Cell>();
            //TODO add some things to buy here.
            Cell goldVal = new Cell(CellType.Gold);
            goldVal.Value = 50;
            ValuesList.Add(goldVal);
            Cell ironVal = new Cell(CellType.Iorn);
            ironVal.Value = 15;
            ValuesList.Add(ironVal);
            Cell uraniumVal = new Cell(CellType.Uranium);
            uraniumVal.Value = 20;
            ValuesList.Add(uraniumVal);
            Cell cabVal = new Cell(CellType.Cab);
            cabVal.Value = 40;
            ValuesList.Add(cabVal);
            Cell hullVal = new Cell(CellType.Hull);
            hullVal.Value = 10;
            ValuesList.Add(hullVal);
            Cell thrustVal = new Cell(CellType.Thruster);
            thrustVal.Value = 20;
            ValuesList.Add(thrustVal);
            Cell blasterVal = new Cell(CellType.Blaster);
            blasterVal.Value = 20;
            ValuesList.Add(blasterVal);
            Cell cargoVal = new Cell(CellType.Cargo);
            cargoVal.Value = 20;
            ValuesList.Add(cargoVal);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D sphere)
        {
            spriteBatch.Draw(sphere, new Rectangle(Position, new Point(Size, Size)), null, Color.Blue, 0f, new Vector2(sphere.Width/2, sphere.Height/2), SpriteEffects.None, 1f);
        }
    }
}
