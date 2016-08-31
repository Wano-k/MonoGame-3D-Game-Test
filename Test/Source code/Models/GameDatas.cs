using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace RPG_Paper_Maker
{
    public class GameDatas
    {
        public HeroesDatas Heroes;
        public SystemDatas System;
        public BattleSystemDatas BattleSystem;
        public TilesetsDatas Tilesets;


        // -------------------------------------------------------------------
        // LoadDatas
        // -------------------------------------------------------------------

        public void LoadDatas()
        {
            Heroes = WANOK.LoadBinaryDatas<HeroesDatas>(WANOK.HeroesPath);
            if (Heroes == null) WANOK.PrintError("Heroes.rpmd version is not compatible.");
            BattleSystem = WANOK.LoadBinaryDatas<BattleSystemDatas>(WANOK.BattleSystemPath);
            if (BattleSystem == null) WANOK.PrintError("BattleSystem.rpmd version is not compatible.");
            System = WANOK.LoadBinaryDatas<SystemDatas>(WANOK.SystemPath);
            if (System == null) WANOK.PrintError("System.rpmd version is not compatible.");
            Tilesets = WANOK.LoadBinaryDatas<TilesetsDatas>(WANOK.TilesetsPath);
            if (Tilesets == null) WANOK.PrintError("Tilesets.rpmd version is not compatible.");
        }

        // -------------------------------------------------------------------
        // SaveDatas
        // -------------------------------------------------------------------

        public void SaveDatas()
        {
            WANOK.SaveBinaryDatas(Heroes, WANOK.HeroesPath);
            WANOK.SaveBinaryDatas(BattleSystem, WANOK.BattleSystemPath);
            WANOK.SaveBinaryDatas(System, WANOK.SystemPath);
            WANOK.SaveBinaryDatas(Tilesets, WANOK.TilesetsPath);
        }
    }
}