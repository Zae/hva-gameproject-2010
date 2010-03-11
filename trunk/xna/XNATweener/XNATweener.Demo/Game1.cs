using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using XNATweener;
using System.Reflection;

namespace Tweening
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GameComponent activeDemo;

        SpriteBatch spriteBatch;
        SpriteFont font;

        KeyboardState oldKeyboardState;

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
            base.Initialize();
            activeDemo = new BasicDemo(this);
            activeDemo.Initialize();
            Components.Add(activeDemo);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            
            // Allows the demo to exit
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                || keyboardState.IsKeyDown(Keys.Escape))                
                this.Exit();

            if (oldKeyboardState.IsKeyDown(Keys.F1) && keyboardState.IsKeyUp(Keys.F1))
            {
                SwitchDemo(new BasicDemo(this));
            }
            if (oldKeyboardState.IsKeyDown(Keys.F2) && keyboardState.IsKeyUp(Keys.F2))
            {
                SwitchDemo(new RotationDemo(this));
            }

            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        private void SwitchDemo(BasicDemo newDemo)
        {
            if (activeDemo != null)
            {
                Components.Remove(activeDemo);
            }
            activeDemo = newDemo;
            activeDemo.Initialize();
            Components.Add(newDemo);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            Vector2 hudPosition = new Vector2(10);
            Vector2 lineSpacing = new Vector2(0, font.LineSpacing);
            spriteBatch.DrawString(font, activeDemo.GetType().Name, hudPosition, Color.Red, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
            hudPosition += lineSpacing * 2;

            foreach (string line in (activeDemo as IUsageText).GetUsageText())
            {
                if (String.IsNullOrEmpty(line))
                {
                    continue;
                }
                string[] lineParts = line.Split(':');
                lineParts[0] += ":";
                Vector2 size = font.MeasureString(lineParts[0]);
                size.Y = 0;
                spriteBatch.DrawString(font, lineParts[0], hudPosition, Color.White);
                spriteBatch.DrawString(font, lineParts[1], hudPosition + size, Color.Black);
                hudPosition += lineSpacing;
            }

            spriteBatch.DrawString(font, "Press F1 or F2 to choose demo", new Vector2(10, GraphicsDevice.Viewport.Height - font.LineSpacing * 3), Color.Black);
            spriteBatch.End();
        }
    }
}
