﻿using IPlayable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIACastellaRieben
{
    /// <summary>
    /// Class that represent a game state (a node in the tree)
    /// </summary>
    class TreeNode
    {
        private int value;
        private TreeNode parent;
        private Board board;

        public TreeNode(int[,] board, TreeNode parent)
        {
            this.board = new Board(board);
            this.parent = parent;
            this.value = Eval();
        }

        /// <summary>
        /// Evaluation function
        /// </summary>
        public int Eval()
        {
            //eval function idea and part of code : https://kartikkukreja.wordpress.com/2013/03/30/heuristic-function-for-reversiothello/
            double coinDiff;
            double mobility;
            double tileWeight;

            double result;

            //this array indicate the weight for each tile of the board
            int[,] tileWeightArray = new int[8, 8]{ { 40,-10,  0,  0,  0,  0,-10, 40},
                                                    {-10,-20,  0,  0,  0,  0,-20,-10},
                                                    {  0,  0,  0,  0,  0,  0,  0,  0},
                                                     {  0,  0,  0,  0,  0,  0,  0,  0},
                                                     {  0,  0,  0,  0,  0,  0,  0,  0},
                                                     {  0,  0,  0,  0,  0,  0,  0,  0},
                                                     {-10,-20,  0,  0,  0,  0,-20,-10},
                                                     { 40,-10,  0,  0,  0,  0,-10, 40}
            };

            int whiteScore = board.GetWhiteScore();
            int blackScore = board.GetBlackScore();

            int whitePossibilities = Ops(true).Count;
            int blackPossibilites = Ops(false).Count;

            int whiteValue = getValue(true, tileWeightArray);
            int blackValue = getValue(false, tileWeightArray);

            //evaluate each parameters and return a value between -100 and 100  
            coinDiff = 100 * (whiteScore - blackScore) / (whiteScore + blackScore);
            if (whitePossibilities + blackPossibilites != 0)
            {
                mobility = 100 * (whitePossibilities - blackPossibilites) / (whitePossibilities + blackPossibilites);
            }
            else
            {
                mobility = 0;
            }
            if (whiteValue + blackValue != 0)
            {
                tileWeight = 100 * (whiteValue - blackValue) / (whiteValue + blackValue);
            }
            else
            {
                tileWeight = 0;
            }

            //weight for each parameters. weight may be changed for better result
            //we try to aim for best tile until late game where we try to have more disc than the opponent
            if (blackScore + whiteScore > 55)
            {
                result = (coinDiff * 20 + mobility * 4 + tileWeight * 2);
            }
            else
            {
                result = (coinDiff * 2 + mobility * 4 + tileWeight * 20);
            }

            return (int)Math.Ceiling(result);
        }

        private int getValue(bool white, int[,] tileWeightArray)
        {
            int count = 0;
            int myColor = white ? 0 : 1;

            //compare each tiles with it's weight defined in our weight array
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (board.GetBoard()[i, j] == myColor)
                    {
                        count += tileWeightArray[i, j];
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Is the game is in final state
        /// </summary>
        /// <returns>True if the game is over</returns>
        public bool Final(bool whiteTurn)
        {
            //check if we have possibilities to play
            if (Ops(whiteTurn).Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the applicable operators
        /// </summary>
        /// <returns>List of operators</returns>
        public List<Tuple<int, int>> Ops(bool iswhite)
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (board.IsPlayable(i, j, iswhite))
                    {
                        list.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Apply opperator on this node
        /// </summary>
        /// <returns></returns>
        public TreeNode Apply(Tuple<int, int> op, bool whiteTurn)
        {
            return new TreeNode(board.FakePlayMove(op.Item1, op.Item2, whiteTurn), this);
        }
    }
}
