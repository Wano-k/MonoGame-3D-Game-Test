using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class SystemElement : SuperListItem
    {
        public static int MAX_ELEMENTS = 9999;
        public Dictionary<string, string> Names;
        public SystemGraphic Icon;


        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public override SuperListItem CreateCopy()
        {
            return new SystemElement();
        }
    }
}
