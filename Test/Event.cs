using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Event
    {
        protected GraphicsDevice Device;
        public Vector3 Position;
        public Vector2 Size;
        protected VertexPositionTexture[] Vertices;
        protected VertexBuffer VB;
        protected IndexBuffer IB;
        protected int[] Indexes;
        protected int Frame = 0, FrameInactive = 0, FrameTick = 0, FrameTickInactive = 0, FrameDuration = 150, FrameDurationInactive = 200;
        protected int Frame_inactive = 0;
        protected bool Act = true;
        public Orientation OrientationEye = Orientation.South;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Event(GraphicsDevice device, Vector3 position, Vector2 size)
        {
            Device = device;

            // Position and size
            Position = position;
            Size = size;

            // Init buffers
            VB = new VertexBuffer(Device, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
            IB = new IndexBuffer(Device, IndexElementSize.ThirtyTwoBits, 6, BufferUsage.WriteOnly);
            Device.SetVertexBuffer(VB);
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

        // -------------------------------------------------------------------
        // CreateTex : coords = [x,y,width,height]
        // -------------------------------------------------------------------

        protected void CreateTex(int[] coords, Texture2D texture)
        {
            // Texture coords
            float left = ((float)coords[0]) / texture.Width;
            float top = ((float)coords[1]) / texture.Height;
            float bot = ((float)(coords[1] + coords[3])) / texture.Height;
            float right = ((float)(coords[0] + coords[2])) / texture.Width;

            // Vertex Position and Texture
            Vertices = new VertexPositionTexture[]
           {
               new VertexPositionTexture(WANOK.VERTICESSPRITE[0], new Vector2(left,top)),
               new VertexPositionTexture(WANOK.VERTICESSPRITE[1], new Vector2(right,top)),
               new VertexPositionTexture(WANOK.VERTICESSPRITE[2], new Vector2(right,bot)),
               new VertexPositionTexture(WANOK.VERTICESSPRITE[3], new Vector2(left,bot))
           };

            // Vertex Indexes
            Indexes = new int[]
            {
                0, 1, 2, 0, 2, 3
            };

            // Update buffers
            VB.SetData(Vertices);
            IB.SetData(Indexes);
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update(Orientation orientationMap)
        {
            //OrientationEye = (Orientation)(((int)OrientationEye + (int)orientationMap + 1) % 4);
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GameTime gameTime, Camera camera, AlphaTestEffect effect, Orientation orientationMap)
        {
            // Bounce
            int bounce = (Frame == 0 || Frame == 2) ? 0 : 1;
            int orientation = WANOK.Mod(((int)orientationMap - 2) * 3 + (int)OrientationEye, 4);

            // Setting effect
            effect.World = Matrix.Identity * Matrix.CreateScale(Size.X, Size.Y, 1.0f) * Matrix.CreateTranslation(-Size.X / 2, 0, 0) * Matrix.CreateRotationY((float)((-camera.HorizontalAngle - 90) * Math.PI / 180.0)) * Matrix.CreateTranslation(Position.X, Position.Y + bounce, Position.Z);


            if (Act)
            {
                effect.Texture = Game1.TexHeroAct;
                CreateTex(new int[] { FrameInactive * 32, orientation * 32, (int)Size.X, (int)Size.Y }, Game1.TexHeroAct);
            }
            else
            {
                effect.Texture = Game1.TexHero;
                CreateTex(new int[] { Frame * 32, orientation * 32, (int)Size.X, (int)Size.Y }, Game1.TexHero);
            }

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length, Indexes, 0, 2);
            }
        }
    }
}
