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
        public static Dictionary<string, int> AutotileBorder = new Dictionary<string, int>{
            { "A1", 2 },
            { "B1", 3 },
            { "C1", 6 },
            { "D1", 7 },
            { "A2", 8 },
            { "B4", 9 },
            { "A4", 10 },
            { "B2", 11 },
            { "C5", 12 },
            { "D3", 13 },
            { "C3", 14 },
            { "D5", 15 },
            { "A5", 16 },
            { "B3", 17 },
            { "A3", 18 },
            { "B5", 19 },
            { "C2", 20 },
            { "D4", 21 },
            { "C4", 22 },
            { "D2", 23 },
        };
        public static string[] listA = new string[] { "A1", "A2", "A3", "A4", "A5" };
        public static string[] listB = new string[] { "B1", "B2", "B3", "B4", "B5" };
        public static string[] listC = new string[] { "C1", "C2", "C3", "C4", "C5" };
        public static string[] listD = new string[] { "D1", "D2", "D3", "D4", "D5" };


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
                0, 1, 2, 0, 2, 3
            };
            int offset = 0;

            if (Game1.TexAutotiles.ContainsKey(Id))
            {
                foreach (KeyValuePair<int[], Autotile> entry in Tiles)
                {
                    foreach (VertexPositionTexture vertex in CreateTex(entry.Key, entry.Value))
                    {
                        verticesList.Add(vertex);
                    }
                    for (int n = 0; n < 6; n++)
                    {
                        indexesList.Add(indexes[n] + offset);
                    }
                    offset += 4;
                }

                VerticesArray = verticesList.ToArray();
                IndexesArray = indexesList.ToArray();
                IB = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, IndexesArray.Length, BufferUsage.None);
                IB.SetData(IndexesArray);
                VB = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, VerticesArray.Length, BufferUsage.None);
                VB.SetData(VerticesArray);
            }
        }

        // -------------------------------------------------------------------
        // CreateTex
        // -------------------------------------------------------------------

        protected VertexPositionTexture[] CreateTex(int[] coords, Autotile autotile)
        {
            int x = coords[0], y = coords[1] * WANOK.SQUARE_SIZE + coords[2], z = coords[3];

            int xTile = autotile.TilesId % 125;
            int yTile = autotile.TilesId / 125;

            // Texture coords
            float left = xTile / 125.0f;
            float top = yTile / 5.0f;
            float bot = (yTile + 1) / 5.0f;
            float right = (xTile + 1) / 125.0f;

            // Vertex Position and Texture
            return new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(x, y, z), new Vector2(left, top)),
                new VertexPositionTexture(new Vector3(x+1, y, z), new Vector2(right, top)),
                new VertexPositionTexture(new Vector3(x+1, y, z+1), new Vector2(right, bot)),
                new VertexPositionTexture(new Vector3(x, y, z+1), new Vector2(left, bot))
            };
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
