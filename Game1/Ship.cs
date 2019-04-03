using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Ship : Unit
    {
        bool enginThrust;
        List<Cell> blasters;
        int shotCooldown;

        protected float mass, thrustSpeed, power;
        protected int cargo, usedCargo;

        public List<PlasmaShot> Shot { get; }
        public List<Item> items;

        public Ship(int size, Vector2 location, float rotation)
        {
            enginThrust = false;
            Shot = new List<PlasmaShot>();
            blasters = new List<Cell>();
            items = new List<Item>();
            shotCooldown = 0;
            mass = 0;
            thrustSpeed = 0;
            power = 0;
            cargo = 0;
            usedCargo = 0;

            velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 0);
            CellSize = 8;
            this.location = location;
            this.rotation = rotation;
            Size = size;
            center = new Vector2(size * .5f - .5f, size * .5f - .5f);
            initializeCells(false);
            calculateValues();
        }

        public void calculateValues()
        {
            blasters.Clear();
            mass = thrustSpeed = power = 0;
            foreach (var cell in cells)
            {
                if (cell.Live)
                {
                    mass += cell.Mass;
                    thrustSpeed += cell.Thrust;
                    power += cell.Power;
                    cargo += cell.Capacity;

                    if (cell.Type == CellType.Blaster)
                    {
                        blasters.Add(cell);
                    }
                }
            }
            foreach (var item in items)
            {
                usedCargo += item.Amount;
            }
        }

        public void Update(PickupManager pickups)
        {
            UpdateMovement();

            if (shotCooldown > 0)
                shotCooldown--;

            for (int i = 0; i < Shot.Count; i++)
            {
                Shot[i].Update();
                if (Shot[i].Life < 0)
                {
                    Shot.RemoveAt(i);
                    i--;
                }
            }

            if (usedCargo < cargo)
            {
                for (int i = 0; i < pickups.Count; i++)
                {
                    float distance = Tool.distance(location, pickups.Index(i).Location);
                    if (distance < 50)
                    {
                        addItem(pickups.CollectAt(i));
                        continue;
                    }

                    if (distance < 400)
                    {
                        pickups.Index(i).MoveTo(Locaion, 10f);
                    }
                }
            }
        }

        private void addItem(Item adition)
        {
            if (usedCargo < cargo)
            {
                foreach (var item in items)
                {
                    if (item.Type == adition.Type)
                    {
                        item.Add(adition.Amount);
                        usedCargo += adition.Amount;
                        return;
                    }
                }
                items.Add(adition);
                usedCargo += adition.Amount;
            }
        }

        public int getAmountByType(CellType type)
        {
            foreach (var item in items)
            {
                if (item.Type == type)
                {
                    return item.Amount;
                }
            }
            return 0;
        }

        public int removeItemByType(CellType type)
        {
            foreach (var item in items)
            {
                if (item.Type == type)
                {
                    int amount = item.Amount;
                    items.Remove(item);
                    return amount;
                }
            }
            return 0;
        }

        public void removeShot(int index)
        {
            Shot.RemoveAt(index);
        }

        public void thrust()
        {
            enginThrust = true;
            float speed = thrustSpeed / mass;
            Vector2 acc = new Vector2(speed * (float)Math.Cos(rotation), speed * (float)Math.Sin(rotation));
            velocity += acc;
        }

        const float TURN_REDUCE = 0.31415f; 

        public void turnLeft()
        {
            rotation -= thrustSpeed / mass * TURN_REDUCE;
        }

        public void turnRight()
        {
            rotation += thrustSpeed / mass * TURN_REDUCE;
        }

        const int SHOTPOWER = 20;
        public void shoot()
        {
            if (shotCooldown <= 0)
            {
                foreach (var blast in blasters)
                {
                    if (blast.Live)
                    {
                        float shotSpeed = 10;
                        float velX = (float)Math.Cos(rotation) * shotSpeed + velocity.X;
                        float velY = (float)Math.Sin(rotation) * shotSpeed + velocity.Y;
                        PlasmaShot shot = new PlasmaShot(blast.Location(), new Vector2(velX, velY), 3 * CellSize / 4,7);
                        Shot.Add(shot);
                    }
                }
                shotCooldown = (int)(SHOTPOWER * blasters.Count / power);
            }
        }

        public void RenderShip(SpriteBatch spriteBatch, Texture2D fill)
        {
            foreach (var cell in cells)
            {
                if (cell.Live)
                {
                    Rectangle RenderRec = new Rectangle((int)(location.X), (int)(location.Y), CellSize, CellSize);
                    Vector2 origin = new Vector2(-cell.X + 0.5f, cell.Y + 0.5f);

                    spriteBatch.Draw(fill, RenderRec, null, cell.GetColor(), rotation, origin, SpriteEffects.None, 0f);
                    // spriteBatch.Draw(fill, new Vector2((float)cell.LocX, (float)cell.LocY), Color.Red);
                    if (cell.Type == CellType.Thruster && enginThrust)
                    {
                        // Take a loook here to see what is going on.
                        RenderRec = new Rectangle((int)(location.X), (int)(location.Y), CellSize /2, CellSize);
                        origin = new Vector2(-cell.X * 2 + 1f, cell.Y + 0.5f);
                        //  origin.X += 3f;
                        spriteBatch.Draw(fill, RenderRec, null, Color.OrangeRed, rotation, origin, SpriteEffects.None, 0f);
                    }
                }
            }
            if (enginThrust)
                enginThrust = false;

            foreach (var shot in Shot)
            {
                shot.Draw(spriteBatch, fill);
            }
        }

        public void RenderBuild(SpriteBatch spriteBatch, Texture2D fill)
        {
            foreach (var cell in cells)
            {
                Color col;
                if (cell.Live)
                {
                    col = cell.GetColor();
                }
                else
                {
                    if (((int)(cell.X + Size) % 2 == 0 && (int)(cell.Y + Size) % 2 == 0) || ((int)(cell.X + Size) % 2 == 1 && (int)(cell.Y + Size) % 2 == 1))
                        col = new Color(0, 80, 0);
                    else
                        col = new Color(0, 0, 80);
                }
                Rectangle RenderRec = new Rectangle((int)(location.X), (int)(location.Y), CellSize * 2, CellSize * 2);
                Vector2 origin = new Vector2(-cell.X + 0.5f, cell.Y + 0.5f);

                spriteBatch.Draw(fill, RenderRec, null, col, 0f, origin, SpriteEffects.None, 0f);
                spriteBatch.Draw(fill, cell.Location(), Color.Yellow);
            }
        }

        public void renderStats(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //  spriteBatch.DrawString(spriteFont, "M:" + mass, location, Color.Yellow);
            //  spriteBatch.DrawString(spriteFont, "P:" + power, location + new Vector2(0, 15), Color.Magenta);
            //   spriteBatch.DrawString(spriteFont, "T:" + thrustSpeed, location + new Vector2(0, 30), Color.Magenta);
            //   spriteBatch.DrawString(spriteFont, "V:" + velocity, location + new Vector2(0, 45), Color.Blue);
            for (int i = 0; i < items.Count; i++)
            {
                spriteBatch.DrawString(spriteFont, "Cargo" +i+":"+ items[i].Type+"-"+items[i].Amount, location + new Vector2(0,i*30), Color.Beige);

            }
        }
    }
}
