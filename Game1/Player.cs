using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Player
    {
        Ship ship;
        Vector2 holdLoc;
        
        BuildManager buildManager;

        Planet pDock;

        List<Button> selectors, sellers;
        Button launch;

        int credits;

        public Player(float startX, float startY, int screenWidth, int screenHeight)
        {
            ship = new Ship(0, new Vector2(startX, startY), 0);
            holdLoc = new Vector2(startX, startY);
            buildManager = new BuildManager(ship);
            pDock = null;
            credits = 0;

            const int SIDE_BUFFER = 10;
            const int TOP_BUFFER = 50;
            const int WIDTH = 260;
            const int HEIGHT = 50;

            // set Selector Buttons
            selectors = new List<Button>();
            sellers = new List<Button>();

            Button sHull = new Button(screenWidth - SIDE_BUFFER - WIDTH, TOP_BUFFER, WIDTH, HEIGHT, "Hull",CellType.Hull);
            Button sCab = new Button(screenWidth - SIDE_BUFFER - WIDTH, TOP_BUFFER* 2, WIDTH, HEIGHT, "Cab", CellType.Cab);
            Button sCargo = new Button(screenWidth - SIDE_BUFFER - WIDTH, TOP_BUFFER*3, WIDTH, HEIGHT, "Cargo", CellType.Cargo);
            Button sThruster = new Button(screenWidth - SIDE_BUFFER - WIDTH, TOP_BUFFER*4, WIDTH, HEIGHT, "Thruster", CellType.Thruster);
            Button sBlaster = new Button(screenWidth - SIDE_BUFFER - WIDTH, TOP_BUFFER * 5, WIDTH, HEIGHT, "Blaseter", CellType.Blaster);
            selectors.Add(sHull);
            selectors.Add(sCab);
            selectors.Add(sCargo);
            selectors.Add(sThruster);
            selectors.Add(sBlaster);

            // set control buttons
            launch = new Button(screenWidth - SIDE_BUFFER - WIDTH, TOP_BUFFER*6, WIDTH, HEIGHT, "Launch");

            // Set Seller buttons
            Button sellGold = new Button(SIDE_BUFFER, TOP_BUFFER, WIDTH, HEIGHT, "Sell Gold", CellType.Gold);
            Button sellIron = new Button(SIDE_BUFFER, TOP_BUFFER * 2, WIDTH, HEIGHT, "Sell Iorn", CellType.Iorn);
            Button sellUranium = new Button(SIDE_BUFFER, TOP_BUFFER * 3, WIDTH, HEIGHT, "Sell Uranium", CellType.Uranium);
            sellers.Add(sellGold);
            sellers.Add(sellIron);
            sellers.Add(sellUranium);
        }

        public Ship getShip()
        {
            return ship;
        }

        public Screen inputControlBuild()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.C))
                buildManager.Selected = CellType.Cab;
            if (Keyboard.GetState().IsKeyDown(Keys.H))
                buildManager.Selected = CellType.Hull;
            if (Keyboard.GetState().IsKeyDown(Keys.T))
                buildManager.Selected = CellType.Thruster;
            if (Keyboard.GetState().IsKeyDown(Keys.B))
                buildManager.Selected = CellType.Blaster;
            if (Keyboard.GetState().IsKeyDown(Keys.G))
                buildManager.Selected = CellType.Cargo;

            CellType bSel = buttonSelect(Mouse.GetState());
            if (bSel != CellType.Fill)
            {
                buildManager.Selected = bSel;
            }

            sellButtonPress(Mouse.GetState());

            Vector2 mouseVec = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            bool leftMouse = Mouse.GetState().LeftButton == ButtonState.Pressed;
            bool rightMouse = Mouse.GetState().RightButton == ButtonState.Pressed;

            buildManager.GetMouse(mouseVec, leftMouse, rightMouse);
            buildManager.building();

            launch.Update(Mouse.GetState());
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || launch.Activated(Mouse.GetState()))
            {
                finishBuild();
                return Screen.play;
            }

            return Screen.build;
        }

        public Screen inputControlPlay(float buildX, float buildY)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                ship.thrust();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                ship.turnLeft();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                ship.turnRight();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                ship.shoot();
            }

            if (pDock != null && Keyboard.GetState().IsKeyDown(Keys.E))
            {
                startBuild(buildX, buildY);
                return Screen.build;
            }
            return Screen.play;
        }

        private void setButtonValues()
        {
            foreach (Button sell in sellers)
            {
                CellType temp = sell.TypeSelector;

                if (pDock != null)
                {
                    foreach (var val in pDock.ValuesList)
                    {
                        if (temp == val.Type)
                        {
                            sell.Value = val.Value * ship.getAmountByType(temp);
                        }
                    }
                }
            }
        }

        private void sellButtonPress(MouseState mouse)
        {
            foreach (Button button in sellers)
            {
                button.Update(mouse);
                CellType type = button.TypeSelector;
                if (button.Activated(mouse))
                {
                    ship.removeItemByType(type);
                    credits += button.Value;
                    button.Value = 0;
                }
            }
        }

        private CellType buttonSelect(MouseState mouse)
        {
            foreach (Button button in selectors)
            {
                button.Update(mouse);
                CellType sel = button.GetCellType(mouse);
                if (sel != CellType.Fill)
                {
                    return sel;
                }
            }
            return CellType.Fill;
        }

        public void Update(PickupManager pickups,PlanetColecction planets)
        {
            ship.Update(pickups);

            pDock = null;
            foreach (var planet in planets.Planets)
            {
                

                if (Tool.distance2(ship.Locaion, planet.Position) < planet.Size)
                {
                    pDock = planet;
                }
            }
        }

        public void startBuild(float x, float y)
        {
            holdLoc = ship.Locaion;
            ship.clearDead();
            ship.expand(64);
            ship.Locaion = new Vector2(x,y);
            setButtonValues();
            //ship.cellSet();
        }

        public void finishBuild()
        {
            ship.setCenter();
            ship.calculateValues();
            ship.Locaion = holdLoc;
        }

        public void DrawPlay(SpriteBatch spriteBatch, Texture2D fill,SpriteFont font)
        {
            ship.RenderShip(spriteBatch, fill);
            ship.renderStats(spriteBatch, font);
        }

        public void DrawBuild(SpriteBatch spriteBatch, Texture2D fill,SpriteFont font)
        {
            ship.RenderBuild(spriteBatch, fill);

            foreach(var button in selectors)
            {
                button.Draw(spriteBatch, fill, font);
            }

            foreach(var sell in sellers)
            {
                sell.Draw(spriteBatch, fill, font, true);
            }

            launch.Draw(spriteBatch, fill, font);
        }
        
        public void DrawHud(int x, int y, SpriteBatch spriteBatch, Texture2D fill, Texture2D sphere,SpriteFont font,AstroroidManager astroroids, PlanetColecction planets)
        {
            DrawRadar(spriteBatch, fill,sphere, ship.Locaion, x, y, astroroids.GetAstroroidList(),planets.Planets);
            spriteBatch.DrawString(font, "Credits: " + credits, new Vector2(20, 20), Color.Green);
        }

        private void DrawRadar(SpriteBatch spriteBatch, Texture2D texture, Texture2D sphere, Vector2 scanCenter, int x, int y,List<Astroroid> roids, List<Planet> planets)
        {
            const float SCALE = 0.015f;
            const float RANGE = 6000f;
            const float BORDER = 2f;
            spriteBatch.Draw(texture, new Rectangle(x - (int)(SCALE * RANGE + BORDER), y - (int)(SCALE * RANGE + BORDER), (int)((SCALE * RANGE + BORDER) * 2), (int)((SCALE * RANGE + BORDER) * 2)), Color.Green);
            spriteBatch.Draw(texture, new Rectangle(x - (int)(SCALE * RANGE), y - (int)(SCALE * RANGE), (int)(SCALE * RANGE * 2), (int)(SCALE * RANGE * 2)), Color.Black);

            // render Planets
            foreach (var planet in planets)
            {
                Vector2 offset = scanCenter - new Vector2(planet.Position.X, planet.Position.Y);

                if (Math.Abs(offset.Y) < RANGE && Math.Abs(offset.X) < RANGE)
                {
                    Rectangle renderPlanet = radarRec(x, y, offset, SCALE, (int)(planet.Size*SCALE));
                    spriteBatch.Draw(sphere, renderPlanet, null, Color.White, 0f, new Vector2(sphere.Width / 2, sphere.Height / 2), SpriteEffects.None, 1f);
                }
            }

            // render Astroroids
            foreach (var roid in roids)
            {
                Vector2 offset = scanCenter - roid.Locaion;

                if (Math.Abs(offset.Y) < RANGE && Math.Abs(offset.X) < RANGE)
                {
                    Rectangle renderRoid = radarRec(x, y, offset, SCALE, 4);
                    spriteBatch.Draw(texture, renderRoid, Color.Gray);
                }
            }

            // render ship loation
            spriteBatch.Draw(texture, new Rectangle(x - 2, y - 2, 4, 4), Color.Green);
        }
        private Rectangle radarRec(int x, int y, Vector2 offset, float scl,int size)
        {
            return new Rectangle((int)(-offset.X * scl) + x, (int)(-offset.Y * scl) + y, size, size);
        }
    }
}
