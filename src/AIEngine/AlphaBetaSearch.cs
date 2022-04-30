using System;
using System.Linq;

namespace AIEngine
{
    public static class AlphaBetaSearch
    {

        public static Func<INode, int> HeuristicFunction = null;



        public static int Search(INode node, int depth, int alpha, int beta, bool Max)
        {
            int score = 0;
            if (depth == 0)
            {
                score = HeuristicFunction(node);
                //node.SetValue(score);
                return score;
            }

            if (Max)
            {
                foreach (INode child in node.GetSccessors())
                {
                    alpha = Math.Max(alpha, Search(child, depth - 1, alpha, beta, !Max));
                    if (beta < alpha)
                    {
                        break;
                    }

                }
                node.SetValue(alpha);
                return alpha;
            }
            else
            {
                foreach (INode child in node.GetSccessors())
                {
                    beta = Math.Min(beta, Search(child, depth - 1, alpha, beta, !Max));

                    if (beta < alpha)
                    {
                        break;
                    }
                }
                node.SetValue(beta);
                return beta;
            }
        }

        public static INode GetAlphaBetaMax(INode n)
        {
            return n.GetSccessors().OrderByDescending((m) => m.GetValue()).ElementAt(0);
        }




    }

}
