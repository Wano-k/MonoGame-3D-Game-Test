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
        public SystemGraphic HeroGraphic = new SystemGraphic("lucas.png", true, GraphicKind.Character, new object[] { 0, 0, 1, 1, 4, 0, 0 });
        public static Texture2D TexTileset, TexNone;
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
            //WANOK.Game.System.PathRTP = "RTP";

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
            TexNone = new Texture2D(GraphicsDevice, 1, 1);
            font = Content.Load<SpriteFont>("Fonts/corbel");


            Map = new Map(GraphicsDevice, WANOK.Game.System.StartMapName);
            LoadSystemGraphic(HeroGraphic, GraphicsDevice);
            Hero = new Hero(GraphicsDevice, WANOK.GetVector3Position(WANOK.Game.System.StartPosition), TexCharacters[HeroGraphic]);
            Hero.CreatePortion(GraphicsDevice, TexCharacters[HeroGraphic], false);
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
            if (!TexCharacters.ContainsKey(graphic)) TexCharacters[graphic] = GetCharacterTexture(device, graphic.LoadTexture(device), graphic.GetCharacterAct().LoadTexture(device));
        }

        // -------------------------------------------------------------------
        // GetCharacterTexture
        // -------------------------------------------------------------------

        public static Texture2D GetCharacterTexture(GraphicsDevice device, Texture2D graphic, Texture2D graphicAct)
        {
            Texture2D newTexture = new Texture2D(device, graphic.Width * 2, graphic.Height);
            Color[] graphicData = new Color[graphic.Width * graphic.Height];
            Color[] graphicActData = new Color[graphicAct.Width * graphicAct.Height];
            graphic.GetData(graphicData);
            graphicAct.GetData(graphicActData);
            WANOK.FillImage(device, newTexture, graphicData, graphic.Width, new Rectangle(0, 0, graphic.Width, graphic.Height), new Rectangle(0, 0, graphic.Width, graphic.Height));
            WANOK.FillImage(device, newTexture, graphicActData, graphic.Width, new Rectangle(graphic.Width, 0, graphic.Width, graphic.Height), new Rectangle(0, 0, graphic.Width, graphic.Height));
            graphic.Dispose();
            graphicAct.Dispose();

            return newTexture;
        }

        // -------------------------------------------------------------------
        // GetCharacterTexture
        // -------------------------------------------------------------------

        public static Texture2D GetAutotileTexture(GraphicsDevice device, Texture2D originalTexture)
        {
            Texture2D newTexture = new Texture2D(device, 125 * WANOK.SQUARE_SIZE, 5 * WANOK.SQUARE_SIZE);
            Color[] imageData = new Color[originalTexture.Width * originalTexture.Height];
            originalTexture.GetData(imageData);

            int count;
            for (int a = 0; a < Autotiles.listA.Length; a++)
            {
                count = 0;
                for (int b = 0; b < Autotiles.listB.Length; b++)
                {
                    for (int c = 0; c < Autotiles.listC.Length; c++)
                    {
                        for (int d = 0; d < Autotiles.listD.Length; d++)
                        {
                            WANOK.FillImage(device, newTexture, imageData, originalTexture.Width, new Rectangle(count * WANOK.SQUARE_SIZE, a * WANOK.SQUARE_SIZE, WANOK.SQUARE_SIZE / 2, WANOK.SQUARE_SIZE / 2), new Rectangle((Autotiles.AutotileBorder[Autotiles.listA[a]] % 4) * (WANOK.SQUARE_SIZE / 2), (Autotiles.AutotileBorder[Autotiles.listA[a]] / 4) * (WANOK.SQUARE_SIZE / 2), WANOK.SQUARE_SIZE / 2, WANOK.SQUARE_SIZE / 2));
                            WANOK.FillImage(device, newTexture, imageData, originalTexture.Width, new Rectangle(count * WANOK.SQUARE_SIZE + (WANOK.SQUARE_SIZE / 2), a * WANOK.SQUARE_SIZE, WANOK.SQUARE_SIZE / 2, WANOK.SQUARE_SIZE / 2), new Rectangle((Autotiles.AutotileBorder[Autotiles.listB[b]] % 4) * (WANOK.SQUARE_SIZE / 2), (Autotiles.AutotileBorder[Autotiles.listB[b]] / 4) * (WANOK.SQUARE_SIZE / 2), WANOK.SQUARE_SIZE / 2, WANOK.SQUARE_SIZE / 2));
                            WANOK.FillImage(device, newTexture, imageData, originalTexture.Width, new Rectangle(count * WANOK.SQUARE_SIZE, a * WANOK.SQUARE_SIZE + (WANOK.SQUARE_SIZE / 2), WANOK.SQUARE_SIZE / 2, WANOK.SQUARE_SIZE / 2), new Rectangle((Autotiles.AutotileBorder[Autotiles.listC[c]] % 4) * (WANOK.SQUARE_SIZE / 2), (Autotiles.AutotileBorder[Autotiles.listC[c]] / 4) * (WANOK.SQUARE_SIZE / 2), WANOK.SQUARE_SIZE / 2, WANOK.SQUARE_SIZE / 2));
                            WANOK.FillImage(device, newTexture, imageData, originalTexture.Width, new Rectangle(count * WANOK.SQUARE_SIZE + (WANOK.SQUARE_SIZE / 2), a * WANOK.SQUARE_SIZE + (WANOK.SQUARE_SIZE / 2), WANOK.SQUARE_SIZE / 2, WANOK.SQUARE_SIZE / 2), new Rectangle((Autotiles.AutotileBorder[Autotiles.listD[d]] % 4) * (WANOK.SQUARE_SIZE / 2), (Autotiles.AutotileBorder[Autotiles.listD[d]] / 4) * (WANOK.SQUARE_SIZE / 2), WANOK.SQUARE_SIZE / 2, WANOK.SQUARE_SIZE / 2));
                            count++;
                        }
                    }
                }
            }
            originalTexture.Dispose();

            return newTexture;
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
            Hero.Draw(GraphicsDevice, Camera, effect, Map.Orientation, HeroGraphic);

            // Interface
            /*
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "[" + Hero.GetCenterX() + "," + Hero.GetCenterZ() + "]", new Vector2(10, 10), Color.White);
            spriteBatch.End();
            */

            // Important settings
            LoadSettings();
            
            base.Draw(gameTime);
        } 
    }
}
