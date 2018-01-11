using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Othello
{
    class TreeNode<T>
    {
        private T value;
        private TreeNode<T> parent;

        /// <summary>
        /// Evaluation function
        /// </summary>
        public int Eval()
        {
            //eval function idea and part of code : https://kartikkukreja.wordpress.com/2013/03/30/heuristic-function-for-reversiothello/
            double coinDiff;
            double mobility;
            double corners;
            double stability;

            int[,] board = board.GetBoard();
            int whiteScore = board.GetWhiteScore();
            int blackScore = board.GetBlackScore();

            int whitePossibilities = getPossibilities(board);
            int blackPossibilites = getPossibilities(board);

            int whiteCorners = 0;
            int BlackCorners = 0;
            getCorners(ref whiteCorners, ref BlackCorners);
            coinDiff = 100 * (whiteScore - blackScore) / (whiteScore + blackScore);
            if (whitePossibilities + blackPossibilites != 0)
            {
                mobility = 100 * (whitePossibilities - blackPossibilites) / (whitePossibilities + blackPossibilites);
            }
            else
            {
                mobility = 0;
            }
                
            return 0;
        }

        private void getCorners(ref int whiteCorners, ref int blackCorners)
        {
            Point[] cases ={
                new Point(0, 0),
                new Point(7, 0),
                new Point(0, 7),
                new Point(7, 7),
            };
            foreach(Point p in cases)
            {

            }
        }

        private int getPossibilities(int[,] board)
        {
            int count = 0;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; i < 8; j++)
                {
                    if(IPlayable().isPlayable(i, j))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Is the game in final state
        /// </summary>
        /// <returns>True if the game is over</returns>
        public bool Final()
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Returns the applicable operators
        /// </summary>
        /// <returns>List of operators</returns>
        public List<TreeNode<T>> Ops()
        {
            //TODO
            return null;
        }

        /// <summary>
        /// Apply opperator on this node
        /// </summary>
        /// <returns></returns>
        public TreeNode<T> Apply(TreeNode<T> op)
        {
            //TODO
            return null;
        }
    }
}
