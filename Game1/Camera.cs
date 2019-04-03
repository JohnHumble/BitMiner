using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Camera
    {
        public Vector2 location;
        public float scale;
        private Matrix transform;

        private int screenWidth, screenHeight;

        // initalize
        public Camera(Vector2 loc)
        {
            location = loc;
            scale = 1;
            transform = new Matrix();
        }
        public Camera(Vector2 loc, float scl)
        {
            location = loc;
            scale = scl;
            transform = new Matrix();
        }
        public Camera(float x, float y, float scl = 1)
        {
            location = new Vector2(x, y);
            scale = scl;
            transform = new Matrix();
        }
        public Camera(float x, float y, float scl, int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            location = new Vector2(x, y);
            scale = scl;
            transform = new Matrix();
        }

        public Vector2 getMid()
        {
            float midX = -location.X + (screenWidth / 2) / scale;
            float midY = -location.Y + (screenHeight / 2) / scale;
              return new Vector2(midX, midY);
            //return location;
        }

        // change location.
        public void setLocation(Vector2 loc)
        {
            loc.X *= -1;
            loc.Y *= -1;
            location = loc;
        }
        public void setLocation(float x, float y)
        {
            location = new Vector2(x * -1, y * -1);
        }

        // change scale
        public void setScale(float Scale)
        {
            scale = Scale;
        }

        // give matrix. use this in draw method
        public Matrix getTransformation()
        {
            transform = Matrix.CreateTranslation(new Vector3(location.X, location.Y, 0)) * Matrix.CreateScale(scale);
            return transform;
        }
    }
}
