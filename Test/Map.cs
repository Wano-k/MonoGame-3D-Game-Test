﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RPG_Paper_Maker;

namespace Test
{
    class Map
    {
        GraphicsDevice Device;
        VertexPositionColor[] GridVerticesArray;
        VertexBuffer VBGrid;
        public MapInfos MapInfos { get; set; }
        public Dictionary<int[], GameMapPortion> Portions;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Map(GraphicsDevice device, string mapName)
        {
            Device = device;
            MapInfos = WANOK.LoadBinaryDatas<MapInfos>(Path.Combine(WANOK.MapsDirectoryPath, mapName, "infos.map"));
        }

        // -------------------------------------------------------------------
        // LoadMap
        // -------------------------------------------------------------------

        public void LoadMap()
        {
            Portions = new Dictionary<int[], GameMapPortion>(new IntArrayComparer());

            for (int i = -WANOK.PORTION_RADIUS; i <= WANOK.PORTION_RADIUS; i++)
            {
                for (int j = -WANOK.PORTION_RADIUS; j <= WANOK.PORTION_RADIUS; j++)
                {
                    LoadPortion(i, j, i, j);
                }
            }
        }

        // -------------------------------------------------------------------
        // LoadPortion
        // -------------------------------------------------------------------

        public void LoadPortion(int real_i, int real_j, int i, int j)
        {
            
            string path = Path.Combine(WANOK.MapsDirectoryPath, MapInfos.RealMapName, real_i + "-" + real_j + ".pmap");

            if (File.Exists(path))
            {
                GameMapPortion gamePortion = WANOK.LoadBinaryDatas<GameMapPortion>(path);
                Portions[new int[] { i, j }] = gamePortion;
                gamePortion.CreatePortionFloor(Device, Game1.currentFloorTex);
            }
            else
            {
                Portions[new int[] { i, j }] = null;
            }
        }

        // -------------------------------------------------------------------
        // CreateGrid
        // -------------------------------------------------------------------

        public void CreateGrid(int width, int height)
        {
            List<VertexPositionColor> gridVerticesList = new List<VertexPositionColor>();
            // Columns
            for (int i = 0; i <= width; i++)
            {
                foreach (VertexPositionColor vertex in CreateGridLine(i, 0, i, height))
                {
                    gridVerticesList.Add(vertex);
                }
            }
            // Rows
            for (int i = 0; i <= height; i++)
            {
                foreach (VertexPositionColor vertex in CreateGridLine(0, i, width, i))
                {
                    gridVerticesList.Add(vertex);
                }
            }
            GridVerticesArray = gridVerticesList.ToArray();
            VBGrid = new VertexBuffer(Device, typeof(VertexPositionColor), GridVerticesArray.Length, BufferUsage.WriteOnly);
            VBGrid.SetData(GridVerticesArray);
        }

        // -------------------------------------------------------------------
        // GenFloor
        // -------------------------------------------------------------------

        public void GenFloor(int[] portion)
        {
            if (Portions[portion] != null)
            {
                Portions[portion].GenFloor(Device, Game1.currentFloorTex);
            }
        }

        // -------------------------------------------------------------------
        // DisposeVertexBuffer
        // -------------------------------------------------------------------

        public void DisposeVertexBuffer()
        {
            Device.SetVertexBuffer(null);
            VBGrid.Dispose();
        }

        // -------------------------------------------------------------------
        // CreateGridLine
        // -------------------------------------------------------------------

        private VertexPositionColor[] CreateGridLine(int x1, int z1, int x2, int z2)
        {
            // Vertex Position and Texture
            return new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(x1, 0, z1), Color.White),
                new VertexPositionColor(new Vector3(x2, 0, z2), Color.White)
            };
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GameTime gameTime, BasicEffect effect)
        {
            Device.Clear(WANOK.GetColor(MapInfos.SkyColor));

            // Drawing Floors
            effect.World = Matrix.Identity * Matrix.CreateScale(WANOK.SQUARE_SIZE, 1.0f, WANOK.SQUARE_SIZE);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            foreach (GameMapPortion gameMap in Portions.Values)
            {
                if (gameMap != null) gameMap.Draw(Device, effect, Game1.currentFloorTex);
            }
        }

        // -------------------------------------------------------------------
        // DisposeBuffers
        // -------------------------------------------------------------------

        public void DisposeBuffers(int[] portion)
        {
            Portions[portion].DisposeBuffers(Device);
        }
    }
}
