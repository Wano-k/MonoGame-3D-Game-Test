using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class Autotiles
    {
        public int Id;
        public Dictionary<int[], Autotile> Tiles = new Dictionary<int[], Autotile>(new IntArrayComparer());

        [NonSerialized()]
        VertexBuffer VB;
        [NonSerialized()]
        VertexPositionTexture[] VerticesArray;
        [NonSerialized()]
        IndexBuffer IB;
        [NonSerialized()]
        int[] IndexesArray;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Autotiles(int id)
        {
            Id = id;
        }

        // -------------------------------------------------------------------
        // GenAutotiles
        // -------------------------------------------------------------------

        public void GenAutotiles(GraphicsDevice device)
        {
            DisposeBuffers(device);
            CreatePortion(device);
        }

        // -------------------------------------------------------------------
        // CreatePortion
        // -------------------------------------------------------------------

        public void CreatePortion(GraphicsDevice device)
        {
            List<VertexPositionTexture> verticesList = new List<VertexPositionTexture>();
            List<int> indexesList = new List<int>();
            int[] indexes = new int[]
            {
                0, 1, 2, 0, 2, 3,
                4, 5, 6, 4, 6, 7,
                8, 9, 10, 8, 10, 11,
                12, 13, 14, 12, 14, 15,
            };
            int offset = 0;

            foreach (KeyValuePair<int[], Autotile> entry in Tiles)
            {
                foreach (VertexPositionTexture vertex in CreateTex(Game1.TexAutotiles[Id], entry.Key, entry.Value))
                {
                    verticesList.Add(vertex);
                }
                for (int n = 0; n < 24; n++)
                {
                    indexesList.Add(indexes[n] + offset);
                }
                offset += 16;
            }

            VerticesArray = verticesList.ToArray();
            IndexesArray = indexesList.ToArray();
            IB = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, IndexesArray.Length, BufferUsage.None);
            IB.SetData(IndexesArray);
            VB = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, VerticesArray.Length, BufferUsage.None);
            VB.SetData(VerticesArray);
        }

        // -------------------------------------------------------------------
        // CreateTex
        // -------------------------------------------------------------------

        protected VertexPositionTexture[] CreateTex(Texture2D texture, int[] coords, Autotile autotile)
        {
            VertexPositionTexture[] res = new VertexPositionTexture[16];

            int x = coords[0], y = coords[1] * WANOK.SQUARE_SIZE + coords[2], z = coords[3];
            float[] left = new float[4], top = new float[4], bot = new float[4], right = new float[4];
            float[] leftPos = new float[4], topPos = new float[4], botPos = new float[4], rightPos = new float[4];

            for (int i = 0; i < 4; i++)
            {
                int xTile = autotile.Tiles[i] % 4;
                int yTile = autotile.Tiles[i] / 4;
                float pos = i < 2 ? 0.0f : 0.5f;

                // Texture coords
                leftPos[i] = (float)(i % 2) / 2;
                topPos[i] = pos;
                botPos[i] = pos + 0.5f;
                rightPos[i] = leftPos[i] + 0.5f;
                left[i] = (xTile + leftPos[i]) * (WANOK.SQUARE_SIZE / 2) / texture.Width;
                top[i] = (yTile + topPos[i]) * (WANOK.SQUARE_SIZE / 2) / texture.Height;
                bot[i] = (yTile + botPos[i]) * (WANOK.SQUARE_SIZE / 2) / texture.Height;
                right[i] = (xTile + rightPos[i]) * (WANOK.SQUARE_SIZE / 2) / texture.Width;

                // Adjust in order to limit risk of textures flood
                float width = left[i] + right[i];
                float height = top[i] + bot[i];
                left[i] += width / WANOK.COEF_BORDER_TEX;
                right[i] -= width / WANOK.COEF_BORDER_TEX;
                top[i] += height / WANOK.COEF_BORDER_TEX;
                bot[i] -= height / WANOK.COEF_BORDER_TEX;

                // Vertex Position and Texture
                res[i * 4] = new VertexPositionTexture(new Vector3(x + leftPos[i], y, z + topPos[i]), new Vector2(left[i], top[i]));
                res[i * 4 + 1] = new VertexPositionTexture(new Vector3(x + rightPos[i], y, z + topPos[i]), new Vector2(right[i], top[i]));
                res[i * 4 + 2] = new VertexPositionTexture(new Vector3(x + rightPos[i], y, z + botPos[i]), new Vector2(right[i], bot[i]));
                res[i * 4 + 3] = new VertexPositionTexture(new Vector3(x + leftPos[i], y, z + botPos[i]), new Vector2(left[i], bot[i]));
            }

            return res;
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GraphicsDevice device, AlphaTestEffect effect)
        {
            if (VB != null)
            {
                if (!Game1.TexAutotiles.ContainsKey(Id)) effect.Texture = Game1.TexNone;
                else effect.Texture = Game1.TexAutotiles[Id];

                device.SetVertexBuffer(VB);
                device.Indices = IB;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, VerticesArray, 0, VerticesArray.Length, IndexesArray, 0, VerticesArray.Length / 2);
                }
            }
        }

        // -------------------------------------------------------------------
        // DisposeBuffers
        // -------------------------------------------------------------------

        public void DisposeBuffers(GraphicsDevice device, bool nullable = true)
        {
            if (VB != null)
            {
                device.SetVertexBuffer(null);
                device.Indices = null;
                VB.Dispose();
                IB.Dispose();
                if (nullable)
                {
                    VB = null;
                    IB = null;
                }
            }
        }
    }
}
