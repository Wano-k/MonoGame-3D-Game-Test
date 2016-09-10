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
        protected int Frame = 0, FrameInactive = 0, FrameTick = 0, FrameTickInactive = 0, FrameDuration = 150, FrameDurationInactive = 200, MaxFrames;
        protected bool Act = false;
        public Orientation OrientationEye = Orientation.South;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Event(Vector3 position, Vector2 size, int maxFrames)
        {
            // Position and size
            Position = position;
            Size = size;
            MaxFrames = maxFrames;
        }

        // -------------------------------------------------------------------
        // GetX
        // -------------------------------------------------------------------

        public int GetX()
        {
            return (int)((Position.X + 1) / WANOK.SQUARE_SIZE);
        }

        public float GetCenterX()
        {
            return Position.X + (WANOK.SQUARE_SIZE / 2);
        }

        // -------------------------------------------------------------------
        // GetZ
        // -------------------------------------------------------------------

        public int GetZ()
        {
            return (int)((Position.Z + 1) / WANOK.SQUARE_SIZE);
        }

        public float GetCenterZ()
        {
            return Position.Z + (WANOK.SQUARE_SIZE / 2);
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
