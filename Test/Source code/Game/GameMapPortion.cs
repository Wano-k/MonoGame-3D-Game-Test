﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace RPG_Paper_Maker
{
    [Serializable]
    class GameMapPortion
    {
        public Dictionary<int[], int[]> Floors;
        public Dictionary<int, Autotiles> Autotiles;
        public Dictionary<int[], Sprites> Sprites;
        public Dictionary<int, Mountains> Mountains;

        // Floors
        [NonSerialized()]
        VertexBuffer VBFloor;
        [NonSerialized()]
        VertexPositionTexture[] VerticesFloorArray;
        [NonSerialized()]
        IndexBuffer IBFloor;
        [NonSerialized()]
        int[] IndexesFloorArray;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public GameMapPortion()
        {
            Floors = new Dictionary<int[], int[]>(new IntArrayComparer());
            Autotiles = new Dictionary<int, Autotiles>();
            Sprites = new Dictionary<int[], Sprites>(new IntArrayComparer());
            Mountains = new Dictionary<int, Mountains>();
        }
        
        // -------------------------------------------------------------------
        // Generate Buffers
        // -------------------------------------------------------------------

        #region Floors

        public void GenFloor(GraphicsDevice device, Texture2D texture)
        {
            DisposeBuffersFloor(device);
            if (Floors.Count > 0) CreatePortionFloor(device, texture);
        }

        public void CreatePortionFloor(GraphicsDevice device, Texture2D texture)
        {
            // Building vertex buffer indexed
            List<VertexPositionTexture> verticesList = new List<VertexPositionTexture>();
            List<int> indexesList = new List<int>();
            int[] indexes = new int[]
            {
                0, 1, 2, 0, 2, 3
            };
            int offset = 0;


            foreach (KeyValuePair<int[], int[]> entry in Floors)
            {
                if (entry.Value[2] * WANOK.SQUARE_SIZE <= texture.Width && entry.Value[3] * WANOK.SQUARE_SIZE <= texture.Height)
                {
                    foreach (VertexPositionTexture vertex in CreateFloorWithTex(texture, entry.Key[0], (entry.Key[1] * WANOK.SQUARE_SIZE) + entry.Key[2], entry.Key[3], entry.Value))
                    {
                        verticesList.Add(vertex);
                    }
                    for (int n = 0; n < 6; n++)
                    {
                        indexesList.Add(indexes[n] + offset);
                    }
                    offset += 4;
                }
            }

            if (verticesList.Count > 0)
            {
                VerticesFloorArray = verticesList.ToArray();
                IndexesFloorArray = indexesList.ToArray();
                IBFloor = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, IndexesFloorArray.Length, BufferUsage.None);
                IBFloor.SetData(IndexesFloorArray);
                VBFloor = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, VerticesFloorArray.Length, BufferUsage.None);
                VBFloor.SetData(VerticesFloorArray);
            }
        }

        protected VertexPositionTexture[] CreateFloorWithTex(Texture2D texture, int x, int y, int z, int[] coords)
        {
            // Texture coords
            float left = ((float)coords[0] * WANOK.SQUARE_SIZE) / texture.Width;
            float top = ((float)coords[1] * WANOK.SQUARE_SIZE) / texture.Height;
            float bot = ((float)(coords[1] + coords[3]) * WANOK.SQUARE_SIZE) / texture.Height;
            float right = ((float)(coords[0] + coords[2]) * WANOK.SQUARE_SIZE) / texture.Width;

            // Adjust in order to limit risk of textures flood
            float width = left + right;
            float height = top + bot;
            left += width / WANOK.COEF_BORDER_TEX;
            right -= width / WANOK.COEF_BORDER_TEX;
            top += height / WANOK.COEF_BORDER_TEX;
            bot -= height / WANOK.COEF_BORDER_TEX;

            // Vertex Position and Texture
            return new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(x, y, z), new Vector2(left, top)),
                new VertexPositionTexture(new Vector3(x+1, y, z), new Vector2(right, top)),
                new VertexPositionTexture(new Vector3(x+1, y, z+1), new Vector2(right, bot)),
                new VertexPositionTexture(new Vector3(x, y, z+1), new Vector2(left, bot))
            };
        }

        #endregion

        #region Autotiles

        public void GenAutotiles(GraphicsDevice device)
        {
            foreach (Autotiles autotiles in Autotiles.Values)
            {
                autotiles.GenAutotiles(device);
            }
        }

        #endregion

        #region Sprites

        public void GenSprites(GraphicsDevice device)
        {
            foreach (KeyValuePair<int[], Sprites> entry in Sprites)
            {
                entry.Value.GenSprites(device, Game1.TexTileset, entry.Key);
            }
        }

        #endregion

        #region Mountains

        public void GenMountains(GraphicsDevice device)
        {
            foreach (Mountains mountains in Mountains.Values)
            {
                mountains.GenMountains(device);
            }
        }

        #endregion

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GraphicsDevice device, AlphaTestEffect effect, Texture2D texture, Camera camera)
        {
            // Drawing mountains
            effect.World = Matrix.Identity * Matrix.CreateScale(WANOK.SQUARE_SIZE, 1.0f, WANOK.SQUARE_SIZE);
            foreach (Mountains mountains in Mountains.Values)
            {
                mountains.Draw(device, effect);
            }

            // Drawing Floors
            effect.World = Matrix.Identity * Matrix.CreateScale(WANOK.SQUARE_SIZE, 1.0f, WANOK.SQUARE_SIZE);
            if (VBFloor != null)
            {
                effect.Texture = texture;

                device.SetVertexBuffer(VBFloor);
                device.Indices = IBFloor;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, VerticesFloorArray, 0, VerticesFloorArray.Length, IndexesFloorArray, 0, VerticesFloorArray.Length / 2);
                }
            }

            // Drawing Autotiles
            foreach (KeyValuePair<int, Autotiles> entry in Autotiles)
            {
                entry.Value.Draw(device, effect);
            }

            foreach (KeyValuePair<int[], Sprites> entry in Sprites)
            {
                entry.Value.Draw(device, effect, camera, entry.Key[2] * WANOK.SQUARE_SIZE, entry.Key[3] * WANOK.SQUARE_SIZE);
            }
        }

        // -------------------------------------------------------------------
        // DisposeBuffers
        // -------------------------------------------------------------------

        public void DisposeBuffers(GraphicsDevice device, bool nullable = true)
        {
            DisposeBuffersFloor(device, nullable);
            foreach (Autotiles autotiles in Autotiles.Values)
            {
                autotiles.DisposeBuffers(device, nullable);
            }
            foreach (Sprites sprites in Sprites.Values)
            {
                sprites.DisposeBuffers(device, nullable);
            }
            foreach (Mountains mountains in Mountains.Values)
            {
                mountains.DisposeBuffers(device, nullable);
            }
        }

        // -------------------------------------------------------------------
        // DisposeBuffersFloor
        // -------------------------------------------------------------------

        public void DisposeBuffersFloor(GraphicsDevice device, bool nullable = true)
        {
            if (VBFloor != null)
            {
                device.SetVertexBuffer(null);
                device.Indices = null;
                VBFloor.Dispose();
                IBFloor.Dispose();
                if (nullable)
                {
                    VBFloor = null;
                    IBFloor = null;
                }
            }
        }
    }
}
