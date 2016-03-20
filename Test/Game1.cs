﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test
{
    public class Game1 : Game
    {
        // Attributes
        GraphicsDeviceManager graphics;
        Camera camera;
        Map map;
        BasicEffect effect;
        public static Texture2D currentFloorTex;



        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // -------------------------------------------------------------------
        // Initialize
        // -------------------------------------------------------------------

        protected override void Initialize()
        {
            // Create game components
            camera = new Camera(this, new Vector3(0.0f, 10.0f, 5.0f), Vector3.Zero);

            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
           

            base.Initialize();
        }

        // -------------------------------------------------------------------
        // LoadContent
        // -------------------------------------------------------------------

        protected override void LoadContent()
        {
            // Effect
            effect = new BasicEffect(GraphicsDevice);

            // Textures loading
            currentFloorTex = Content.Load<Texture2D>("Pictures/Textures2D/Floors/rtp");

            // Map
            map = new Map(this.GraphicsDevice, "testmap");
        }

        // -------------------------------------------------------------------
        // UnloadContent
        // -------------------------------------------------------------------

        protected override void UnloadContent()
        {
            
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = camera.World;

            this.map.Draw(gameTime,effect);

            base.Draw(gameTime);
        } 
    }
}
