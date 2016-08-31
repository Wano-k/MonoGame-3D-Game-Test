using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace RPG_Paper_Maker
{
    [Serializable]
    class Mountain
    {
        public int SquareHeight;
        public int PixelHeight;
        public int Angle;
        public bool DrawTop = true;
        public bool DrawBot = true;
        public bool DrawLeft = true;
        public bool DrawRight = true;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Mountain(int squareHeight, int pixelHeight, int angle)
        {
            SquareHeight = squareHeight;
            PixelHeight = pixelHeight;
            Angle = angle;
        }
    }
}
