using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Hero
    {
        GraphicsDevice device;
        public Vector3 Position;
        public Vector2 Size;
        VertexPositionTexture[] verticesHero;
        VertexBuffer vb;
        IndexBuffer ib;
        int[] indexes;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Hero(GraphicsDevice device)
        {
            this.device = device;

            // Position and size
            this.Position = new Vector3(0,0,0);
            this.Size = new Vector2(32, 32);

            // Init buffers
            this.vb = new VertexBuffer(this.device, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
            this.ib = new IndexBuffer(this.device, IndexElementSize.ThirtyTwoBits, 6, BufferUsage.WriteOnly);
            this.device.SetVertexBuffer(this.vb);
        }

        // -------------------------------------------------------------------
        // CreateHeroWithTex : coords = [x,y,width,height]
        // -------------------------------------------------------------------

        private void CreateHeroWithTex(int[] coords, Texture2D texture)
        {
            // Texture coords
            float left = ((float)coords[0]) / texture.Width;
            float top = ((float)coords[1]) / texture.Height;
            float bot = ((float)(coords[1] + coords[3])) / texture.Height;
            float right = ((float)(coords[0] + coords[2])) / texture.Width;

            // Vertex Position and Texture
            this.verticesHero = new VertexPositionTexture[]
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
            this.vb.SetData(this.verticesHero);
            this.ib.SetData(this.indexes);
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GameTime gameTime, BasicEffect effect)
        {
            effect.Texture = Game1.heroTex;
            effect.World = Matrix.Identity * Matrix.CreateTranslation(0, 0, 0) * Matrix.CreateScale(Size.X, Size.Y, 1.0f);

            CreateHeroWithTex(new int[] { 0, 0, (int)Size.X, (int)Size.Y }, Game1.heroTex);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, this.verticesHero, 0, this.verticesHero.Length, this.indexes, 0, 2);
            }
        }
    }
}
