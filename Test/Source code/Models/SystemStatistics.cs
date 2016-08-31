using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class SystemStatistics : SuperListItem
    {
        public static int MAX_STATISTICS = 999;
        public Dictionary<string, string> Names;
        public SystemGraphic Bar;

        #region Game over 

        public GameOverOptions AllGameOverOptions;
        [Serializable]
        public class GameOverOptions
        {
            public bool NoImplication;
            public bool AllHeroes;
            public List<int> HeroesSelected;
            public Comparaison Comparaison;
            public int Value;
            public Measure Measure;


            // -------------------------------------------------------------------
            // CreateCopy
            // -------------------------------------------------------------------

            public GameOverOptions CreateCopy()
            {
                return new GameOverOptions();
            }
        }

        #endregion

        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public override SuperListItem CreateCopy()
        {
            return new SystemStatistics();
        }
    }
}
