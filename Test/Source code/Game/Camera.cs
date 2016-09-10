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
    class Camera : GameComponent
    {
        protected Vector3 Position;
        protected Vector3 Target;
        protected Vector3 UpVector = Vector3.Up;
        public Matrix Projection;
        public Matrix View { get { return Matrix.CreateLookAt(Position, Target, UpVector); } }
        public Matrix World;
        public double HorizontalAngle = -90.0, TargetAngle = -90.0, VerticalAngle = 0.0, Distance = 200.0, Height = 100.0;
        public int RotateVelocity = 180;
        protected double RotateSteps = 90.0, RotateTick = 0.0;
        protected bool UpdateMapOrientation = false;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Camera(Game game)
            :base(game)
        {
            Position = Vector3.Zero;
            Target = Vector3.Zero;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
            World = Matrix.Identity;
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update(GameTime gameTime, Hero hero, KeyboardState kb, Map map)
        {
            // Horizontal angle
            if (TargetAngle != HorizontalAngle)
            {
                float speed = (float)(RotateVelocity * (gameTime.ElapsedGameTime.Milliseconds) / 1000.0);
                if (TargetAngle > HorizontalAngle)
                {
                    HorizontalAngle += speed;
                    if (HorizontalAngle > TargetAngle)
                    {
                        HorizontalAngle = TargetAngle;
                        UpdateMapOrientation = false;
                    }
                    else if (!UpdateMapOrientation && TargetAngle - HorizontalAngle < (RotateSteps / 2))
                    {
                        UpdateMapOrientation = true;
                        map.Orientation = (Orientation)(((int)map.Orientation + 1) % 4);
                        if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.D)) hero.OrientationEye = (Orientation)(((int)hero.OrientationEye + 1) % 4);
                    }
                }
                else if (TargetAngle < HorizontalAngle){
                    HorizontalAngle -= speed;
                    if (HorizontalAngle < TargetAngle)
                    {
                        HorizontalAngle = TargetAngle;
                        UpdateMapOrientation = false;
                    }
                    else if (!UpdateMapOrientation && HorizontalAngle - TargetAngle < (RotateSteps / 2))
                    {
                        UpdateMapOrientation = true;
                        map.Orientation = (Orientation)(((int)map.Orientation - 1) % 4);
                        if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.D)) hero.OrientationEye = (Orientation)(((int)hero.OrientationEye - 1) % 4);
                    }
                }
            }
            if (HorizontalAngle >= 270.0 || HorizontalAngle <= -450.0)
            {
                HorizontalAngle = -90.0;
                TargetAngle = -90.0;
            }

            // Keyboard 
            if (TargetAngle == HorizontalAngle)
            {
                if (kb.IsKeyDown(Keys.Left))
                {
                    TargetAngle -= RotateSteps;
                }
                else if (kb.IsKeyDown(Keys.Right))
                {
                    TargetAngle += RotateSteps;
                }
            }

            System.Diagnostics.Debug.Write(Target.X + " - " + hero.GetCenterX() + "\n");

            // Updating camera according to hero position
            Target.X = hero.GetCenterX();
            Target.Y = hero.Position.Y;
            Target.Z = hero.GetCenterZ();

            // Camera position

            Position.X = Target.X - (float)(Distance * Math.Cos(HorizontalAngle * Math.PI / 180.0));
            Position.Y = Target.Y - (float)(Distance * Math.Sin(VerticalAngle * Math.PI / 180.0)) + (float)Height;
            Position.Z = Target.Z - (float)(Distance * Math.Sin(HorizontalAngle * Math.PI / 180.0));

            // Rotate tick update
            RotateTick = gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
