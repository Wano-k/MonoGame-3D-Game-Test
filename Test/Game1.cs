using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using RPG_Paper_Maker;

namespace Test
{
    public class Game1 : Game
    {
        // Infos
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;
        SpriteFont font;
        Camera Camera;
        Map Map;
        Hero Hero;

        // Content
        public static Texture2D currentFloorTex;
        public static Texture2D heroTex;



        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Game1()
        {
            // Initialize
            WANOK.SystemDatas = WANOK.LoadBinaryDatas<SystemDatas>(Path.Combine("Content", "Datas", "System.rpmd"));
            if (WANOK.SystemDatas == null) WANOK.PrintError("System.rpmd version is not compatible.");

            // Graphics
            graphics = new GraphicsDeviceManager(this);
            SetWindowSettings(WANOK.SystemDatas.FullScreen);
            Window.Title = WANOK.SystemDatas.GameName[WANOK.SystemDatas.Langs[0]];
            
            // Content
            Content.RootDirectory = "Content";
        }

        // -------------------------------------------------------------------
        // Initialize
        // -------------------------------------------------------------------

        protected override void Initialize()
        {
            // Create game components
            Camera = new Camera(this);

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
            Stream stream;
            stream = TitleContainer.OpenStream(Path.Combine("Content", "Pictures", "Textures2D", "Tilesets", "plains.png"));
            currentFloorTex = Texture2D.FromStream(GraphicsDevice, stream);
            stream = TitleContainer.OpenStream(Path.Combine("Content", "Pictures", "Textures2D", "Characters", "lucas.png"));
            heroTex = Texture2D.FromStream(GraphicsDevice, stream);
            font = Content.Load<SpriteFont>("Fonts/corbel");
            stream.Close();

            // Search for map start
            Map = new Map(GraphicsDevice, WANOK.SystemDatas.StartMapName);
            Hero = new Hero(GraphicsDevice, new Vector3(WANOK.SystemDatas.StartPosition[0]*WANOK.SQUARE_SIZE, WANOK.SystemDatas.StartPosition[1] * WANOK.SQUARE_SIZE + WANOK.SystemDatas.StartPosition[2], WANOK.SystemDatas.StartPosition[3] * WANOK.SQUARE_SIZE));
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
        // SetWindowSettings
        // -------------------------------------------------------------------

        public void SetWindowSettings(bool isFullScreen)
        {
            if (isFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.ToggleFullScreen();
            }
            else
            {
                graphics.PreferredBackBufferWidth = WANOK.SystemDatas.ScreenWidth;
                graphics.PreferredBackBufferHeight = WANOK.SystemDatas.ScreenHeight;
            }
            graphics.ApplyChanges();
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        protected override void Update(GameTime gameTime)
        {
            // Keyboard
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape)) Exit();
            if (kb.IsKeyDown(Keys.LeftAlt) && kb.IsKeyDown(Keys.Enter))
            {
                SetWindowSettings(graphics.IsFullScreen);
            }

            // Update camera
            Hero.Update(gameTime, Camera, Map, kb);
            Camera.Update(gameTime, Hero, kb);
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
            effect.View = Camera.View;
            effect.Projection = Camera.Projection;
            effect.World = Matrix.Identity;
            effect.TextureEnabled = true;

            // Drawing map + hero
            Map.Draw(gameTime, effect);
            Hero.Draw(gameTime, Camera, effect);

            //spriteBatch.DrawString()
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "[" + Hero.GetX() + "," + Hero.GetY() + "]", new Vector2(10, 10), Color.Black);
            spriteBatch.End();

            // Important settings
            LoadSettings();
            
            base.Draw(gameTime);
        } 
    }
}
