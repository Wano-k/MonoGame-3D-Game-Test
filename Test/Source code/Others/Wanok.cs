using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using RPG_Paper_Maker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

// -------------------------------------------------------------------
// STATIC Class for global variables
// -------------------------------------------------------------------

namespace Test
{
    static class WANOK
    {
        public static Vector3[] VERTICESFLOOR = new Vector3[]
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 1.0f),
            new Vector3(0.0f, 0.0f, 1.0f)
        };
        public static Vector3[] VERTICESSPRITE = new Vector3[]
        {
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(1.0f, 1.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f)
        };
        public static int BASIC_SQUARE_SIZE = 32;
        public static int SQUARE_SIZE { get { return Game.System.SquareSize; } }
        public static float RELATION_SIZE { get { return (float)(BASIC_SQUARE_SIZE) / SQUARE_SIZE; } }
        public static int PORTION_SIZE = 16;
        public static int PORTION_RADIUS = 6;
        public static int COEF_BORDER_TEX = 10000;
        public static string NONE_IMAGE_STRING = "<None>";
        public static string TILESET_IMAGE_STRING = "<Tileset>";
        public static GameDatas Game = new GameDatas();
        public static string CurrentLang = "eng";

        // PATHS
        public static string HeroesPath { get { return Path.Combine("Content", "Datas", "Heroes.rpmd"); } }
        public static string SystemPath { get { return Path.Combine("Content", "Datas", "System.rpmd"); } }
        public static string BattleSystemPath { get { return Path.Combine("Content", "Datas", "BattleSystem.rpmd"); } }
        public static string TilesetsPath { get { return Path.Combine("Content", "Datas", "Tilesets.rpmd"); } }
        public static string MapsDirectoryPath { get { return Path.Combine("Content", "Datas", "Maps"); } }


        // -------------------------------------------------------------------
        // SaveDatas
        // -------------------------------------------------------------------

        public static void SaveDatas(object obj, string path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(json);
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
            }
        }

        // -------------------------------------------------------------------
        // SaveBinaryDatas
        // -------------------------------------------------------------------

        public static void SaveBinaryDatas(object obj, string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
                fs.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
            }
        }

        // -------------------------------------------------------------------
        // LoadDatas
        // -------------------------------------------------------------------

        public static T LoadDatas<T>(string path)
        {
            T obj;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string json = sr.ReadToEnd();
                obj = JsonConvert.DeserializeObject<T>(json);
                sr.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                obj = default(T);
                System.Diagnostics.Debug.Write(e.Message);
            }

            return obj;
        }

        // -------------------------------------------------------------------
        // LoadBinaryDatas
        // -------------------------------------------------------------------

        public static T LoadBinaryDatas<T>(string path)
        {
            T obj;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                obj = (T)formatter.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                obj = default(T);
            }

            return obj;
        }

        // -------------------------------------------------------------------
        // LoadBinaryDatas
        // -------------------------------------------------------------------

        public static void PrintError(string message)
        {
            try
            {
                new ErrorBox(message).Run();
            }
            catch { }
        }

        // -------------------------------------------------------------------
        // LoadPortionMap
        // -------------------------------------------------------------------

        public static GameMapPortion LoadPortionMap(string mapName, int i, int j)
        {
            string path = Path.Combine(MapsDirectoryPath, mapName, i + "-" + j + ".pmap");
            if (File.Exists(path)) return LoadBinaryDatas<GameMapPortion>(path);
            else return null;
        }

        // -------------------------------------------------------------------
        // GetColor
        // -------------------------------------------------------------------

        public static Color GetColor(int id)
        {
            return SystemColor.GetMonogameColor(Game.System.GetColorById(id));
        }

        // -------------------------------------------------------------------
        // GetTilesetTexturePath
        // -------------------------------------------------------------------

        public static string GetTilesetTexturePath(int id)
        {
            return Game.Tilesets.GetTilesetById(id).Graphic.GetGraphicPath();
        }

        // -------------------------------------------------------------------
        // Mod
        // -------------------------------------------------------------------

        public static int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        // -------------------------------------------------------------------
        // GetPortion
        // -------------------------------------------------------------------

        public static int[] GetPortion(int x, int z)
        {
            return new int[] { x / PORTION_SIZE, z / PORTION_SIZE };
        }

        // -------------------------------------------------------------------
        // GetDefaultNames
        // -------------------------------------------------------------------

        public static Dictionary<string, string> GetDefaultNames(string name = "")
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["eng"] = name;

            return dic;
        }

        // -------------------------------------------------------------------
        // GetVector3Position
        // -------------------------------------------------------------------

        public static Vector3 GetVector3Position(int[] coords)
        {
            return new Vector3(coords[0] * SQUARE_SIZE, GetCoordsPixelHeight(coords), coords[3] * SQUARE_SIZE);
        }

        // -------------------------------------------------------------------
        // GetPixelHeight
        // -------------------------------------------------------------------

        public static int GetPixelHeight(int[] height)
        {
            return GetPixelHeight(height[0], height[1]);
        }

        public static int GetCoordsPixelHeight(int[] coords)
        {
            return GetPixelHeight(coords[1], coords[2]);
        }

        public static int GetPixelHeight(int y, int yPlus)
        {
            return (y * SQUARE_SIZE) + yPlus;
        }
    }
}
