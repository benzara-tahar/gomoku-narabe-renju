using System;
using System.Linq;

namespace AIEngine
{
    public static class MinMaxSearch
    {
        public static Func<INode, int> HeuristicFunction = null;


        public static int Search(INode node, bool mode, int depth)
        {
            int ret = 0;
            if (depth == 0 || node.IsTerminal())
            {
                ret = HeuristicFunction(node);
                node.SetValue(ret);
                //Console.WriteLine("Noud Terminal: " + ret);
                //node.ToString();
                return ret;
            }

            if (mode)
                foreach (var n in node.GetSccessors())
                {
                    ret = Math.Max(-9999999, Search(n, !mode, depth - 1));
                }
            else
                foreach (var n in node.GetSccessors())
                {
                    ret = Math.Min(+9999999, Search(n, mode, depth - 1));
                }
            node.SetValue(ret);
            return ret;
        }

        public static INode MaxMove(INode node)
        {

            foreach (var n in node.GetSccessors())
            {
                Console.WriteLine(n.GetValue().ToString());
            }
            return node.GetSccessors().OrderByDescending((n) => n.GetValue()).ElementAt(0);
        }
    }
}
