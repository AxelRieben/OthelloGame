using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Othello
{
    [Serializable]
    public class Board
    {
        private int[,] board;
        private Player playerBlack;
        private Player playerWhite;
        //starting with black
        bool isWhite;
        bool isPaused;

        public Board()
        {
            board = new int[Constants.GRID_SIZE, Constants.GRID_SIZE];
            playerWhite = new Player(this, "Doge", 0);
            playerBlack = new Player(this, "Grumpy Cat", 1);
            initGame();
        }

        public Board(int[,] board)
        {
            this.board = (int[,])board.Clone();
            playerWhite = new Player(this, "Doge", 0);
            playerBlack = new Player(this, "Grumpy Cat", 1);
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

        public Board(Board boardSource)
        {
            board = (int[,])boardSource.GetBoard().Clone();
            playerWhite = boardSource.PlayerWhite;
            playerBlack = boardSource.PlayerBlack;
            isWhite = boardSource.GetTurn();
        }

        #region Property

        public Player PlayerBlack
        {
            get
            {
                return playerBlack;
            }
            set
            {
                playerBlack = value;
            }
        }

        public Player PlayerWhite
        {
            get
            {
                return playerWhite;
            }
            set
            {
                playerWhite = value;
            }
        }

        public bool IsPaused
        {
            get { return isPaused; }
            set
            {
                isPaused = value;
                if (value)
                {
                    if (isWhite)
                    {
                        playerWhite.StopTimer();
                    }
                    else
                    {
                        playerBlack.StopTimer();
                    }
                }
                else
                {
                    if (isWhite)
                    {
                        playerWhite.StartTimer();
                    }
                    else
                    {
                        playerBlack.StartTimer();
                    }
                }
            }
        }

        #endregion

        #region Public Method

        public void initGame()
        {
            //set an empty board
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

            isWhite = false;
            isPaused = false;


            //create 2 player
            playerWhite.Reset(); // = new Player(this, "Doge", 0);
            playerBlack.Reset(); // = new Player(this, "Grumpy Cat", 1);
            playerBlack.StartTimer();
        }

        public bool Save(string filename, ref Board board)
        {
            Stream stream = null;

            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, board);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());  //TODO Remove
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return true;
        }

        public bool Load(string filename, ref Board board)
        {
            Stream stream = null;

            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                Board serializedBoard = (Board)formatter.Deserialize(stream);
                board = new Board(serializedBoard);
                board.PlayerBlack = new Player(serializedBoard.PlayerBlack);
                board.playerWhite = new Player(serializedBoard.PlayerWhite);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString()); //TODO Remove
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return true;
        }

        public void PlayerTimeElapsed()
        {
            string message = "";

            if (isWhite)
            {
                message += playerWhite.Name;
            }
            else
            {
                message += playerBlack.Name;
            }

            message += " has elapsed his time limit. ";

            if (isWhite)
            {
                message += playerBlack.Name;
            }
            else
            {
                message += playerWhite.Name;
            }

            message += " has won the game !";

            IsPaused = true;
            MessageBox.Show(message, "Game Over", MessageBoxButton.OK);
        }

        public int GetBlackScore()
        {
            return playerBlack.Score;
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public int GetWhiteScore()
        {
            return playerWhite.Score;
        }

        public bool GetTurn()
        {
            return isWhite;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return IsPlayableFlipOption(column, line, isWhite, false);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            if (IsPlayableFlipOption(column, line, isWhite, true))
            {
                SwitchTurn();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SwitchTurn()
        {
            if (isWhite)
            {
                playerWhite.StopTimer();
                playerBlack.StartTimer();
            }
            else
            {
                playerBlack.StopTimer();
                playerWhite.StartTimer();
            }

            this.isWhite = !this.isWhite;
        }


        #endregion

        #region Private Method

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

        #endregion
    }
}
