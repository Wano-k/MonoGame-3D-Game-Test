using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class SystemEvent
    {
        public string Name;
        public List<SystemEventPage> Pages;

        [NonSerialized()]
        public int CurrentPage = 0;

        // -------------------------------------------------------------------
        // SYSTEM EVENT PAGE
        // -------------------------------------------------------------------

        #region Event page 

        [Serializable]
        public class SystemEventPage
        {
            public SystemGraphic Graphic;
            public DrawType GraphicDrawType;
            public EventTrigger Trigger;
            public EventCommandConditions ConditionsTree;
            public NTree<EventCommand> CommandsTree;

            [Serializable]
            public class PageOptions
            {
                public bool MoveAnimation;
                public SystemGraphic StopAnimation;
                public bool DirectionFix;
                public bool Through;
                public bool SetWithCamera;


                public PageOptions CreateCopy()
                {
                    return new PageOptions();
                }
            }

            public PageOptions Options;


            // -------------------------------------------------------------------
            // CreateCopy
            // -------------------------------------------------------------------

            public SystemEventPage CreateCopy()
            {
                return new SystemEventPage();
            }
        }

        #endregion


        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public SystemEvent CreateCopy()
        {
            return new SystemEvent();
        }

        // -------------------------------------------------------------------
        // GetCurrentPage
        // -------------------------------------------------------------------

        public SystemEventPage GetCurrentPage()
        {
            return Pages[CurrentPage];
        }
    }
}
