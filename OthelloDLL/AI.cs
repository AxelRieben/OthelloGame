using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIACastellaRieben
{
    class AI
    {
        private Board board;

        public AI(Board board)
        {
            this.board = board;
        }

        /// <summary>
        /// Alpha-beta algorithm
        /// </summary>
        /// <param name="root"></param>
        /// <param name="depth"></param>
        /// <param name="minOrMax"></param>
        /// <param name="parentValue"></param>
        /// <param name="whiteTurn"></param>
        /// <returns></returns>
        public Tuple<int, Tuple<int, int>> alphabeta2(TreeNode root, int depth, int minOrMax, int parentValue, bool whiteTurn)
        {
            // minOrMax = 1 : maximize
            // minOrMax = -1 : minimize
            if (depth == 0 || root.Final(whiteTurn))
            {
                return new Tuple<int, Tuple<int, int>>(root.Eval(), null);
            }

            int optVal = minOrMax * (-int.MaxValue);
            Tuple<int, int> optOp = null;

            foreach (Tuple<int, int> op in root.Ops(whiteTurn))
            {
                TreeNode newNode = root.Apply(op, whiteTurn);
                Tuple<int, Tuple<int, int>> newAlpha = alphabeta2(newNode, depth - 1, -minOrMax, optVal, !whiteTurn);

                if (newAlpha.Item1 * minOrMax > optVal * minOrMax)
                {
                    optVal = newAlpha.Item1;
                    optOp = op;

                    if (optVal * minOrMax > parentValue * minOrMax)
                    {
                        break;
                    }
                }
            }

            return new Tuple<int, Tuple<int, int>>(optVal, optOp);
        }
    }
}
