using Microsoft.Xna.Framework;
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
        public MapInfos MapInfos { get; set; }
        public Dictionary<int[], GameMapPortion> Portions;
        public Orientation Orientation = Orientation.North; // Camera orientation
        public int[] CurrentPortion = new int[] { 0, 0 };


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Map(GraphicsDevice device, string mapName)
        {
            string pathDir = Path.Combine(WANOK.MapsDirectoryPath, mapName);
            if (!Directory.Exists(pathDir)) WANOK.PrintError("Error: could not find the " + mapName + " map directory.");
            if (WANOK.SystemDatas.StartMapName == "") WANOK.PrintError("Error: could not find the start position.");

            Device = device;
            MapInfos = WANOK.LoadBinaryDatas<MapInfos>(Path.Combine(pathDir, "infos.map"));


            // Set textures
            if (Game1.TexTileset != null) Game1.TexTileset.Dispose();
            foreach (int i in Game1.TexAutotiles.Keys)
            {
                Game1.TexAutotiles[i].Dispose();
            }
            Game1.TexAutotiles.Clear();
            Tileset tileset = WANOK.SystemDatas.GetTilesetById(MapInfos.Tileset);
            Game1.TexTileset = tileset.Graphic.LoadTexture(device);
            for (int i = 0; i < tileset.Autotiles.Count; i++)
            {
                Game1.TexAutotiles[tileset.Autotiles[i]] = WANOK.SystemDatas.GetAutotileById(tileset.Autotiles[i]).Graphic.LoadTexture(Device);
            }

            // Load map
            LoadMap();
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
                GameMapPortion gamePortion;
                try
                {
                    gamePortion = WANOK.LoadBinaryDatas<GameMapPortion>(path);
                    Portions[new int[] { i, j }] = gamePortion;
                    GenTextures(gamePortion);
                }
                catch
                {
                    WANOK.PrintError("Error: could not load the map portion " + real_i + "-" + real_j + ".pmap");
                }
            }
            else
            {
                Portions[new int[] { i, j }] = null;
            }
        }

        // -------------------------------------------------------------------
        // GenTextures
        // -------------------------------------------------------------------

        public void GenTextures(int[] portion)
        {
            if (Portions[portion] != null)
            {
                GenTextures(Portions[portion]);
            }
        }

        public void GenTextures(GameMapPortion portion)
        {
            portion.GenFloor(Device, Game1.TexTileset);
            portion.GenAutotiles(Device);
            portion.GenSprites(Device);
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update(int[]  newPortion)
        {
            // Portion moving
            if (newPortion[0] != CurrentPortion[0] || newPortion[1] != CurrentPortion[1])
            {
                UpdateMovingPortion(newPortion, CurrentPortion);
            }
            CurrentPortion = newPortion;
        }

        // -------------------------------------------------------------------
        // UpdateMovingPortion
        // -------------------------------------------------------------------

        public void UpdateMovingPortion(int[] currentPortion, int[] previousPortion)
        {
            // If cursor going to right side
            if (currentPortion[0] > previousPortion[0])
            {
                for (int j = -WANOK.PORTION_RADIUS; j <= WANOK.PORTION_RADIUS; j++)
                {
                    for (int i = -WANOK.PORTION_RADIUS; i < WANOK.PORTION_RADIUS; i++)
                    {
                        SetPortion(i, j, i + 1, j);
                    }
                    LoadPortion(currentPortion, WANOK.PORTION_RADIUS, j);
                }
            }
            // If cursor going to left side
            else if (currentPortion[0] < previousPortion[0])
            {
                for (int j = -WANOK.PORTION_RADIUS; j <= WANOK.PORTION_RADIUS; j++)
                {
                    for (int i = WANOK.PORTION_RADIUS; i > -WANOK.PORTION_RADIUS; i--)
                    {
                        SetPortion(i, j, i - 1, j);
                    }
                    LoadPortion(currentPortion, -WANOK.PORTION_RADIUS, j);
                }
            }
            // If cursor going to up side
            if (currentPortion[1] > previousPortion[1])
            {
                for (int i = -WANOK.PORTION_RADIUS; i <= WANOK.PORTION_RADIUS; i++)
                {
                    for (int j = -WANOK.PORTION_RADIUS; j < WANOK.PORTION_RADIUS; j++)
                    {
                        SetPortion(i, j, i, j + 1);
                    }
                    LoadPortion(currentPortion, i, WANOK.PORTION_RADIUS);
                }
            }
            // If cursor going to down side
            else if (currentPortion[1] < previousPortion[1])
            {
                for (int i = -WANOK.PORTION_RADIUS; i <= WANOK.PORTION_RADIUS; i++)
                {
                    for (int j = WANOK.PORTION_RADIUS; j > -WANOK.PORTION_RADIUS; j--)
                    {
                        SetPortion(i, j, i, j - 1);
                    }
                    LoadPortion(currentPortion, i, -WANOK.PORTION_RADIUS);
                }
            }
        }

        // -------------------------------------------------------------------
        // SetPortion
        // -------------------------------------------------------------------

        public void SetPortion(int i, int j, int k, int l)
        {
            DisposeBuffers(new int[] { i, j }, false);
            Portions[new int[] { i, j }] = Portions[new int[] { k, l }];
        }

        // -------------------------------------------------------------------
        // LoadPortion
        // -------------------------------------------------------------------

        public void LoadPortion(int[] currentPortion, int i, int j)
        {
            DisposeBuffers(new int[] { i, j }, false);
            LoadPortion(currentPortion[0] + i, currentPortion[1] + j, i, j);
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GameTime gameTime, AlphaTestEffect effect, Camera camera)
        {
            Device.Clear(WANOK.GetColor(MapInfos.SkyColor));

            foreach (GameMapPortion gameMap in Portions.Values)
            {
                if (gameMap != null) gameMap.Draw(Device, effect, Game1.TexTileset, camera);
            }
        }

        // -------------------------------------------------------------------
        // DisposeBuffers
        // -------------------------------------------------------------------

        public void DisposeBuffers(int[] portion, bool nullable = true)
        {
            if (Portions[portion] != null)
            {
                Portions[portion].DisposeBuffers(Device, nullable);
            }
        }

        // -------------------------------------------------------------------
        // DisposeVertexBuffer
        // -------------------------------------------------------------------

        public void DisposeVertexBuffer()
        {
            foreach (KeyValuePair<int[], GameMapPortion> entry in Portions)
            {
                if (entry.Value != null) DisposeBuffers(entry.Key);
            }
        }
    }
}
