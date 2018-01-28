using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello
{
    /// <summary>
    /// Class representing the UI
    /// </summary>
    public partial class MainWindow : Window
    {
        private Tile[,] tiles;
        private Board board;
        private int numConsecutiveSkip;

        public MainWindow()
        {
            InitializeComponent();
            board = new Board();
            initGrid();
            resetMainWindows();
        }

        #region Public Method

        /// <summary>
        /// Refresh the button of the grid according to the board state
        /// </summary>
        public void UpdateGridValue()
        {
            int[,] gridBoard = board.GetBoard();
            int numPlayableTiles = 0;
            int numEmptyTiles = 0;

            for (int y = 0; y < Constants.GRID_SIZE; y++)
            {
                for (int x = 0; x < Constants.GRID_SIZE; x++)
                {
                    //Set the playable tiles
                    if (board.IsPlayable(x, y, board.GetTurn()))
                    {
                        tiles[x, y].IsPlayable = true;
                        numPlayableTiles++;
                    }
                    else
                    {
                        tiles[x, y].IsPlayable = false;
                    }

                    if (gridBoard[x, y] == -1)
                    {
                        numEmptyTiles++;
                    }

                    tiles[x, y].State = gridBoard[x, y];
                }
            }

            updateImagePlayer();
            checkGameEnd(numPlayableTiles, numEmptyTiles);
        }

        #endregion

        #region private Method

        /// <summary>
        /// Reset the game
        /// </summary>
        private void resetMainWindows()
        {
            board.InitGame();
            updateTilesReference();
            UpdateGridValue();
            DataContext = board;
            numConsecutiveSkip = 0;
        }

        /// <summary>
        /// Create the buttons in the grid and store them in an array
        /// </summary>
        private void initGrid()
        {
            Style style = (Style)FindResource("GridButton");
            tiles = new Tile[Constants.GRID_SIZE, Constants.GRID_SIZE];

            for (int y = 0; y < Constants.GRID_SIZE; y++)
            {
                for (int x = 0; x < Constants.GRID_SIZE; x++)
                {
                    Tile tile = new Tile(this, board, x, y, -1);
                    tiles[x, y] = tile;
                    tile.Style = style;
                    GridBoard.Children.Add(tile);
                }
            }
        }

        /// <summary>
        /// Check if the game is over
        /// </summary>
        /// <param name="numPlayableTiles"></param>
        /// <param name="numEmptyTiles"></param>
        private void checkGameEnd(int numPlayableTiles, int numEmptyTiles)
        {
            //The game is over if the board is full or if the two player have skiped
            if (numEmptyTiles == 0 || numConsecutiveSkip == 2)
            {
                string message = "";

                if (board.PlayerWhite.Score > board.PlayerBlack.Score)
                {
                    message += board.PlayerWhite.Name;
                    message += " has won the game with ";
                    message += board.PlayerWhite.Score + " points against " + board.PlayerBlack.Score + " !";
                }
                else if (board.PlayerWhite.Score == board.PlayerBlack.Score)
                {
                    message = "The game ended with an equality !";
                }
                else
                {
                    message += board.PlayerBlack.Name;
                    message += " has won the game with ";
                    message += board.PlayerBlack.Score + " points against " + board.PlayerWhite.Score + " !";
                }

                board.IsPaused = true;
                MessageBox.Show(message, "Game Over", MessageBoxButton.OK);
            }
            else
            {
                //If a player has no playable tiles he needs to press the pass button
                if (numPlayableTiles == 0)
                {
                    MessageBox.Show("You can't play, you need to pass", "Pass");
                    numConsecutiveSkip++;
                    if (board.GetTurn())
                    {
                        ButtonPassWhite.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ButtonPassBlack.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    numConsecutiveSkip = 0;
                }
            }
        }

        /// <summary>
        /// Update the tiles board reference when a game has been loaded
        /// </summary>
        private void updateTilesReference()
        {
            int[,] gridBoard = board.GetBoard();

            for (int y = 0; y < Constants.GRID_SIZE; y++)
            {
                for (int x = 0; x < Constants.GRID_SIZE; x++)
                {
                    tiles[x, y].Board = board;
                }
            }
        }

        /// <summary>
        /// Update the images of the players on the UI
        /// </summary>
        private void updateImagePlayer()
        {
            //Transparency
            if (board.GetTurn())
            {
                ImageBlack.Opacity = 0.5;
                ImageWhite.Opacity = 1;
            }
            else
            {
                ImageWhite.Opacity = 0.5;
                ImageBlack.Opacity = 1;
            }

            //Crown
            if (board.GetWhiteScore() > board.GetBlackScore())
            {
                ImageCrownWhite.Visibility = Visibility.Visible;
                ImageCrownBlack.Visibility = Visibility.Hidden;
            }
            else if (board.GetWhiteScore() < board.GetBlackScore())
            {
                ImageCrownWhite.Visibility = Visibility.Hidden;
                ImageCrownBlack.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// Handle click on the pass button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPassClick(object sender, RoutedEventArgs e)
        {
            if (board.GetTurn())
            {
                ButtonPassWhite.Visibility = Visibility.Hidden;
            }
            else
            {
                ButtonPassBlack.Visibility = Visibility.Hidden;
            }

            board.SwitchTurn();
            UpdateGridValue();
        }

        /// <summary>
        /// Handle the click on the new game button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNewGameClick(object sender, RoutedEventArgs e)
        {
            board.IsPaused = true;

            MessageBoxResult result = MessageBox.Show("The current game will be lost, are you sure you want to start a new game ?", "New Game", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                resetMainWindows();
            }
        }

        /// <summary>
        /// Handle the click on the save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            board.IsPaused = true;

            //Open a file dialog
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Save the game";
            saveDialog.FileName = "game";
            saveDialog.DefaultExt = ".otlo";
            saveDialog.Filter = "Othello files (*.otlo)|*.otlo";
            saveDialog.FilterIndex = 2;
            bool? result = saveDialog.ShowDialog();

            if (result == true && saveDialog.FileName != "")
            {
                string filename = saveDialog.FileName;
                if (board.Save(filename, ref board))
                {
                    MessageBox.Show("Game has been saved in " + filename + " !", "Saved", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Game couldn't be saved !", "Sorry", MessageBoxButton.OK);
                }
            }

            board.IsPaused = false;
        }

        /// <summary>
        /// Handle the click on the load button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLoadClick(object sender, RoutedEventArgs e)
        {
            board.IsPaused = true;

            //Open a file dialog
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Load a game";
            openDialog.FileName = "game.othlo";
            openDialog.Filter = "Othello files (*.otlo)|*.otlo";
            openDialog.FilterIndex = 2;
            bool? result = openDialog.ShowDialog();

            if (result == true && openDialog.FileName != "")
            {
                string filename = openDialog.FileName;
                if (board.Load(filename, ref board))
                {
                    DataContext = board;
                    updateTilesReference();
                    UpdateGridValue();
                    MessageBox.Show("Game has been load !", "Loaded", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Game couldn't be load !", "Sorry", MessageBoxButton.OK);

                }
            }

            board.IsPaused = false;
        }

        /// <summary>
        /// Handle the click on the about button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAboutClick(object sender, RoutedEventArgs e)
        {
            board.IsPaused = true;
            MessageBox.Show("This game has been developped by Axel Rieben & Killian Castella at the High School Arc.", "About", MessageBoxButton.OK);
            board.IsPaused = false;
        }

        #endregion
    }
}
