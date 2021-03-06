﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;
using OthelloIACastellaRieben;

namespace OthelloIACastellaRieben
{
    public class Board : IPlayable.IPlayable
    {
        //Board that represent the game
        private int[,] board;

        //The two players
        private Player playerBlack;
        private Player playerWhite;

        //Our implementation of the alpha-beta algorithm
        private AI ai;

        public Board()
        {
            ai = new AI(this);
            initGame();
        }

        public Board(int[,] board)
        {
            //Clone a new board
            this.board = (int[,])board.Clone();
            playerWhite = new Player("White", 0);
            playerBlack = new Player("Black", 1);

            //set score for both player from board
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (board[i, j] == 0)
                    {
                        playerWhite.Score++;
                    }
                    else if (board[i, j] == 1)
                    {
                        playerBlack.Score++;
                    }
                }
            }
        }

        public void initGame()
        {
            //set an empty board
            board = new int[8, 8];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = -1;
                }
            }

            //add starting disc
            board[3, 3] = 0;
            board[4, 3] = 1;
            board[3, 4] = 1;
            board[4, 4] = 0;

            //create 2 player
            playerWhite = new Player("White", 0);
            playerBlack = new Player("Black", 1);
        }

        public int GetBlackScore()
        {
            return playerBlack.Score;
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public string GetName()
        {
            return "Rieben Axel & Castella Killian";
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            TreeNode root = new TreeNode(game, null);
            AI ai = new AI(this);
            //get the best move from our aplphabeta function
            return ai.alphabeta2(root, level, 1, int.MaxValue, whiteTurn).Item2;
        }

        public int GetWhiteScore()
        {
            return playerWhite.Score;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return IsPlayableFlipOption(column, line, isWhite, false);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            return IsPlayableFlipOption(column, line, isWhite, true);
        }

        private bool IsPlayableFlipOption(int column, int line, bool isWhite, bool flipCatchedTile)
        {
            int myColor = isWhite ? 0 : 1;
            int opponentColor = isWhite ? 1 : 0;
            //check if tile is in the board
            if (!CheckIfTileExist(column, line, 0, 0))
            {
                return false;
            }
            //check if there is no disc here.
            if (board[column, line] != -1)
            {
                return false;
            }
            //then we look for opponent piece in the v8 neighbourhood , this will tell us the possible direction
            List<Tuple<int, int>> vulnerableNeighbour = getDirection(column, line, opponentColor);
            if (!flipCatchedTile)
            {
                //check if neighbour can be taken
                foreach (Tuple<int, int> a in vulnerableNeighbour)
                {
                    if (CheckDirection(column, line, a.Item1, a.Item2, myColor))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                int nbTilesFliped = 0;
                foreach (Tuple<int, int> a in vulnerableNeighbour)
                {
                    nbTilesFliped += FlipDirection(column, line, a.Item1, a.Item2, myColor);
                }
                //if our move do not flip any tile , move is not legal
                if (nbTilesFliped == 0)
                {
                    return false;
                }
                else
                {
                    //update score
                    if (isWhite)
                    {
                        playerWhite.Score = playerWhite.Score + nbTilesFliped + 1;
                        playerBlack.Score = playerBlack.Score - nbTilesFliped;
                    }
                    else
                    {
                        playerBlack.Score = playerBlack.Score + nbTilesFliped + 1;
                        playerWhite.Score = playerWhite.Score - nbTilesFliped;
                    }
                    return true;
                }

            }
        }

        private bool CheckIfTileExist(int column, int line, int xMove, int yMove)
        {
            return (column + xMove <= 7 && line + yMove <= 7 && column + xMove >= 0 && line + yMove >= 0);
        }

        private List<Tuple<int, int>> getDirection(int column, int line, int opponentColor)
        {
            List<Tuple<int, int>> vulnerableNeighbour = new List<Tuple<int, int>>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i != 0) || (j != 0))
                    {
                        //check if direction is valid
                        if (CheckIfTileExist(column, line, i, j))
                        {
                            //check if there is an opponent disc there
                            if (board[column + i, line + j] == opponentColor)
                            {
                                // if theres is , we add the tile to the vulnerable neighbour list
                                Tuple<int, int> possibilities = new Tuple<int, int>(i, j);
                                vulnerableNeighbour.Add(possibilities);
                            }
                        }
                    }
                }
            }
            return vulnerableNeighbour;
        }

        private bool CheckDirection(int column, int line, int xMove, int yMove, int myColor)
        {
            //check the direction | maximal distance = 6
            for (int i = 2; i <= 6; i++)
            {
                if (CheckIfTileExist(column, line, xMove * i, yMove * i))
                {
                    int tileValue = board[column + xMove * i, line + yMove * i];
                    //if tile is empty it's not possible to place my disc
                    if (tileValue == -1)
                    {
                        return false;
                    }
                    //if tile has already a disc with my color the move is legal
                    else if (tileValue == myColor)
                    {
                        return true;
                    }
                    //if tile has already a disc with the color of my opponent i can maybe place my disc later
                    else if (tileValue == 1 - myColor)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                //if tile does not exist
                else
                {
                    return false;
                }
            }
            //if we tried every tile and we couldn't place our disc.
            return false;
        }

        private int FlipDirection(int column, int line, int xMove, int yMove, int myColor)
        {
            int flipped = 1;
            for (int i = 2; i <= 7; i++)
            {
                if (CheckIfTileExist(column, line, xMove * i, yMove * i))
                {
                    int tileValue = board[column + xMove * i, line + yMove * i];
                    //if tile is empty it's not possible to place my disc
                    if (tileValue == -1)
                    {
                        return 0;
                    }
                    //if tile has already a disc with my color the move is legal
                    else if (tileValue == myColor)
                    {
                        flipCoinInDirection(column, line, xMove, yMove, column + xMove * i, line + yMove * i, myColor);
                        return flipped;
                    }
                    //if tile has already a disc with the color of my opponent i can maybe place my disc later
                    else if (tileValue == 1 - myColor)
                    {
                        flipped++;
                    }
                    else
                    {
                        return 0;
                    }
                }
                //if tile does not exist
                else
                {
                    return 0;
                }
            }
            //if we tried every tile and we couldn't place our disc.
            return 0;
        }

        private void flipCoinInDirection(int column, int line, int xMove, int yMove, int endColumn, int endLine, int myColor)
        {
            int currentColumn = column;
            int currentLine = line;
            // we flip every coin from start tile to end tile in direction(xMove,yMove)
            while (currentColumn != endColumn || currentLine != endLine)
            {
                board[currentColumn, currentLine] = myColor;
                currentColumn = currentColumn + xMove;
                currentLine = currentLine + yMove;
            }
        }

        public int[,] FakePlayMove(int column, int line, bool whiteTurn)
        {
            //return the board the play would get without chnanging it.
            int[,] oldBoard = GetBoard();
            PlayMove(column, line, whiteTurn);
            int[,] newBoard = GetBoard();
            board = oldBoard;
            return newBoard;
        }
    }
}
