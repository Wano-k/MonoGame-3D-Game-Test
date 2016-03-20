using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Map
    {
        GraphicsDevice device;
        VertexPositionTexture[] verticesFloors;
        VertexBuffer vb;
        IndexBuffer ib;
        int[] indexes;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Map(GraphicsDevice device, string mapName)
        {
            this.device = device;
            int[] coords = {0,0,16,16};
            CreateFloorWithTex(coords);

            this.vb = new VertexBuffer(this.device, typeof(VertexPositionTexture), this.verticesFloors.Length, BufferUsage.WriteOnly);
            vb.SetData(this.verticesFloors);
            this.ib = new IndexBuffer(this.device, IndexElementSize.ThirtyTwoBits, this.indexes.Length, BufferUsage.WriteOnly);
            this.ib.SetData(this.indexes);

            this.device.SetVertexBuffer(this.vb);
        }

        // -------------------------------------------------------------------
        // CreateFloorWithTex : coords = [x,y,width,height]
        // -------------------------------------------------------------------
        private void CreateFloorWithTex(int[] coords)
        {
            // Texure coords
            float left = ((float)coords[0]) / Game1.currentFloorTex.Width;
            float top = ((float)coords[1]) / Game1.currentFloorTex.Height;
            float bot = ((float)(coords[1]+coords[3])) / Game1.currentFloorTex.Height;
            float right = ((float)(coords[0] + coords[2])) / Game1.currentFloorTex.Width;

            // Vertex Position and Texture
            this.verticesFloors = new VertexPositionTexture[]
           {
               new VertexPositionTexture(WANOK.VERTICESFLOOR[0], new Vector2(left,top)),
               new VertexPositionTexture(WANOK.VERTICESFLOOR[1], new Vector2(right,top)),
               new VertexPositionTexture(WANOK.VERTICESFLOOR[2], new Vector2(right,bot)),
               new VertexPositionTexture(WANOK.VERTICESFLOOR[3], new Vector2(left,bot))
           };

            // Vertex Indexes
            this.indexes = new int[]
            {
                0, 1, 2, 0, 2, 3
            };
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GameTime gameTime, BasicEffect effect)
        {
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = Game1.currentFloorTex;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, this.verticesFloors, 0, this.verticesFloors.Length, this.indexes, 0, 2);
            }         
        }
    }
}
