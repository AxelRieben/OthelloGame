using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Othello
{
    class Tile : Button
    {
        private Board board;

        private int posX;
        private int posY;
        private int tileState; //-1 = no one, 0 = Doge, 1 = Grumpy

        private Brush[] BRUSHES = { Brushes.Transparent, Constants.GetBrush(Constants.IMG_DOGE), Constants.GetBrush(Constants.IMG_GRUMPY) };

        public Tile(Board board, int x, int y, int state)
        {
            this.posX = x;
            this.posY = y;
            this.board = board;

            Grid.SetColumn(this, x);
            Grid.SetRow(this, y);

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
            if (board.IsPlayable(posX, posY, board.GetTurn()))
            {
                board.PlayMove(posX, posY, board.GetTurn());
                MessageBox.Show("Played");
            }
        }
    }
}
