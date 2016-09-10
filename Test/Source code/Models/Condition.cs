using RPG_Paper_Maker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public static class Condition
    {

        // -------------------------------------------------------------------
        // ToBool
        // -------------------------------------------------------------------

        public static bool ToBool(List<object> condition)
        {
            return true;
        }

        // -------------------------------------------------------------------
        // ToBoolMultiple
        // -------------------------------------------------------------------

        public static bool ToBoolMultiples(NTree<List<object>> conditionsTree)
        {
            if (conditionsTree.Children.Count > 0)
            {
                bool test;

                if ((string)conditionsTree.Data[0] == "") return true;
                
                else if ((string)conditionsTree.Data[0] == "And")
                {
                    test = true;
                    foreach (NTree<List<object>> node in conditionsTree.Children)
                    {
                        test &= ToBoolMultiples(node);
                        if (!test) return false;
                    }
                    return test;
                }
                else if ((string)conditionsTree.Data[0] == "Or")
                {
                    test = false;
                    foreach (NTree<List<object>> node in conditionsTree.Children)
                    {
                        test |= ToBoolMultiples(node);
                        if (test) return true;
                    }
                    return test;
                }
            }
            else
            {
                return ToBool(conditionsTree.Data);
            }

            return false;
        }
    }
}
