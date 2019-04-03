using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class PickUp : Item
    {
        public Vector2 Location { get; protected set; }
        public Vector2 Velocity { get; set; }
        public Cell Cell { get; protected set; }

        public PickUp(CellType type, int amount, Vector2 location, Vector2 velocity)
        {
            Type = type;
            Amount = amount;
            Location = location;
            Velocity = velocity;
            Cell = new Cell(type);
        }

        public void Update()
        {
            Location += Velocity;
        }

        public void MoveTo(Vector2 target, float speed)
        {
            Vector2 targetVec = Tool.getUnitVector(Location, target);
            Velocity = targetVec * speed;
        }

        public Item GetItem()
        {
            return new Item(Type, Amount);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            int size = 16;
            Rectangle rectangle = new Rectangle((int)(Location.X + size / 2), (int)(Location.Y + size / 2), size, size);
            spriteBatch.Draw(texture, rectangle, Cell.Tint);
        }
    }
}
