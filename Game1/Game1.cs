using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BitMiner
{
    enum Screen { play,build}

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        Station station;
        
        Texture2D fill;
        Texture2D sphere;
        SpriteFont smallFont;
        PlanetColecction planets;
        AstroroidManager astroroids;
        PickupManager pickups;


        StarManager stars;
        Camera camera;

        Screen screen;

        int screenWidth, screenHeight;
        
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            screenHeight = 800;
            screenWidth = 1600;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
            player = new Player(0,0,screenWidth,screenHeight);
            player.startBuild(screenWidth * .5f, screenHeight * .5f);

            station = new Station(new Vector2(0, 0));
            planets = new PlanetColecction();

            pickups = new PickupManager();
            camera = new Camera(screenWidth * 0.5f, screenHeight * .5f, 0.75f,screenWidth,screenHeight);
            
            astroroids = new AstroroidManager();
            stars = new StarManager(camera);
            screen = Screen.build;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fill = loadColorTexture(Color.White);
            sphere = Content.Load<Texture2D>("Sphere");

            smallFont = Content.Load<SpriteFont>("SmallFont");
        }

        private Texture2D loadColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { color });
            return texture;
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (screen)
            {
                case Screen.build:
                    build();
                    break;

                case Screen.play:
                    playGame();
                    break;
                default:
                    break;
            }
            
            base.Update(gameTime);
        }

        private void build()
        {
            screen = player.inputControlBuild();


           // player.Locaion = new Vector2(screenWidth / 2, screenHeight / 2);

        }

        private void playGame()
        {
            if (astroroids.Count() < 10)
            {
                astroroids.createAstroroids(30,planets.Planets[0]);
            }
            stars.update();
            astroroids.update(pickups, player.getShip(), planets);
            player.Update(pickups, planets);
            pickups.Update();

            Vector2 cameraLoc = player.getShip().Locaion;
            cameraLoc.X -= screenWidth / 2 / camera.scale;
            cameraLoc.Y -= screenHeight / 2 / camera.scale;
            camera.setLocation(cameraLoc);

            screen = player.inputControlPlay(screenWidth * .5f, screenHeight * .5f);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // World
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.getTransformation());
            switch (screen)
            {
                case Screen.build:

                    break;

                case Screen.play:
                    stars.Draw(spriteBatch, fill);
                    planets.Draw(spriteBatch,sphere);
                    // station.Draw(spriteBatch,fill);

                    player.DrawPlay(spriteBatch, fill,smallFont);
                    
                    astroroids.draw(spriteBatch, fill);

                    pickups.Draw(spriteBatch, fill);

                    //spriteBatch.DrawString(smallFont, "" + camera.getMid(), camera.getMid() - new Vector2(0, +40), Color.Blue);
                    //spriteBatch.Draw(fill, new Rectangle((int)camera.getMid().X, (int)camera.getMid().Y, 10, 10), Color.Blue);
                    break;
                default:
                    break;
            }
            spriteBatch.End();

            // Hud
            spriteBatch.Begin();
            switch (screen)
            {
                case Screen.build:
                    player.DrawBuild(spriteBatch, fill, smallFont);
                    break;

                case Screen.play:
                    player.DrawHud(screenWidth - 100, 100, spriteBatch, fill, sphere,smallFont, astroroids, planets);
                    
                    break;

                default:
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
