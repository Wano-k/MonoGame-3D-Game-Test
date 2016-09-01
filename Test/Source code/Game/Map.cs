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
        public Events Events { get; set; }
        public Dictionary<int[], GameMapPortion> Portions;
        public Dictionary<int[], EventsPortion> EventsPortions;
        public Orientation Orientation = Orientation.North; // Camera orientation
        public int[] CurrentPortion;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Map(GraphicsDevice device, string mapName)
        {
            string pathDir = Path.Combine(WANOK.MapsDirectoryPath, mapName);
            if (!Directory.Exists(pathDir)) WANOK.PrintError("Error: could not find the " + mapName + " map directory.");
            if (WANOK.Game.System.StartMapName == "") WANOK.PrintError("Error: could not find the start position.");
            CurrentPortion = WANOK.GetPortion(WANOK.Game.System.StartPosition[0], WANOK.Game.System.StartPosition[3]);
            Device = device;
            MapInfos = WANOK.LoadBinaryDatas<MapInfos>(Path.Combine(WANOK.MapsDirectoryPath, mapName, "infos.map"));
            if (MapInfos == null) WANOK.PrintError("infos.map version is not compatible.");
            Events = WANOK.LoadBinaryDatas<Events>(Path.Combine(WANOK.MapsDirectoryPath, mapName, "events.map"));
            if (Events == null) WANOK.PrintError("Events.rpm version is not compatible.");


            // Set textures
            if (Game1.TexTileset != null) Game1.TexTileset.Dispose();
            foreach (int i in Game1.TexAutotiles.Keys)
            {
                Game1.TexAutotiles[i].Dispose();
            }
            Game1.TexAutotiles.Clear();
            foreach (int i in Game1.TexReliefs.Keys)
            {
                Game1.TexReliefs[i].Dispose();
            }
            Game1.TexReliefs.Clear();
            foreach (Texture2D texture in Game1.TexCharacters.Values)
            {
                texture.Dispose();
            }
            Game1.TexCharacters.Clear();

            SystemTileset tileset = WANOK.Game.Tilesets.GetTilesetById(MapInfos.Tileset);
            Game1.TexTileset = tileset.Graphic.LoadTexture(device);
            for (int i = 0; i < tileset.Autotiles.Count; i++)
            {
                Game1.TexAutotiles[tileset.Autotiles[i]] = WANOK.Game.Tilesets.GetAutotileById(tileset.Autotiles[i]).Graphic.LoadTexture(Device);
            }
            for (int i = 0; i < tileset.Reliefs.Count; i++)
            {
                Game1.TexReliefs[tileset.Reliefs[i]] = WANOK.Game.Tilesets.GetReliefById(tileset.Reliefs[i]).Graphic.LoadTexture(Device);
            }
            LoadEventTextures();

            // Load map
            LoadMap();
        }

        // -------------------------------------------------------------------
        // LoadMap
        // -------------------------------------------------------------------

        public void LoadMap()
        {
            Portions = new Dictionary<int[], GameMapPortion>(new IntArrayComparer());
            EventsPortions = new Dictionary<int[], EventsPortion>(new IntArrayComparer());
            int k = (WANOK.Game.System.StartPosition[0] / WANOK.PORTION_SIZE);
            int l = (WANOK.Game.System.StartPosition[3] / WANOK.PORTION_SIZE);

            for (int i = -WANOK.PORTION_RADIUS; i <= WANOK.PORTION_RADIUS; i++)
            {
                for (int j = -WANOK.PORTION_RADIUS; j <= WANOK.PORTION_RADIUS; j++)
                {
                    LoadPortion(i + k, j + l, i, j);
                }
            }
        }

        // -------------------------------------------------------------------
        // LoadEventSpriteTexture
        // -------------------------------------------------------------------

        public void LoadSpriteTexture(SystemGraphic graphic, SystemGraphic graphicAct)
        {
            if (!graphic.IsTileset())
            {
                Game1.LoadSystemGraphic(graphic, Device);
                if (graphicAct != null) Game1.LoadSystemGraphic(graphicAct, Device);
            }
        }

        // -------------------------------------------------------------------
        // LoadEventTextures
        // -------------------------------------------------------------------

        public void LoadEventTextures()
        {
            foreach (Dictionary<int[], SystemEvent> entry in Events.CompleteList.Values)
            {
                foreach (SystemEvent ev in entry.Values)
                {
                    for (int i = 0; i < ev.Pages.Count; i++)
                    {
                        switch (ev.Pages[i].GraphicDrawType)
                        {
                            case DrawType.None:
                                break;
                            case DrawType.Floors:
                                break;
                            case DrawType.Autotiles:
                                break;
                            case DrawType.FaceSprite:
                                LoadSpriteTexture(ev.Pages[i].Graphic, ev.Pages[i].Options.StopAnimation);
                                break;
                            case DrawType.FixSprite:
                                LoadSpriteTexture(ev.Pages[i].Graphic, ev.Pages[i].Options.StopAnimation);
                                break;
                            case DrawType.DoubleSprite:
                                LoadSpriteTexture(ev.Pages[i].Graphic, ev.Pages[i].Options.StopAnimation);
                                break;
                            case DrawType.QuadraSprite:
                                LoadSpriteTexture(ev.Pages[i].Graphic, ev.Pages[i].Options.StopAnimation);
                                break;
                            case DrawType.OnFloorSprite:
                                LoadSpriteTexture(ev.Pages[i].Graphic, ev.Pages[i].Options.StopAnimation);
                                break;
                            case DrawType.Montains:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        // -------------------------------------------------------------------
        // LoadPortion
        // -------------------------------------------------------------------

        public void LoadPortion(int real_i, int real_j, int i, int j)
        {
            int[] portion = new int[] { i, j };
            int[] globalPortion = new int[] { real_i, real_j };
            Portions[portion] = WANOK.LoadPortionMap(MapInfos.RealMapName, real_i, real_j);
            EventsPortions[portion] = new EventsPortion(Events.CompleteList.ContainsKey(globalPortion) ? Events.CompleteList[globalPortion] : null);
            if (Portions[portion] != null)
            {
                GenTextures(portion);
            }
            GenEvent(portion, globalPortion);
        }

        // -------------------------------------------------------------------
        // GenTextures
        // -------------------------------------------------------------------

        public void GenTextures(int[] portion)
        {
            if (Portions[portion] != null)
            {
                Portions[portion].GenFloor(Device, Game1.TexTileset);
                Portions[portion].GenAutotiles(Device);
                Portions[portion].GenSprites(Device);
                Portions[portion].GenMountains(Device);
            }
        }

        public void GenEvent(int[] portion, int[] globalPortion)
        {
            if (Events.CompleteList.ContainsKey(globalPortion)) EventsPortions[portion].GenEvents(Device, Events.CompleteList[globalPortion]);
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update(int[] newPortion)
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
            EventsPortions[new int[] { i, j }] = EventsPortions[new int[] { k, l }];
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

            for (int i = -WANOK.PORTION_RADIUS; i <= WANOK.PORTION_RADIUS; i++)
            {
                for (int j = -WANOK.PORTION_RADIUS; j <= WANOK.PORTION_RADIUS; j++)
                {
                    int[] portion = new int[] { i, j };
                    int[] globalPortion = new int[] { i + CurrentPortion[0], j + CurrentPortion[1] };

                    // map portion
                    GameMapPortion gameMap = Portions[portion];
                    if (gameMap != null) gameMap.Draw(Device, effect, Game1.TexTileset, camera);

                    // events
                    if (Events.CompleteList.ContainsKey(globalPortion))
                        EventsPortions[portion].DrawSprites(Device, effect, camera, Orientation, Events.CompleteList[globalPortion]);
                }
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
            EventsPortions[portion].DisposeBuffers(Device, nullable);
        }

        // -------------------------------------------------------------------
        // DisposeVertexBuffer
        // -------------------------------------------------------------------

        public void DisposeVertexBuffer()
        {
            foreach (int[] coords in Portions.Keys)
            {
                DisposeBuffers(coords);
            }
        }
    }
}
