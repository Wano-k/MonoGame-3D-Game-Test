using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(Position, Target, UpVector);
            }
        }
        public Matrix World;

        public Camera(Game game, Vector3 position, Vector3 target)
            :base(game)
        {
            this.Position = position;
            this.Target = target;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.Width / game.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            World = Matrix.Identity;
        }
    }
}
