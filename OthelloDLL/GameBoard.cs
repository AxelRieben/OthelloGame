using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;
using OthelloDLL;

namespace IPlayable
{
    public class GameBoard : IPlayable
    {
        private int[,] board;
        private Player playerBlack;
        private Player playerWhite;

        public GameBoard()
        {
            initGame();
        }

        public void initGame()
        {
            board = new int[8, 8];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = -1;
                }
            }

            board[3, 3] = 0;
            board[4, 3] = 1;
            board[3, 4] = 1;
            board[4, 4] = 0;

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
            throw new NotImplementedException();
        }

        public int GetWhiteScore()
        {
            return playerWhite.Score;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }
    }
}
