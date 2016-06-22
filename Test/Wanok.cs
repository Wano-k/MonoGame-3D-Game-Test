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
        public static int SQUARE_SIZE = 16;
        public static float RELATION_SIZE { get { return (float)(BASIC_SQUARE_SIZE) / SQUARE_SIZE; } }
        public static int PORTION_SIZE = 16;
        public static int PORTION_RADIUS = 4;
        public static string MapsDirectoryPath { get { return Path.Combine("Content", "Datas", "Maps"); } }
        public static SystemDatas SystemDatas;


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
            new ErrorBox(message).Run();
        }
    }
}
