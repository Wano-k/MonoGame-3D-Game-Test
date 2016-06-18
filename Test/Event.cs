using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Event
    {
        protected GraphicsDevice device;
        public Vector3 Position;
        public Vector2 Size;
        protected VertexPositionTexture[] vertices;
        protected VertexBuffer vb;
        protected IndexBuffer ib;
        protected int[] indexes;
        protected int Frame = 0, FrameInactive = 0, FrameTick = 0, FrameTickInactive = 0, FrameDuration = 200, FrameDurationInactive = 200;
        protected int Frame_inactive = 0;
        protected bool Act = true;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Event(GraphicsDevice device, Vector3 position, Vector2 size)
        {
            this.device = device;

            // Position and size
            this.Position = position;
            this.Size = size;

            // Init buffers
            this.vb = new VertexBuffer(this.device, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
            this.ib = new IndexBuffer(this.device, IndexElementSize.ThirtyTwoBits, 6, BufferUsage.WriteOnly);
            this.device.SetVertexBuffer(this.vb);
        }

        // -------------------------------------------------------------------
        // GetX
        // -------------------------------------------------------------------

        public int GetX()
        {
            return (int)((Position.X + 1) / WANOK.SQUARE_SIZE);
        }

        // -------------------------------------------------------------------
        // GetY
        // -------------------------------------------------------------------

        public int GetY()
        {
            return (int)((Position.Z + 1) / WANOK.SQUARE_SIZE);
        }

        // -------------------------------------------------------------------
        // CreateTex : coords = [x,y,width,height]
        // -------------------------------------------------------------------

        protected void CreateTex(int[] coords, Texture2D texture)
        {
            // Texture coords
            float left = ((float)coords[0]) / texture.Width;
            float top = ((float)coords[1]) / texture.Height;
            float bot = ((float)(coords[1] + coords[3])) / texture.Height;
            float right = ((float)(coords[0] + coords[2])) / texture.Width;

            // Vertex Position and Texture
            this.vertices = new VertexPositionTexture[]
           {
               new VertexPositionTexture(WANOK.VERTICESSPRITE[0], new Vector2(left,top)),
               new VertexPositionTexture(WANOK.VERTICESSPRITE[1], new Vector2(right,top)),
               new VertexPositionTexture(WANOK.VERTICESSPRITE[2], new Vector2(right,bot)),
               new VertexPositionTexture(WANOK.VERTICESSPRITE[3], new Vector2(left,bot))
           };

            // Vertex Indexes
            this.indexes = new int[]
            {
                0, 1, 2, 0, 2, 3
            };

            // Update buffers
            this.vb.SetData(this.vertices);
            this.ib.SetData(this.indexes);
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GameTime gameTime, Camera camera, BasicEffect effect)
        {
            // Bounce
            int bounce = (Frame == 0 || Frame == 2) ? 0 : 1;

            // Setting effect
            effect.Texture = Game1.heroTex;
            effect.World = Matrix.Identity * Matrix.CreateScale(Size.X, Size.Y, 1.0f) * Matrix.CreateTranslation(-Size.X / 2, 0, 0) * Matrix.CreateRotationY((float)((-camera.HorizontalAngle - 90) * Math.PI / 180.0)) * Matrix.CreateTranslation(Position.X, Position.Y + bounce, Position.Z);
            CreateTex(new int[] { 0, 0, (int)Size.X, (int)Size.Y }, Game1.heroTex);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, this.vertices, 0, this.vertices.Length, this.indexes, 0, 2);
            }
        }
    }
}
