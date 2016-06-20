using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Hero : Event
    {

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Hero(GraphicsDevice device, Vector3 position) : base(device, position, new Vector2(32, 32))
        {

        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update(GameTime gameTime, Camera camera, Map map, KeyboardState kb)
        {
            double angle = camera.HorizontalAngle;
            int x = GetX(), y = GetY(), x_plus, z_plus;
            double speed = 1.1 * camera.RotateVelocity * (gameTime.ElapsedGameTime.Milliseconds) / 1000.0;

            // Updating diag speed
            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S)) // Up / Down
            {
                if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.D)) // Left / Right
                {
                    speed *= 0.7;
                }
            }

            float previous_x = Position.X, previous_y = Position.Y, previous_z = Position.Z;
            if (kb.IsKeyDown(Keys.W))
            {
                x_plus = (int)(speed * (Math.Cos(angle * Math.PI / 180.0)));
                z_plus = (int)(speed * (Math.Sin(angle * Math.PI / 180.0)));
                Position.Z += z_plus;
                Position.X += x_plus;
                //if ((y > 0 && y_plus < 0) || (y < map.Size[1] && y_plus > 0)) Position.Y += y_plus;
                //if (y_plus == 0 && ((x > 0 && x_plus < 0) || (x < map.Size[0] && x_plus > 0))) Position.X += x_plus;
            }
            if (kb.IsKeyDown(Keys.S))
            {
                x_plus = (int)(speed * (Math.Cos(angle * Math.PI / 180.0)));
                z_plus = (int)(speed * (Math.Sin(angle * Math.PI / 180.0)));
                Position.Z -= z_plus;
                Position.X -= x_plus;
            }
            if (kb.IsKeyDown(Keys.A))
            {
                x_plus = (int)(speed * (Math.Cos((angle - 90.0) * Math.PI / 180.0)));
                z_plus = (int)(speed * (Math.Sin((angle - 90.0) * Math.PI / 180.0)));
                Position.Z += z_plus;
                Position.X += x_plus;
            }
            if (kb.IsKeyDown(Keys.D))
            {
                x_plus = (int)(speed * (Math.Cos((angle - 90.0) * Math.PI / 180.0)));
                z_plus = (int)(speed * (Math.Sin((angle - 90.0) * Math.PI / 180.0)));
                Position.Z -= z_plus;
                Position.X -= x_plus;
            }

            // Frame update
            if (previous_x != Position.X || previous_y != Position.Y || previous_z != Position.Z)
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
