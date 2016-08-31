using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public abstract class EventCommand
    {
        public EventCommandKind EventCommandKind;

        public abstract EventCommand CreateCopy();
    }

    [Serializable]
    public class EventCommandConditions : EventCommand
    {
        public NTree<List<object>> Tree;


        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public override EventCommand CreateCopy()
        {
            return new EventCommandConditions();
        }
    }

    [Serializable]
    public class EventCommandOther : EventCommand
    {
        public List<object> Command;


        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public override EventCommand CreateCopy()
        {
            return new EventCommandOther();
        }
    }
}
