#region File Description
//-----------------------------------------------------------------------------
// Game1.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

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

namespace TexturedQuad
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        Quad quad;
        VertexDeclaration quadVertexDecl;
        Matrix View, Projection;
        protected override void Initialize()
        {
            quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, 1, 1);
            View = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, 
                Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 4.0f / 3.0f, 1, 500);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        Texture2D texture;
        BasicEffect quadEffect;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("Glass");
            quadEffect = new BasicEffect(graphics.GraphicsDevice, null);
            quadEffect.EnableDefaultLighting();

            quadEffect.World = Matrix.Identity;
            quadEffect.View = View;
            quadEffect.Projection = Projection;
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;
            quadVertexDecl = new VertexDeclaration(graphics.GraphicsDevice,
               VertexPositionNormalTexture.VertexElements);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == 
                ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.VertexDeclaration = quadVertexDecl;
            quadEffect.Begin();
            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList, 
                    quad.Vertices, 0, 4, 
                    quad.Indexes, 0, 2);

                pass.End();
            }
            quadEffect.End();
            
            base.Draw(gameTime);
        }
    }
}
