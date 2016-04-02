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
        private Vector3 Position;
        private Vector3 Target;
        private Vector3 UpVector = Vector3.Up;
        public Matrix Projection;
        public Matrix View { get { return Matrix.CreateLookAt(Position, Target, UpVector); } }
        public Matrix World;

        public Double HorizontalAngle = -90.0, TargetAngle = -90.0, VerticalAngle = 0.0, Distance = 220.0, Height = 100.0;
        private int RotateVelocity = 180;
        private Double RotateSteps = 90.0, RotateTick = 0.0;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Camera(Game game, Vector3 position, Vector3 target)
            :base(game)
        {
            this.Position = position;
            this.Target = target;
            
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 0.01f, 100000.0f);
            World = Matrix.Identity;
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update(GameTime gameTime, Hero hero, KeyboardState kb)
        {
            // Horizontal angle
            if (TargetAngle != HorizontalAngle)
            {
                float speed = (float)(RotateVelocity * (gameTime.ElapsedGameTime.Milliseconds) / 1000.0);
                if (TargetAngle > HorizontalAngle)
                {
                    HorizontalAngle += speed;
                    if (HorizontalAngle > TargetAngle) HorizontalAngle = TargetAngle;
                }
                else if (TargetAngle < HorizontalAngle){
                    HorizontalAngle -= speed;
                    if (HorizontalAngle < TargetAngle) HorizontalAngle = TargetAngle;
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

            // Updating camera according to hero position
            Target.X = hero.Position.X + hero.Size.X/4;
            Target.Y = hero.Position.Y;
            Target.Y = hero.Position.Z;

            // Camera position
            Position.X = Target.X - (float)(Distance * Math.Cos(HorizontalAngle * Math.PI / 180.0));
            Position.Y = Target.Y - (float)(Distance * Math.Sin(VerticalAngle * Math.PI / 180.0)) + (float)Height;
            Position.Z = Target.X - (float)(Distance * Math.Sin(HorizontalAngle * Math.PI / 180.0));

            // Rotate tick update
            RotateTick = gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
