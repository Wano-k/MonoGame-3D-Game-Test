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
        public class EventSpriteVertices
        {
            public VertexBuffer VB;
            public VertexPositionTexture[] VerticesArray;
            public IndexBuffer IB;
            public int[] IndexesArray;

            
            // -------------------------------------------------------------------
            // CreatePortion
            // -------------------------------------------------------------------

            public void CreatePortion(GraphicsDevice device, Texture2D texture2D, int[] texture, bool isTileset)
            {
                DisposeBuffers(device);

                // Building vertex buffer indexed
                if (!isTileset || (texture[2] * WANOK.SQUARE_SIZE <= Game1.TexTileset.Width && texture[3] * WANOK.SQUARE_SIZE <= Game1.TexTileset.Height))
                {
                    int count = 0;
                    VerticesArray = new VertexPositionTexture[4];
                    foreach (VertexPositionTexture vertex in CreateTex(texture2D, texture, isTileset))
                    {
                        VerticesArray[count++] = vertex;
                    }
                    IndexesArray = new int[]
                    {
                    0, 1, 2, 0, 2, 3
                    };

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

        public Sprite Sprite;
        public int[] InitialTexture;
        Dictionary<bool, Dictionary<int, Dictionary<int, EventSpriteVertices>>> Vertices;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public EventSprite(Vector3 position, Vector2 size, int maxFrames, Sprite sprite, int[] initialTexture) : base(position, size, maxFrames)
        {
            Sprite = sprite;
            InitialTexture = initialTexture;
            Frame = initialTexture[0] / initialTexture[2];
            OrientationEye = (Orientation)(initialTexture[1] / initialTexture[3]);

            Vertices = new Dictionary<bool, Dictionary<int, Dictionary<int, EventSpriteVertices>>>();
            foreach (bool act in new bool[] { true, false })
            {
                Vertices[act] = new Dictionary<int, Dictionary<int, EventSpriteVertices>>();
                for (int frame = 0; frame < maxFrames; frame++)
                {
                    Vertices[act][frame] = new Dictionary<int, EventSpriteVertices>();
                    for (int orientation = 0; orientation < 4; orientation++)
                    {
                        Vertices[act][frame][orientation] = new EventSpriteVertices();
                    }
                }
            }
        }

        // -------------------------------------------------------------------
        // CreatePortion
        // -------------------------------------------------------------------

        public void CreatePortion(GraphicsDevice device, Texture2D texture2D, bool isTileset)
        {
            foreach (KeyValuePair<bool, Dictionary<int, Dictionary<int, EventSpriteVertices>>> entry in Vertices)
            {
                foreach (KeyValuePair<int, Dictionary<int, EventSpriteVertices>> entry2 in entry.Value)
                {
                    foreach (KeyValuePair<int, EventSpriteVertices> entry3 in entry2.Value)
                    {
                        int[] newTexture = isTileset ? InitialTexture : new int[] { InitialTexture[0] + (entry.Key ? texture2D.Width / 2 : 0) + (entry2.Key * InitialTexture[2]), InitialTexture[1] + (entry3.Key * InitialTexture[3]), InitialTexture[2] , InitialTexture[3] };
                        entry3.Value.CreatePortion(device, texture2D, newTexture, isTileset);
                    }
                }
            }
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GraphicsDevice device, Camera camera, AlphaTestEffect effect, Orientation orientationMap, SystemGraphic characterGraphic)
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
                effect.Texture = Game1.TexCharacters[characterGraphic];
                if (Act) 
                {
                    position = Position;
                }
                else
                {
                    position = new Vector3(Position.X, Position.Y + bounce, Position.Z);
                }
            }

            int frame = Act ? FrameInactive : Frame;
            int orientation = WANOK.Mod(((int)orientationMap - 2) * 3 + (int)OrientationEye, 4);
            Sprite.Draw(device, effect, Vertices[Act][frame][orientation].VerticesArray, Vertices[Act][frame][orientation].IndexesArray, camera, position, InitialTexture[2], InitialTexture[3]);
        }

        // -------------------------------------------------------------------
        // DisposeBuffers
        // -------------------------------------------------------------------

        public void DisposeBuffers(GraphicsDevice device, bool nullable = true)
        {
            foreach (Dictionary<int, Dictionary<int, EventSpriteVertices>> dic in Vertices.Values)
            {
                foreach (Dictionary<int, EventSpriteVertices> dic2 in dic.Values)
                {
                    foreach (EventSpriteVertices vertices in dic2.Values)
                    {
                        vertices.DisposeBuffers(device, nullable);
                    }
                }
            }
        }
    }
}
