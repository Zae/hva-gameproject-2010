using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace GO
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GO : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static PrimitiveBatch primitiveBatch;
        public static int width;
        public static int halfWidth;
        public static int height;
        public static int halfHeight;

        private State state;

        public static GO instance;

        public GO()
        {
            //Set the singleton instance for static reference
            instance = this;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);

            graphics.ToggleFullScreen();

            width = graphics.GraphicsDevice.DisplayMode.Width;
            halfWidth = width / 2;

            height = graphics.GraphicsDevice.DisplayMode.Height;
            halfHeight = height / 2;
           
            Content.RootDirectory = "Content";
        }

        public static GO get()
        {
            return instance;   
        }

        public void setState(State newState)
        {
            state.focusLost();
            state = newState;
            state.focusGained();
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


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

            primitiveBatch = new PrimitiveBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Fonts.font = Content.Load<SpriteFont>("TitleFont");

            Music.titleSong = Content.Load<Song>("TitleSong");
            Music.gameSong1 = Content.Load<Song>("GameSong1");
            Music.gameSong2 = Content.Load<Song>("GameSong2");
            Music.actionSong1 = Content.Load<Song>("ActionSong1");
            Music.actionSound1 = Content.Load<SoundEffect>("ActionSound1");

            state = new StateTitle();
            state.focusGained();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            //// Allows the game to exit
            //if (gamepad.getstate(playerindex.one).buttons.back == buttonstate.pressed)
            //    this.exit();

            // TODO: Add your update logic here
            KeyboardState keyState = Keyboard.GetState();

            

            if (keyState.IsKeyDown(Keys.F1))
            {
                this.Exit();
            }

            state.update(gameTime.ElapsedGameTime.Milliseconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
           
            // TODO: Add your drawing code here
            state.draw();

            base.Draw(gameTime);
        }
    }
}
