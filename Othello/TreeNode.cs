using IPlayable;
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
        private int[,] board;

        /// <summary>
        /// Evaluation function
        /// </summary>
        public int Eval()
        {
            //eval function idea and part of code : https://kartikkukreja.wordpress.com/2013/03/30/heuristic-function-for-reversiothello/
            double coinDiff;
            double mobility;
            double corners;
            double xSquare;
            double cSquare;
            double stability;

            Board gameBoard = new Board();

            int[,] board = gameBoard.GetBoard();
            int whiteScore = gameBoard.GetWhiteScore();
            int blackScore = gameBoard.GetBlackScore();

            int whitePossibilities = getPossibilities(board,true,gameBoard);
            int blackPossibilites = getPossibilities(board,false,gameBoard);

            int whiteCorners = 0;
            int BlackCorners = 0;
            getCorners(ref whiteCorners, ref BlackCorners,board);

            coinDiff = 100 * (whiteScore - blackScore) / (whiteScore + blackScore);
            if (whitePossibilities + blackPossibilites != 0)
            {
                mobility = 100 * (whitePossibilities - blackPossibilites) / (whitePossibilities + blackPossibilites);
            }
            else
            {
                mobility = 0;
            }
            if (whiteCorners + BlackCorners != 0)
            {
                corners = 100 * (whiteCorners -BlackCorners) / (whiteCorners + BlackCorners);
            }
            else
            {
                corners = 0;
            }
            int whiteStability=getStability(board);
            int blackStability=getStability(board);
            

            if (whiteStability + blackStability != 0)
            {
                stability = 100 * (whiteStability - blackStability) / (whiteStability + blackStability);
            }
            else
            {
                stability = 0;
            }
            return 0;
        }

        private int getStability(int[,] board)
        {
            int totalStability=0;
            int caseStability; 
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; i < 8; j++)
                {
                    //caseStability = ;
                    //totalStability += caseStability;
                }
            }
            return totalStability;
        }

        private void getCorners(ref int whiteCorners, ref int blackCorners, int[,] board)
        {

            Tuple<int, int>[] cases ={
                new Tuple<int, int>(0, 0),
                new Tuple<int, int>(0, 7),
                new Tuple<int, int>(7, 0),
                new Tuple<int, int>(7, 7),
            };
            foreach (Tuple<int, int> p in cases)
            {
                
            switch(board[p.Item1, p.Item2])
                {
                    case 0:
                        whiteCorners++;
                        break;
                    case 1:
                        blackCorners++;
                        break;
                    default:
                        break;
                }
                
            }
        }

        private int getPossibilities(int[,] board,bool isWhite,Board gameboard)
        {
            int count = 0;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; i < 8; j++)
                {
                    if(gameboard.IsPlayable(i, j,isWhite))
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
        public List<Tuple<int, int>> Ops()
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (IsPlayable(i, j, false))
                    {
                        list.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
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
