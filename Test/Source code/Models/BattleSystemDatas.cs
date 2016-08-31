using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class BattleSystemDatas
    {
        public static int MAX_WEAPONS_KIND = 9999;
        public static int MAX_ARMORS_KIND = 9999;

        // ListBoxes
        public List<SystemElement> Elements = new List<SystemElement>();
        public List<SystemStatistics> Statistics = new List<SystemStatistics>();
        public List<SuperListItemName> WeaponsKind = new List<SuperListItemName>();
        public List<SuperListItemName> ArmorsKind = new List<SuperListItemName>();


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public BattleSystemDatas()
        {

        }
    }
}
