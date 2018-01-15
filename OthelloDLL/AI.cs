using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIACastellaRieben
{
    class AI
    {
        public AI()
        {

        }
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

        /*public static Tuple<int, TreeNode<int>> AlphaBeta(TreeNode<int> root, int depth)
        {
            Tuple<int, TreeNode<int>> result = Max(root, int.MaxValue, depth);
            return result;
        }

        /// <summary>
        /// Maximize the evaluation function with the alpha-beta optimisation
        /// </summary>
        /// <param name="root"></param>
        /// <param name="parentMin"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static Tuple<int, TreeNode<int>> Max(TreeNode<int> root, int parentMin, int depth)
        {
            if (depth == 0 || root.Final())
            {
                return Tuple.Create<int, TreeNode<int>>(root.Eval(), null);
            }

            int maxVal = int.MinValue;
            TreeNode<int> maxOp = null;

            foreach (Tuple<int,int> op in root.Ops())
            {
                TreeNode<int> newNode = root.Apply(op);
                Tuple<int, TreeNode<int>> tuple = Min(newNode, maxVal, depth - 1);
                int val = tuple.Item1;

                if (val > maxVal)
                {
                    maxVal = val;
                    maxOp = op;

                    if (maxVal > parentMin)
                    {
                        break;
                    }
                }
            }

            return Tuple.Create(maxVal, maxOp);
        }

        /// <summary>
        /// Minimize the evaluation function with the alpha-beta optimisation
        /// </summary>
        /// <param name="root"></param>
        /// <param name="parentMax"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static Tuple<int, TreeNode<int>> Min(TreeNode<int> root, int parentMax, int depth)
        {
            if (depth == 0 || root.Final())
            {
                return Tuple.Create<int, TreeNode<int>>(root.Eval(), null);
            }

            int minVal = int.MinValue;
            TreeNode<int> minOp = null;

            foreach (TreeNode<int> op in root.Ops())
            {
                TreeNode<int> newNode = root.Apply(op);
                Tuple<int, TreeNode<int>> tuple = Max(newNode, minVal, depth - 1);
                int val = tuple.Item1;

                if (val < minVal)
                {
                    minVal = val;
                    minOp = op;

                    if (minVal < parentMax)
                    {
                        break;
                    }
                }
            }

            return Tuple.Create(minVal, minOp);
        }*/
    }
}
