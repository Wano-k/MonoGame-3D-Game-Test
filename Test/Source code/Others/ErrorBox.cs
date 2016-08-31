using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using RPG_Paper_Maker;
using System.Text;

namespace Test
{
    public class ErrorBox : Game
    {
        // Infos
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;
        SpriteFont font;
        public string Message;



        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public ErrorBox(string str)
        {
            // Graphics
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 250;
            graphics.ApplyChanges();
            Window.Title = "Error";
            Message = str;

            // Content
            Content.RootDirectory = "Content";
        }

        // -------------------------------------------------------------------
        // Initialize
        // -------------------------------------------------------------------

        protected override void Initialize()
        {
            base.Initialize();
        }

        // -------------------------------------------------------------------
        // LoadContent
        // -------------------------------------------------------------------

        protected override void LoadContent()
        {
            effect = new BasicEffect(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/corbel");

            Message = WrapText(Message);
        }

        // -------------------------------------------------------------------
        // UnloadContent
        // -------------------------------------------------------------------

        protected override void UnloadContent()
        {

        }

        private string WrapText(string text)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float linewidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                try
                {
                    Vector2 size = font.MeasureString(word);
                    if (linewidth + size.X < graphics.PreferredBackBufferWidth - 20)
                    {
                        sb.Append(word + " ");
                        linewidth += size.X + spaceWidth;
                    }
                    else if (size.X > graphics.PreferredBackBufferWidth - 20)
                    {
                        sb.Append("\n" + word + " ");
                        linewidth = size.X + spaceWidth;
                    }
                    else
                    {
                        sb.Append("\n" + word + " ");
                        linewidth = size.X + spaceWidth;
                    }
                    if (font.MeasureString(sb).Y >= graphics.PreferredBackBufferHeight - 20) break;
                }
                catch { }
            }

            return sb.ToString();
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        protected override void Draw(GameTime gameTime)
        {
            // Background color
            GraphicsDevice.Clear(new Color(240, 240, 240));

            //spriteBatch.DrawString()
            spriteBatch.Begin();
            spriteBatch.DrawString(font, Message, new Vector2(10, 10), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
