using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class SystemColor : SuperListItem
    {
        public int[] Color = new int[] { 0, 0, 0 };
        public static SystemColor BlackColor = new SystemColor("Black", new int[] { 0, 0, 0 });
        public static SystemColor BlackGrayColor = new SystemColor("Black-Gray", new int[] { 32, 32, 32 });


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public SystemColor(string n, int[] color)
        {
            Name = n;
            Color = color;
        }

        // -------------------------------------------------------------------
        // GetMonogameColor
        // -------------------------------------------------------------------

        public Microsoft.Xna.Framework.Color GetMonogameColor()
        {
            return new Microsoft.Xna.Framework.Color(Color[0], Color[1], Color[2]);
        }
    }
}
