using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test
{
    public class Game1 : Game
    {
        // Infos
        GraphicsDeviceManager graphics;
        Camera camera;
        Map map;
        Hero hero;
        BasicEffect effect;

        // Content
        public static Texture2D currentFloorTex;
        public static Texture2D heroTex;



        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Game1()
        {
            // Graphics
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 600;
            Window.Title = "RPM Monogame Test";
            
            // Content
            Content.RootDirectory = "Content";
        }

        // -------------------------------------------------------------------
        // Initialize
        // -------------------------------------------------------------------

        protected override void Initialize()
        {
            // Create game components
            camera = new Camera(this, Vector3.Zero, Vector3.Zero);

            // Important graphic settings
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.Clear(Color.Transparent);

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
            heroTex = Content.Load<Texture2D>("Pictures/Textures2D/Characters/lucas");

            // Drawable objects
            map = new Map(this.GraphicsDevice, "testmap");
            hero = new Hero(this.GraphicsDevice);
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
            // Keyboard
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape)) Exit();

            // Update camera
            camera.Update(gameTime,hero,kb);

            base.Update(gameTime);
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        protected override void Draw(GameTime gameTime)
        {
            // Background color
            GraphicsDevice.Clear(new Color(205, 222, 227));

            // Effect settings
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = camera.World;
            effect.TextureEnabled = true;

            // Drawing map + hero
            this.map.Draw(gameTime,effect);
            this.hero.Draw(gameTime, effect);

            base.Draw(gameTime);
        } 
    }
}
