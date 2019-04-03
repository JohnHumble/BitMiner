using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class PlasmaShot
    {
        Vector2 velocity;
        float rotation;
        int size;
        public int Life { get; private set; }
        public int Damage { get; set; }

        public Vector2 Location { get; private set; }

        public PlasmaShot(Vector2 location, Vector2 velocity, int size, int damage, int life = 360)
        {
            this.Life = life;
            this.Location = location;
            this.size = size;
            this.velocity = velocity;
            Damage = damage;
            rotation = (float)Math.Atan2(velocity.Y, velocity.X);
        }

        public void Update()
        {
            Location += velocity;
            Life--;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D fill)
        {
            Rectangle rend = new Rectangle((int)Location.X, (int)Location.Y, size, size);
            spriteBatch.Draw(fill, rend, null, Color.Red, rotation, new Vector2(fill.Width * 0.5f,fill.Height * 0.5f), SpriteEffects.None, 0f);
        }
    }
}
