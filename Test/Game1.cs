using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test
{
    public class Game1 : Game
    {
        // Infos
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;
        SpriteFont font;
        Camera camera;
        Map map;
        Hero hero;

        // Content
        public static Texture2D currentFloorTex;
        public static Texture2D heroTex;



        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Game1()
        {
            // Graphics
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
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
            camera = new Camera(this);

            LoadSettings();

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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            System.IO.Stream stream;

            stream = TitleContainer.OpenStream(@"Content/Pictures/Textures2D/Floors/rtp.png");
            currentFloorTex = Texture2D.FromStream(GraphicsDevice, stream);
            //currentFloorTex = Content.Load<Texture2D>("Pictures/Textures2D/Floors/rtp");
            stream = TitleContainer.OpenStream(@"Content/Pictures/Textures2D/Characters/lucas.png");
            heroTex = Texture2D.FromStream(GraphicsDevice, stream);
            //heroTex = Content.Load<Texture2D>("Pictures/Textures2D/Characters/lucas");
            font = Content.Load<SpriteFont>("Fonts/corbel");
            stream.Close();

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
        // LoadSettings
        // -------------------------------------------------------------------

        protected void LoadSettings()
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
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
            hero.Update(gameTime, camera, map, kb);
            camera.Update(gameTime, hero, kb);

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
            effect.World = Matrix.Identity;
            effect.TextureEnabled = true;

            // Drawing map + hero
            map.Draw(gameTime, effect);
            hero.Draw(gameTime, camera, effect);

            //spriteBatch.DrawString()
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "[" + hero.GetX() + "," + hero.GetY() + "]", new Vector2(10, 10), Color.Black);
            spriteBatch.End();

            // Important settings
            LoadSettings();

            base.Draw(gameTime);
        } 
    }
}
