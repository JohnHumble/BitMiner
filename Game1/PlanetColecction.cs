using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class PlanetColecction
    {
        public List<Planet> Planets { get; protected set; }
        public PlanetColecction()
        {
            Planets = new List<Planet>();

            createPlanets();
        }

        public void createPlanets()
        {
            Planet planet = new Planet(0, 0, 1024, 1024f);
            Planets.Add(planet);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D sphere)
        {
            foreach(var planet in Planets)
            {
                planet.Draw(spriteBatch, sphere);
            }
        }
    }
}
