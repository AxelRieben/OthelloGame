using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class AI
    {

        public static Tuple<int, TreeNode<int>> AlphaBeta(TreeNode<int> root, int depth)
        {
            Tuple<int, TreeNode<int>> result = Max(root, depth);
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

            foreach (TreeNode<int> op in root.Ops())
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
        }
    }
}
