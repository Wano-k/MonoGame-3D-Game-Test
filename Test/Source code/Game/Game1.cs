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

        // Components
        Camera Camera;
        Map Map;
        Hero Hero;

        // Content
        public static Texture2D TexTileset, TexNone, TexHero, TexHeroAct;
        public static Dictionary<int, Texture2D> TexAutotiles = new Dictionary<int, Texture2D>();
        public static Dictionary<int, Texture2D> TexReliefs = new Dictionary<int, Texture2D>();
        public static Dictionary<SystemGraphic, Texture2D> TexCharacters = new Dictionary<SystemGraphic, Texture2D>();


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Game1()
        {
            // Initialize
            WANOK.Game.LoadDatas();
            WANOK.Game.System.PathRTP = "RTP";

            // Graphics
            graphics = new GraphicsDeviceManager(this);
            SetWindowSettings(WANOK.Game.System.FullScreen);
            Window.Title = WANOK.Game.System.GameName[WANOK.Game.System.Langs[0]];
            
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
            fs = new FileStream(Path.Combine(WANOK.Game.System.PathRTP, "Content", "Pictures", "Textures2D", "Characters", "lucas.png"), FileMode.Open);
            TexHero = Texture2D.FromStream(GraphicsDevice, fs);
            fs.Close();
            fs = new FileStream(Path.Combine(WANOK.Game.System.PathRTP, "Content", "Pictures", "Textures2D","Characters", "lucas_act.png"), FileMode.Open);
            TexHeroAct = Texture2D.FromStream(GraphicsDevice, fs);
            fs.Close();
            TexNone = new Texture2D(GraphicsDevice, 1, 1);
            font = Content.Load<SpriteFont>("Fonts/corbel");

            /*
            string heroPath = WANOK.GetTilesetTexturePath(Map.MapInfos.Tileset);
            fs = new FileStream(heroPath, FileMode.Open);
            currentFloorTex = Texture2D.FromStream(GraphicsDevice, fs);
            fs = new FileStream(Path.Combine(Path.GetDirectoryName(heroPath), Path.GetFileNameWithoutExtension(heroPath) + "_act" + Path.GetExtension(heroPath)), FileMode.Open);
            */

            Map = new Map(GraphicsDevice, WANOK.Game.System.StartMapName);
            Hero = new Hero(GraphicsDevice, new Vector3(WANOK.Game.System.StartPosition[0]*WANOK.SQUARE_SIZE, WANOK.Game.System.StartPosition[1] * WANOK.SQUARE_SIZE + WANOK.Game.System.StartPosition[2], WANOK.Game.System.StartPosition[3] * WANOK.SQUARE_SIZE));
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
                graphics.PreferredBackBufferWidth = WANOK.Game.System.ScreenWidth;
                graphics.PreferredBackBufferHeight = WANOK.Game.System.ScreenHeight;
            }
            graphics.ApplyChanges();
        }

        // -------------------------------------------------------------------
        // LoadSystemGraphic
        // -------------------------------------------------------------------

        public static void LoadSystemGraphic(SystemGraphic graphic, GraphicsDevice device)
        {
            if (!TexCharacters.ContainsKey(graphic)) TexCharacters[graphic] = graphic.LoadTexture(device);
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

            // Update all
            Hero.Update(gameTime, Camera, Map, kb);
            Camera.Update(gameTime, Hero, kb, Map);
            Map.Update(Hero.GetPortion());

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
