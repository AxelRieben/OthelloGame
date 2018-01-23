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

        public MainWindow()
        {
            InitializeComponent();
            board = new Board();
            initGrid();
            UpdateGridValue();
            this.DataContext = board;
        }

        private void initGrid()
        {
            tiles = new Tile[Constants.GRID_SIZE, Constants.GRID_SIZE];

            for (int y = 0; y < Constants.GRID_SIZE; y++)
            {
                for (int x = 0; x < Constants.GRID_SIZE; x++)
                {
                    Tile tile = new Tile(this, board, x, y, -1);
                    tiles[x, y] = tile;
                    GridBoard.Children.Add(tile);
                }
            }
        }

        public void UpdateGridValue()
        {
            int tileValue = -1;
            int[,] gridBoard = board.GetBoard();

            for (int y = 0; y < Constants.GRID_SIZE; y++)
            {
                for (int x = 0; x < Constants.GRID_SIZE; x++)
                {
                    if (!board.IsPlayable(x, y, board.GetTurn()))
                    {
                        tileValue = gridBoard[x, y];
                    }
                    else
                    {
                        tileValue = -1;
                    }
                    tiles[x, y].State = tileValue;
                }
            }
        }

        private void ButtonNewGame(object sender, RoutedEventArgs e)
        {
            board.initGame();
            UpdateGridValue();
        }

        private void ButtonSave(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void ButtonLoad(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}
