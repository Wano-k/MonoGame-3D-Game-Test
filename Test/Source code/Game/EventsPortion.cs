using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Paper_Maker;
using Microsoft.Xna.Framework;

namespace Test
{
    class EventsPortion
    {
        public Dictionary<int[], EventSprite> Sprites = new Dictionary<int[], EventSprite>(new IntArrayComparer());


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public EventsPortion(Dictionary<int[], SystemEvent> dictionary)
        {
            if (dictionary != null)
            {
                foreach (KeyValuePair<int[], SystemEvent> entry in dictionary)
                {
                    AddSprite(entry.Key, entry.Value);
                }
            }
        }

        // -------------------------------------------------------------------
        // IsEmpty
        // -------------------------------------------------------------------

        public bool IsEmpty()
        {
            return (Sprites.Count == 0);
        }

        // -------------------------------------------------------------------
        // AddSprite
        // -------------------------------------------------------------------

        public void AddSprite(int[] coords, SystemEvent ev)
        {
            int[] texture;
            int frames = 1;
            if (ev.GetCurrentPage().Graphic.IsTileset())
            {
                texture = new int[] { (int)ev.Pages[0].Graphic.Options[(int)SystemGraphic.OptionsEnum.TilesetX],
                                            (int)ev.Pages[0].Graphic.Options[(int)SystemGraphic.OptionsEnum.TilesetY],
                                            (int)ev.Pages[0].Graphic.Options[(int)SystemGraphic.OptionsEnum.TilesetWidth],
                                            (int)ev.Pages[0].Graphic.Options[(int)SystemGraphic.OptionsEnum.TilesetHeight] };
            }
            else
            {
                frames = (int)ev.GetCurrentPage().Graphic.Options[(int)SystemGraphic.OptionsEnum.Frames];
                int index = (int)ev.GetCurrentPage().Graphic.Options[(int)SystemGraphic.OptionsEnum.Index];
                int width = Game1.TexCharacters[ev.GetCurrentPage().Graphic].Width / 2 / frames;
                int height = Game1.TexCharacters[ev.Pages[0].Graphic].Height / ((int)ev.GetCurrentPage().Graphic.Options[(int)SystemGraphic.OptionsEnum.Diagonal] == 0 ? 4 : 8);
                texture = new int[] { (index % frames) * width, (index / frames) * height, width, height };
            }

            Sprites[coords] = new EventSprite(WANOK.GetVector3Position(coords), new Vector2(texture[2], 16), frames, new Sprite(ev.GetCurrentPage().GraphicDrawType, new int[] { 0, 0 }, 0), texture);
        }

        // -------------------------------------------------------------------
        // RemoveSprite
        // -------------------------------------------------------------------

        public void RemoveSprite(GraphicsDevice device, int[] coords)
        {
            Sprites[coords].DisposeBuffers(device);
            Sprites.Remove(coords);
        }

        // -------------------------------------------------------------------
        // GenEvents
        // -------------------------------------------------------------------

        public void GenEvents(GraphicsDevice device, Dictionary<int[], SystemEvent> dictionary)
        {
            if (Sprites.Count > 0)
            {
                foreach (int[] coords in Sprites.Keys)
                {
                    GenEvent(device, coords, dictionary[coords]);
                }
            }
        }

        public void GenEvent(GraphicsDevice device, int[] coords, SystemEvent ev)
        {
            Sprites[coords].CreatePortion(device, ev.GetCurrentPage().Graphic.IsTileset() ? Game1.TexTileset : Game1.TexCharacters[ev.GetCurrentPage().Graphic], ev.GetCurrentPage().Graphic.IsTileset());
        }

        // -------------------------------------------------------------------
        // DrawSprites
        // -------------------------------------------------------------------

        public void DrawSprites(GraphicsDevice device, AlphaTestEffect effect, Camera camera, Orientation orientationMap, Dictionary<int[], SystemEvent> events)
        {
            foreach (KeyValuePair<int[], EventSprite> entry in Sprites)
            {
                entry.Value.Draw(device, camera, effect, orientationMap, events[entry.Key].GetCurrentPage().Graphic.IsTileset() ? null : events[entry.Key].GetCurrentPage().Graphic);
            }
        }

        // -------------------------------------------------------------------
        // DisposeBuffers
        // -------------------------------------------------------------------

        public void DisposeBuffers(GraphicsDevice device, bool nullable = true)
        {
            foreach (EventSprite eventSprite in Sprites.Values)
            {
                eventSprite.DisposeBuffers(device, nullable);
            }
        }
    }
}
