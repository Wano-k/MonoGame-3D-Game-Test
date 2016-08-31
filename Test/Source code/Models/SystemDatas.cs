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
        public static int MAX_SWITCHES = 9999;

        public Dictionary<string, string> GameName;
        public List<string> Langs;
        public string StartMapName = "MAP0001";
        public int[] StartPosition = new int[] { 12, 0, 0, 12 };
        public int ScreenWidth = 640;
        public int ScreenHeight = 480;
        public bool FullScreen = false;
        public int SquareSize = 16;
        public string PathRTP;

        // ListBoxes
        public List<SystemColor> Colors = new List<SystemColor>();

        // Switches & variables
        public List<SuperListItemNameWithoutLang> Switches = new List<SuperListItemNameWithoutLang>();


        // -------------------------------------------------------------------
        // GetColorById
        // -------------------------------------------------------------------

        public SystemColor GetColorById(int id)
        {
            if (id > Colors.Count) return new SystemColor(-1);
            return Colors.Find(i => i.Id == id);
        }

        // -------------------------------------------------------------------
        // GetColorIndexById
        // -------------------------------------------------------------------

        public int GetColorIndexById(int id)
        {
            return Colors.IndexOf(GetColorById(id));
        }

        // -------------------------------------------------------------------
        // GetSwitchById
        // -------------------------------------------------------------------

        public SuperListItemNameWithoutLang GetSwitchById(int id)
        {
            if (id > Switches.Count) return new SuperListItemNameWithoutLang(-1);
            return Switches.Find(i => i.Id == id);
        }
    }
}
