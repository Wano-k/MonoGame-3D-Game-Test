using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public abstract class SuperListItem
    {
        public int Id;
        public string Name = "";

        public abstract SuperListItem CreateCopy();
    }

    [Serializable]
    public abstract class ComboxBoxSpecialTilesetItem : SuperListItem
    {
        public SystemGraphic Graphic;
    }
}
