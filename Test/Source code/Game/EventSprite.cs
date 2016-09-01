using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Paper_Maker;

namespace Test
{
    class EventSprite : Event
    {
        public Sprite Sprite;
        public int[] InitialTexture;

        VertexBuffer VB;
        VertexPositionTexture[] VerticesArray;
        IndexBuffer IB;
        int[] IndexesArray;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public EventSprite(Vector3 position, Vector2 size, Sprite sprite, int[] initialTexture) : base(position, size)
        {
            Sprite = sprite;
            InitialTexture = initialTexture;
        }

        // -------------------------------------------------------------------
        // CreatePortion
        // -------------------------------------------------------------------

        public void CreatePortion(GraphicsDevice device, Texture2D texture2D, int[] texture, bool isTileset)
        {
            DisposeBuffers(device);

            // Building vertex buffer indexed
            List <VertexPositionTexture> verticesList = new List<VertexPositionTexture>();
            List<int> indexesList = new List<int>();
            int[] indexes = new int[]
            {
                0, 1, 2, 0, 2, 3
            };
            if (!isTileset || (texture[2] * WANOK.SQUARE_SIZE <= Game1.TexTileset.Width && texture[3] * WANOK.SQUARE_SIZE <= Game1.TexTileset.Height))
            {
                foreach (VertexPositionTexture vertex in CreateTex(texture2D, texture, isTileset))
                {
                    verticesList.Add(vertex);
                }
                for (int n = 0; n < 6; n++)
                {
                    indexesList.Add(indexes[n]);
                }

                VerticesArray = verticesList.ToArray();
                IndexesArray = indexesList.ToArray();
                IB = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, IndexesArray.Length, BufferUsage.None);
                IB.SetData(IndexesArray);
                VB = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, VerticesArray.Length, BufferUsage.None);
                VB.SetData(VerticesArray);
            }
        }

        // -------------------------------------------------------------------
        // CreateTex
        // -------------------------------------------------------------------

        protected VertexPositionTexture[] CreateTex(Texture2D texture, int[] coords, bool isTileset)
        {
            float pixelX = coords[0], pixelY = coords[1], pixelWidth = coords[2], pixelHeight = coords[3];
            if (isTileset)
            {
                pixelX *= WANOK.SQUARE_SIZE;
                pixelY *= WANOK.SQUARE_SIZE;
                pixelWidth *= WANOK.SQUARE_SIZE;
                pixelHeight *= WANOK.SQUARE_SIZE;
            }

            // Texture coords
            float left = pixelX / texture.Width;
            float top = pixelY / texture.Height;
            float bot = (pixelY + pixelHeight) / texture.Height;
            float right = (pixelX + pixelWidth) / texture.Width;

            // Adjust in order to limit risk of textures flood
            float width = left + right;
            float height = top + bot;
            left += width / WANOK.COEF_BORDER_TEX;
            right -= width / WANOK.COEF_BORDER_TEX;
            top += height / WANOK.COEF_BORDER_TEX;
            bot -= height / WANOK.COEF_BORDER_TEX;

            // Vertex Position and Texture
            return new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(0, pixelHeight, 0), new Vector2(left, top)),
                new VertexPositionTexture(new Vector3(pixelWidth, pixelHeight, 0), new Vector2(right, top)),
                new VertexPositionTexture(new Vector3(pixelWidth, 0, 0), new Vector2(right, bot)),
                new VertexPositionTexture(new Vector3(0, 0, 0), new Vector2(left, bot))
            };
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GraphicsDevice device, Camera camera, AlphaTestEffect effect, Orientation orientationMap, SystemGraphic characterGraphic, SystemGraphic characterGraphicAct)
        {
            Vector3 position;
            int bounce = (Frame == 0 || Frame == 2) ? 0 : 1;

            if (characterGraphic == null)
            {
                effect.Texture = Game1.TexTileset;
                position = Act ? Position : new Vector3(Position.X, Position.Y + bounce, Position.Z);
            }
            else
            {
                int orientation = WANOK.Mod(((int)orientationMap - 2) * 3 + (int)OrientationEye, 4);

                if (Act)
                {
                    effect.Texture = characterGraphicAct == null ? Game1.TexCharacters[characterGraphic] : Game1.TexCharacters[characterGraphicAct];
                    CreatePortion(device, effect.Texture, new int[] { FrameInactive * 32, orientation * 32, (int)Size.X, (int)Size.Y }, false);
                    position = Position;
                }
                else
                {
                    effect.Texture = Game1.TexCharacters[characterGraphic];
                    CreatePortion(device, effect.Texture, new int[] { Frame * 32, orientation * 32, (int)Size.X, (int)Size.Y }, false);
                    position = new Vector3(Position.X, Position.Y + bounce, Position.Z);
                }
            }

            Sprite.Draw(device, effect, VerticesArray, IndexesArray, camera, position, (int)Size.X, (int)Size.Y);
        }

        // -------------------------------------------------------------------
        // DisposeBuffers
        // -------------------------------------------------------------------

        public void DisposeBuffers(GraphicsDevice device, bool nullable = true)
        {
            if (VB != null)
            {
                device.SetVertexBuffer(null);
                device.Indices = null;
                VB.Dispose();
                IB.Dispose();
                if (nullable)
                {
                    VB = null;
                    IB = null;
                }
            }
        }
    }
}
