using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class SystemDatas
    {
        public Dictionary<string, string> GameName;
        public List<string> Langs;
        public string StartMapName = "MAP0001";
        public int[] StartPosition = new int[] { 12, 0, 0, 12 };
        public int ScreenWidth = 640;
        public int ScreenHeight = 480;
        public bool FullScreen = false;
        public int SquareSize = 16;
        public List<Tileset> Tilesets = new List<Tileset>();
        public List<SystemAutotile> Autotiles = new List<SystemAutotile>();
        public List<SystemRelief> Reliefs = new List<SystemRelief>();
        public List<SystemColor> Colors = new List<SystemColor>();
        public string PathRTP;

        // -------------------------------------------------------------------
        // GetColorById
        // -------------------------------------------------------------------

        public SystemColor GetColorById(int id)
        {
            if (id > Colors.Count) return new SystemColor(-1);
            return Colors.Find(i => i.Id == id);
        }

        // -------------------------------------------------------------------
        // GetTilesetById
        // -------------------------------------------------------------------

        public Tileset GetTilesetById(int id)
        {
            if (id > Tilesets.Count) return new Tileset(-1);
            return Tilesets.Find(i => i.Id == id);
        }

        // -------------------------------------------------------------------
        // GetAutotileById
        // -------------------------------------------------------------------

        public SystemAutotile GetAutotileById(int id)
        {
            if (id > Autotiles.Count) return new SystemAutotile(-1);
            return Autotiles.Find(i => i.Id == id);
        }

        // -------------------------------------------------------------------
        // GetReliefById
        // -------------------------------------------------------------------

        public SystemRelief GetReliefById(int id)
        {
            if (id == -1 || id > Reliefs.Count) return new SystemRelief(-1);
            return Reliefs.Find(i => i.Id == id);
        }

        // -------------------------------------------------------------------
        // GetReliefIndexById
        // -------------------------------------------------------------------

        public int GetReliefIndexById(int id)
        {
            return Reliefs.IndexOf(GetReliefById(id));
        }

        // -------------------------------------------------------------------
        // NoStart
        // -------------------------------------------------------------------

        public void NoStart()
        {
            StartMapName = "";
            StartPosition = new int[] { 0, 0, 0 };
        }
    }
}
