using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace RPG_Paper_Maker
{
    [Serializable]
    class Mountains
    {
        public Dictionary<int, MountainsGroup> Groups = new Dictionary<int, MountainsGroup>();


        // -------------------------------------------------------------------
        // GenMountains
        // -------------------------------------------------------------------

        public void GenMountains(GraphicsDevice device)
        {
            foreach (int id in Groups.Keys)
            {
                Groups[id].GenMountains(device, id);
            }
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GraphicsDevice device, AlphaTestEffect effect)
        {
            foreach (int id in Groups.Keys)
            {
                Groups[id].Draw(device, effect, id);
            }
        }

        // -------------------------------------------------------------------
        // DisposeBuffers
        // -------------------------------------------------------------------

        public void DisposeBuffers(GraphicsDevice device, bool nullable = true)
        {
            foreach (int id in Groups.Keys)
            {
                Groups[id].DisposeBuffers(device, nullable);
            }
        }
    }
}