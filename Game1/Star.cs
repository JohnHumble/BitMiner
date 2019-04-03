using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Star
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int Size { get; set; }
        public Color C { get; set; }
        
        public Star()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Star(float x, float y, float z, int size, Color c)
        {
            X = x;
            Y = y;
            Z = z;
            C = c;
            Size = size;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 offset)
        {
            float differenceX = (X - offset.X) / Z;
            float differenceY = (Y - offset.Y) / Z;

            Rectangle rec = new Rectangle((int)(offset.X + differenceX), (int)(offset.Y + differenceY), Size, Size);

            spriteBatch.Draw(texture, rec, C);
        }

    }
}
