using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class StarManager
    {
        List<Star> stars;
        Camera camera;
        Random rand;

        int depth;

        public StarManager(Camera camera)
        {
            this.camera = camera;
            stars = new List<Star>();
            rand = new Random();
            depth = 2000 * (int)(1 / camera.scale);

            initialize(200, -depth, depth);
        }

        public void update()
        {
            foreach (var star in stars)
            {
                float x = camera.getMid().X - star.X;
                float y = camera.getMid().Y - star.Y;

                if (x > depth)
                {
                    star.X += depth * 2;
                }
                if (x < -depth)
                {
                    star.X -= depth * 2;
                }
                if (y > depth)
                {
                    star.Y += depth * 2;
                }
                if (y < -depth)
                {
                    star.Y -= depth * 2;
                }
            }
        }

        private void initialize(int count, int min, int max)
        {
            for (int i = 0; i < count; i++)
            {
                //float mid = (max + min) / 2;

                float x = (rand.Next(min, max)) - camera.getMid().X;
                float y = (rand.Next(min, max)) - camera.getMid().Y;
                float z = (float)rand.NextDouble() + 1f;
                int size = rand.Next(2, 6);

                Color color = Color.Black;
                color.R = (byte)rand.Next(210, 256);
                color.G = (byte)rand.Next(210, 256);
                color.B = (byte)rand.Next(210, 256);

                Star nStar = new Star(x,y,z,size,color);
                stars.Add(nStar);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            foreach (var star in stars)
            {
                star.Draw(spriteBatch, texture, camera.getMid());
            }
        }
    }
}
