using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    abstract class Event 
    {
        public static double SpeedBasicCoef = 0.00666;
        public Vector3 Position;
        public Vector2 Size;
        public double Speed = 1.0;
        protected int Frame = 0, FrameInactive = 0, FrameTick = 0, FrameTickInactive = 0, FrameDuration = 150, FrameDurationInactive = 200;
        protected int Frame_inactive = 0;
        protected bool Act = true;
        public Orientation OrientationEye = Orientation.South;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Event(Vector3 position, Vector2 size)
        {
            // Position and size
            Position = position;
            Size = size;
        }

        // -------------------------------------------------------------------
        // GetX
        // -------------------------------------------------------------------

        public int GetX()
        {
            return (int)((Position.X + 1) / WANOK.SQUARE_SIZE);
        }

        // -------------------------------------------------------------------
        // GetZ
        // -------------------------------------------------------------------

        public int GetZ()
        {
            return (int)((Position.Z + 1) / WANOK.SQUARE_SIZE);
        }

        // -------------------------------------------------------------------
        // GetPortion
        // -------------------------------------------------------------------

        public int[] GetPortion()
        {
            return WANOK.GetPortion(GetX(), GetZ());
        }
    }
}
