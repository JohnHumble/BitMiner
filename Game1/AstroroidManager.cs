using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class AstroroidManager
    {
        Random rand;

        List<Astroroid> roids;
        public AstroroidManager()
        {
            roids = new List<Astroroid>();
            rand = new Random();
        }

        public List<Astroroid> GetAstroroidList()
        {
            return roids;
        }

        public void createAstroroids(int number, Planet planet = null)
        {
            for (int i = 0; i < number; i++)
            {
                const int SPAWN_RANGE = 10000;
                float x = rand.Next(-SPAWN_RANGE, 10000);
                float y = rand.Next(-SPAWN_RANGE, SPAWN_RANGE);

                createAstroroid(x,y,planet);
            }
        }

        private void createAstroroid(float x, float y,Planet planet)
        {
            Vector2 loc = new Vector2(x, y);

            Astroroid roid = new Astroroid(8,16,loc,rand,planet);
            roids.Add(roid);
        }

        public void update(PickupManager pickUps, Ship player, PlanetColecction planets)
        {
            for (int i = 0; i < roids.Count; i++)
            {
                roids[i].Update();

                // calculate force of gravity
                roids[i].setAcceleration(new Vector2(0,0));
                float disPlanet = float.MaxValue; // distance to closest planet;
                foreach (var planet in planets.Planets)
                {
                    float disP = Tool.distance(roids[i].Locaion, planet.Position);
                    if (disPlanet > disP)
                        disPlanet = disP;
                    if (Tool.magnitude(roids[i].Acceleration) < 10f)
                    {
                        Vector2 planetAcel = Tool.getPlanetGravity(planet, roids[i].Locaion,roids[i].Mass);
                        roids[i].addAcceleration(planetAcel);
                    }
                }

                float distancePlayer = Tool.distance(roids[i].Locaion, player.Locaion);
                
                // test damage
                if (distancePlayer < 1000f)
                {
                    for (int j = 0; j < player.Shot.Count; j++)
                    {
                        float distanceShot = Tool.distance(roids[i].Locaion, player.Shot[j].Location);

                        if (distanceShot < roids[i].Size * roids[i].CellSize * 1.5f)
                        {
                            if (roids[i].testHit(player.Shot[j].Location, player.Shot[j].Damage, pickUps))
                            {
                                player.removeShot(j);
                                j--;
                            }
                        }
                    }
                }

                if (!roids[i].Live || disPlanet > 80000f)
                {
                    roids.RemoveAt(i);
                    i--;
                    continue;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            foreach (var roid in roids)
            {
                roid.RenderUnit(spriteBatch, texture);
            }
        }
        

        public int Count()
        {
            return roids.Count();
        }

    }
}
