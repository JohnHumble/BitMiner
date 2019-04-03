using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Station : Unit
    {

        public Station(Vector2 location)
        {
            Locaion = location;

            velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 0);

            CellSize = 32;
            Size = 16;
            this.location = location;
            rotation = 0f;

            center = new Vector2(Size * .5f - .5f, Size * .5f - .5f);
            initializeCells(false);
            buildStation();
            cellSet();
        }

        public void buildStation()
        {
            // TODO put some thing here.
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            RenderUnit(spriteBatch, texture);
        }
    }
}
