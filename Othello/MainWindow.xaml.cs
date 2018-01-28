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

        public void UpdateGridValue()
        {
            int[,] gridBoard = board.GetBoard();
            int numPlayableTiles = 0;
            int totalPlayableTiles = 0;

            for (int y = 0; y < Constants.GRID_SIZE; y++)
            {
                for (int x = 0; x < Constants.GRID_SIZE; x++)
                {
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
                        totalPlayableTiles++;
                    }

                    tiles[x, y].State = gridBoard[x, y];
                }
            }

            checkGameEnd(numPlayableTiles, totalPlayableTiles);
        }

        #endregion

        #region private Method

        private void resetMainWindows()
        {
            board.initGame();
            UpdateTilesReference();
            UpdateGridValue();
            this.DataContext = board;
            numConsecutiveSkip = 0;
        }

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

        private void checkGameEnd(int numPlayableTiles, int totalPlayableTiles)
        {
            if (totalPlayableTiles == 0 || numConsecutiveSkip == 2)
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

        private void UpdateTilesReference()
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

        #endregion

        #region Event
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

        private void ButtonNewGameClick(object sender, RoutedEventArgs e)
        {
            board.IsPaused = true;

            MessageBoxResult result = MessageBox.Show("The current game will be lost, are you sure you want to start a new game ?", "New Game", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                resetMainWindows();
            }
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            board.IsPaused = true;

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

        private void ButtonLoadClick(object sender, RoutedEventArgs e)
        {
            board.IsPaused = true;

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
                    this.DataContext = board;
                    UpdateTilesReference();
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

        private void ButtonAboutClick(object sender, RoutedEventArgs e)
        {
            board.IsPaused = true;
            MessageBox.Show("This game has been developped by Axel Rieben & Killian Castella at the High School Arc.", "Ohtello Doge vs Grumpy Cat", MessageBoxButton.OK);
            board.IsPaused = false;
        }

        #endregion
    }
}
