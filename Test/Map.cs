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
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GameTime gameTime, AlphaTestEffect effect, Camera camera)
        {
            Device.Clear(WANOK.GetColor(MapInfos.SkyColor));

            // Drawing Floors
            effect.World = Matrix.Identity * Matrix.CreateScale(WANOK.SQUARE_SIZE, 1.0f, WANOK.SQUARE_SIZE);
            effect.VertexColorEnabled = false;
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
            Portions[portion].DisposeBuffers(Device, nullable);
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
