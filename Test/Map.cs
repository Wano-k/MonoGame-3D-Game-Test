using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Test
{
    class Map
    {
        GraphicsDevice device;
        List<VertexPositionTexture> verticesList;
        List<int> indexesList;
        VertexPositionTexture[] verticesArray;
        int[] indexesArray;
        VertexBuffer vb;
        IndexBuffer ib;
        int[] indexes;
        public int[] Size = new int[2];


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Map(GraphicsDevice device, string mapName)
        {
            this.device = device;

            // Size
            Size[0] = (int)(WANOK.PORTION_RADIUS * WANOK.SQUARESIZE);
            Size[1] = (int)(WANOK.PORTION_RADIUS * WANOK.SQUARESIZE);

            // Building vertex buffer indexed
            this.verticesList = new List<VertexPositionTexture>();
            this.indexesList = new List<int>();
            this.indexes = new int[]
            {
                0, 1, 2, 0, 2, 3
            };
            int offset = 0, k = 0;
            Game_map_portion portion;
            for (int i = 0; i < WANOK.PORTION_RADIUS; i++)
            {
                for (int j = 0; j < WANOK.PORTION_RADIUS; j++)
                {
                    // Reading each portions
                    FileStream fs = new FileStream("Content/Datas/Maps/MAP0001/" + i + "-" + j + ".JSON", FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    string json = sr.ReadToEnd();
                    portion = JsonConvert.DeserializeObject<Game_map_portion>(json);

                    for (int l = 0; l < portion.texture_floors.Count; l++)
                    {
                        for (int m = 0; m < portion.floors[l].Count; m++)
                        {
                            foreach (VertexPositionTexture vertex in CreateFloorWithTex(portion.floors[l][m][0], portion.floors[l][m][1], portion.texture_floors[l]))
                            {
                                this.verticesList.Add(vertex);
                            }
                            for (int n = 0; n < 6; n++)
                            {
                                this.indexesList.Add(indexes[n] + offset);
                            }
                            offset += 4;
                        }
                    }
                    k++;
                }
            }
            this.verticesArray = this.verticesList.ToArray();
            this.indexesArray = this.indexesList.ToArray();
            this.ib = new IndexBuffer(this.device, IndexElementSize.ThirtyTwoBits, this.indexesArray.Length, BufferUsage.None);
            this.ib.SetData(this.indexesArray);
            vb = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, this.verticesArray.Length, BufferUsage.None);
            vb.SetData(this.verticesArray);
        }

        // -------------------------------------------------------------------
        // CreateFloorWithTex : coords = [x,y,width,height]
        // -------------------------------------------------------------------

        protected VertexPositionTexture[] CreateFloorWithTex(int x, int z, int[] coords)
        {
            // Texture coords
            float left = ((float)coords[0]) / Game1.currentFloorTex.Width;
            float top = ((float)coords[1]) / Game1.currentFloorTex.Height;
            float bot = ((float)(coords[1]+coords[3])) / Game1.currentFloorTex.Height;
            float right = ((float)(coords[0] + coords[2])) / Game1.currentFloorTex.Width;

            // Adjust in order to limit risk of textures flood
            float width = left + right;
            float height = top + bot;
            int coef = 10000;
            left += width / coef;
            right -= width / coef;
            top += height / coef;
            bot -= height / coef;

            // Vertex Position and Texture
            return new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(x, 0, z), new Vector2(left, top)),
                new VertexPositionTexture(new Vector3(x+1, 0, z), new Vector2(right, top)),
                new VertexPositionTexture(new Vector3(x+1, 0, z+1), new Vector2(right, bot)),
                new VertexPositionTexture(new Vector3(x, 0, z+1), new Vector2(left, bot))
            };
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GameTime gameTime, BasicEffect effect)
        {
            // Effect settings
            effect.Texture = Game1.currentFloorTex;
            effect.World = Matrix.Identity * Matrix.CreateScale(WANOK.SQUARESIZE, 1.0f, WANOK.SQUARESIZE);

            // Drawing
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, this.verticesArray, 0, this.verticesArray.Length, this.indexesArray, 0, this.verticesArray.Length / 2);
            }
        }
    }
}
