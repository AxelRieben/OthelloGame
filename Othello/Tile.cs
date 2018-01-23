using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Othello
{
    class Tile : Button
    {
        private MainWindow parent;
        private Board board;

        private int posX;
        private int posY;
        private int tileState; //-1 = no one, 0 = Doge, 1 = Grumpy

        private Brush[] BRUSHES = { Brushes.Transparent, Constants.GetBrush(Constants.IMG_DOGE), Constants.GetBrush(Constants.IMG_GRUMPY) };

        public Tile(MainWindow parent, Board board, int x, int y, int state)
        {
            this.posX = x;
            this.posY = y;
            this.board = board;
            this.parent = parent;

            Grid.SetColumn(this, x);
            Grid.SetRow(this, y);
            BorderBrush = Brushes.White;

            this.tileState = state;
        }

        public int State
        {
            get { return tileState; }
            set
            {
                tileState = value;
                this.Background = BRUSHES[value + 1];
            }
        }

        protected override void OnClick()
        {
            bool isWhite = board.GetTurn();
            //MessageBox.Show(RuntimeHelpers.GetHashCode(board).ToString());

            if (board.IsPlayable(posX, posY, isWhite))
            {
                if(board.PlayMove(posX, posY, isWhite)){
                    parent.UpdateGridValue();
                }
                else
                {
                    MessageBox.Show("Illegal movment");
                }
            }
        }
    }
}
