using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using RPG_Paper_Maker;
using System.Collections.Generic;

namespace Test
{
    public class Game1 : Game
    {
        // Infos
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AlphaTestEffect effect;
        SpriteFont font;
        Camera Camera;
        Map Map;
        Hero Hero;

        // Content
        public static Texture2D TexTileset, TexHero, TexHeroAct, TexNone;
        public static Dictionary<int, Texture2D> TexAutotiles = new Dictionary<int, Texture2D>();


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Game1()
        {
            // Initialize
            WANOK.SystemDatas = WANOK.LoadBinaryDatas<SystemDatas>(Path.Combine("Content", "Datas", "System.rpmd"));
            if (WANOK.SystemDatas == null) WANOK.PrintError("System.rpmd version is not compatible.");
            WANOK.SystemDatas.PathRTP = "RTP";

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
            effect = new AlphaTestEffect(GraphicsDevice);
            effect.AlphaFunction = CompareFunction.Greater;
            effect.ReferenceAlpha = 1;

            // Textures loading
            spriteBatch = new SpriteBatch(GraphicsDevice);
            FileStream fs;
            fs = new FileStream(Path.Combine(WANOK.SystemDatas.PathRTP, "Content", "Pictures", "Textures2D", "Characters", "lucas.png"), FileMode.Open);
            TexHero = Texture2D.FromStream(GraphicsDevice, fs);
            fs = new FileStream(Path.Combine(WANOK.SystemDatas.PathRTP, "Content", "Pictures", "Textures2D","Characters", "lucas_act.png"), FileMode.Open);
            TexHeroAct = Texture2D.FromStream(GraphicsDevice, fs);
            TexNone = new Texture2D(GraphicsDevice, 1, 1);
            font = Content.Load<SpriteFont>("Fonts/corbel");

            /*
            string heroPath = WANOK.GetTilesetTexturePath(Map.MapInfos.Tileset);
            fs = new FileStream(heroPath, FileMode.Open);
            currentFloorTex = Texture2D.FromStream(GraphicsDevice, fs);
            fs = new FileStream(Path.Combine(Path.GetDirectoryName(heroPath), Path.GetFileNameWithoutExtension(heroPath) + "_act" + Path.GetExtension(heroPath)), FileMode.Open);
            */

            // Search for map start
            Map = new Map(GraphicsDevice, WANOK.SystemDatas.StartMapName);
            /*
            fs = new FileStream(WANOK.GetTilesetTexturePath(Map.MapInfos.Tileset), FileMode.Open);
            CurrentFloorTex = Texture2D.FromStream(GraphicsDevice, fs);
            fs.Close();
            
            Map.LoadMap();*/
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
            Camera.Update(gameTime, Hero, kb, Map);
            base.Update(gameTime);
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        protected override void Draw(GameTime gameTime)
        {
            // Effect settings
            effect.View = Camera.View;
            effect.Projection = Camera.Projection;
            effect.World = Matrix.Identity;

            // Drawing map + hero
            Map.Draw(gameTime, effect, Camera);
            Hero.Draw(gameTime, Camera, effect, Map.Orientation);

            // Interface
            /*
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "[" + Hero.GetX() + "," + Hero.GetY() + "]", new Vector2(10, 10), Color.Black);
            spriteBatch.End();
            */

            // Important settings
            LoadSettings();
            
            base.Draw(gameTime);
        } 
    }
}
