using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RPG_Paper_Maker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Hero : EventSprite
    {

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Hero(GraphicsDevice device, Vector3 position, Texture2D texture) : base(position, new Vector2(32, 16), 4, new Sprite(DrawType.FaceSprite, new int[] { 0, 0 }, 0), new int[] { 0, 0, 32, 32 })
        {

        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update(GameTime gameTime, Camera camera, Map map, KeyboardState kb)
        {
            double angle = camera.HorizontalAngle;
            float x_plus, z_plus;
            double speed = Speed * ((gameTime.ElapsedGameTime.Milliseconds) * SpeedBasicCoef * WANOK.SQUARE_SIZE);
            bool isAKeyDown = false;

            // Updating diag speed
            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S)) // Up / Down
            {
                if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.D)) // Left / Right
                {
                    speed = Math.Sqrt((speed * speed) / 2);
                }
            }

            if (kb.IsKeyDown(Keys.A)) // Left
            {
                x_plus = (float)(speed * (Math.Cos((angle - 90.0) * Math.PI / 180.0)));
                z_plus = (float)(speed * (Math.Sin((angle - 90.0) * Math.PI / 180.0)));
                if ((Position.X > 0 && x_plus < 0) || (Position.X < (map.MapInfos.Width - 1) * WANOK.SQUARE_SIZE && x_plus > 0)) Position.X += x_plus;
                if (((int)x_plus) == 0 && ((Position.Z > 0 && z_plus < 0) || (Position.Z < (map.MapInfos.Height - 1) * WANOK.SQUARE_SIZE && z_plus > 0))) Position.Z += z_plus;
                isAKeyDown = true;
                OrientationEye = (Orientation)WANOK.Mod(((int)map.Orientation) - 1, 4);
            }
            if (kb.IsKeyDown(Keys.D)) // Right
            {
                x_plus = (float)(speed * (Math.Cos((angle - 90.0) * Math.PI / 180.0)));
                z_plus = (float)(speed * (Math.Sin((angle - 90.0) * Math.PI / 180.0)));
                if ((Position.X < (map.MapInfos.Width - 1) * WANOK.SQUARE_SIZE && x_plus < 0) || (Position.X > 0 && x_plus > 0)) Position.X -= x_plus;
                if (((int)x_plus) == 0 && ((Position.Z < (map.MapInfos.Height - 1) * WANOK.SQUARE_SIZE && z_plus < 0) || (Position.Z > 0 && z_plus > 0))) Position.Z -= z_plus;
                isAKeyDown = true;
                OrientationEye = (Orientation)WANOK.Mod(((int)map.Orientation) + 1, 4);
            }
            if (kb.IsKeyDown(Keys.W)) // Up
            {
                x_plus = (float)(speed * (Math.Cos(angle * Math.PI / 180.0)));
                z_plus = (float)(speed * (Math.Sin(angle * Math.PI / 180.0)));
                if ((Position.Z > 0 && z_plus < 0) || (Position.Z < (map.MapInfos.Height - 1) * WANOK.SQUARE_SIZE && z_plus > 0)) Position.Z += z_plus;
                if (((int)z_plus) == 0 && ((Position.X > 0 && x_plus < 0) || (Position.X < (map.MapInfos.Width - 1) * WANOK.SQUARE_SIZE && x_plus > 0))) Position.X += x_plus;
                isAKeyDown = true;
                OrientationEye = map.Orientation;
            }
            if (kb.IsKeyDown(Keys.S)) // Down
            {
                x_plus = (float)(speed * (Math.Cos(angle * Math.PI / 180.0)));
                z_plus = (float)(speed * (Math.Sin(angle * Math.PI / 180.0)));
                if ((Position.Z < (map.MapInfos.Height - 1) * WANOK.SQUARE_SIZE && z_plus < 0) || (Position.Z > 0 && z_plus > 0)) Position.Z -= z_plus;
                if (((int)z_plus) == 0 && ((Position.X < (map.MapInfos.Width - 1) * WANOK.SQUARE_SIZE && x_plus < 0) || (Position.X > 0 && x_plus > 0))) Position.X -= x_plus;
                isAKeyDown = true;
                OrientationEye = (Orientation)WANOK.Mod(((int)map.Orientation) + 2, 4);
            }

            // Frame update
            if (isAKeyDown)
            {
                FrameInactive = 0;
                Act = false;
                FrameTick += gameTime.ElapsedGameTime.Milliseconds;
                if (FrameTick >= FrameDuration)
                {
                    Frame += 1;
                    if (Frame > 3) Frame = 0;
                    FrameTick = 0;
                }
            }
            else
            {
                Frame = 0;
                Act = true;
                FrameTickInactive += gameTime.ElapsedGameTime.Milliseconds;
                if (FrameTickInactive >= FrameDurationInactive)
                {
                    FrameInactive += 1;
                    if (FrameInactive > 3) FrameInactive = 0;
                    FrameTickInactive = 0;
                }
            }
        }
    }
}
