using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Tool
    {
        public static float distance(Vector2 i, Vector2 j)
        {
            Vector2 tmp = j - i;
            return Math.Abs(tmp.X) + Math.Abs(tmp.Y);
        }
        
        public static float distance(Vector2 i, Point j)
        {
            return distance(i, new Vector2(j.X,j.Y));
        }

        public static float distance2(Vector2 i, Vector2 j)
        {
            return magnitude(j - i);
        }
        public static float distance2(Vector2 i, Point j)
        {
            return magnitude(new Vector2(j.X, j.Y) - i);
        }

        public static Vector2 getPlanetGravity(Planet planet, Vector2 location, float mass)
        {
            Vector2 accel = getUnitVector(location, planet.Position);
            float dis = distance2(location, planet.Position);
            
            return accel * planet.Gravity * mass / dis / dis;
        }

        public static Vector2 getUnitVector(Vector2 location, Vector2 target)
        {
            float velx = target.X - location.X;
            float vely = target.Y - location.Y;

            float reduce = magnitude(new Vector2(velx, vely));

            return new Vector2(velx / reduce, vely / reduce);
        }

        public static Vector2 getUnitVector(Vector2 location, Point target)
        {
            return getUnitVector(location, new Vector2(target.X, target.Y));
        }

        public static float angleTo(Vector2 location, Vector2 target)
        {
            Vector2 vec = getUnitVector(location, target);
            return (float)Math.Atan2(vec.Y, vec.X);
        }

        public static float angleTo(Vector2 location, Point target)
        {
            Vector2 vec = getUnitVector(location, target);
            return (float)Math.Atan2(vec.Y, vec.X);
        }

        public static float magnitude(Vector2 vec)
        {
            return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
        }

    }
}
