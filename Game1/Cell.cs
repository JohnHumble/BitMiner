using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BitMiner
{
    enum CellType { Hull, Cab, Thruster, Blaster, Cargo, Fill, Iorn, Gold, Uranium }

    class Cell
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float LocX { get; set; }
        public float LocY { get; set; }

        public bool Live { get; set; }
        public int Health { get; set; }
        public int Value { get; set; }

        public CellType Type { get; set; }

        public float Power { get; set; }
        public float Mass { get; set; }
        public float Thrust { get; set; }
        public int Capacity { get; set; }

        public Color Tint { get; set; }

        public Cell(CellType type)
        {
            X = 0;
            Y = 0;
            LocX = 0;
            LocY = 0;
            Live = false;
            Type = type;
            setValuesByType();
        }

        public Cell(float x, float y, float locX, float locY)
        {
            X = x;
            Y = y;
            LocX = locX;
            LocY = locY;

            Live = false;
            Type = CellType.Fill;
            setValuesByType();
        }

        public Cell(float x, float y, float locX, float locY, CellType type, bool live)
        {
            X = x;
            Y = y;
            LocX = locX;
            LocY = locY;

            Live = live;
            Type = type;
            setValuesByType();
        }

        public Cell(Cell old, CellType type, bool live)
        {
            Live = live;
            X = old.X;
            Y = old.Y;
            LocX = old.LocX;
            LocY = old.LocY;
            Type = type;
            setValuesByType();
        }

        private void setValuesByType()
        {
            switch (Type)
            {
                case CellType.Hull:
                    Tint = Color.Gray;
                    Health = 10;
                    Mass = 1;
                    Power = 0;
                    Thrust = 0;
                    Capacity = 0;
                    break;

                case CellType.Thruster:
                    Tint = Color.DarkGray;
                    Health = 10;
                    Mass = 1;
                    Power = 0.5f;
                    Thrust = 2f;
                    Capacity = 0;
                    break;

                case CellType.Cab:
                    Tint = Color.Goldenrod;
                    Health = 10;
                    Mass = 1;
                    Power = 1;
                    Thrust = .1f;
                    Capacity = 2;
                    break;

                case CellType.Cargo:
                    Tint = Color.CornflowerBlue;
                    Health = 10;
                    Mass = 1;
                    Power = 0;
                    Thrust = 0;
                    Capacity = 10;
                    break;

                case CellType.Blaster:
                    Tint = Color.Red;
                    Health = 10;
                    Mass = 1;
                    Power = 0;
                    Thrust = 0;
                    Capacity = 0;
                    break;

                case CellType.Fill:
                    Tint = Color.Gray;
                    Health = 10;
                    Mass = 1;
                    Power = 0;
                    Thrust = 0;
                    Capacity = 0;
                    break;

                case CellType.Gold:
                    Tint = Color.Gold;
                    Health = 10;
                    Mass = 1;
                    Power = 0;
                    Thrust = 0;
                    Capacity = 0;
                    break;

                case CellType.Iorn:
                    Tint = Color.LightGray;
                    Health = 10;
                    Mass = 1;
                    Power = 0;
                    Thrust = 0;
                    Capacity = 0;
                    break;

                case CellType.Uranium:
                    Tint = Color.DarkGreen;
                    Health = 10;
                    Mass = 1;
                    Power = 0;
                    Thrust = 0;
                    Capacity = 0;
                    break;

                default:
                    Tint = Color.Black;
                    break;
            }
        }

        public void reset()
        {
            setValuesByType();
        }

        public void subHealth(int value)
        {
            Health -= value;
            if (Health <= 0)
            {
                Live = false;
                Health = 0;
            }
        }

        public PickUp subHealthPickup(int value, int salvage)
        {
            subHealth(value);
            if (!Live)
            {
                return new PickUp(Type, salvage, new Vector2(LocX, LocY), new Vector2(0, 0));
            }
            return null;
        }

        public Color GetColor()
        {
            return Tint;
        }
        
        public Vector2 Location()
        {
            return new Vector2(LocX, LocY);
        }
    }
}
