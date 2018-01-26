using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private bool isPlayable;

        private ImageBrush[] BRUSHES = { Constants.GetTansparentBrush(), Constants.GetBrush(Constants.IMG_DOGE), Constants.GetBrush(Constants.IMG_GRUMPY) };
        private ImageBrush[] BRUSHES_TRANSPARENT = { Constants.GetBrush(Constants.IMG_DOGE), Constants.GetBrush(Constants.IMG_GRUMPY) };

        public Tile(MainWindow parent, Board board, int x, int y, int state)
        {
            this.posX = x;
            this.posY = y;
            this.board = board;
            this.parent = parent;

            BRUSHES_TRANSPARENT[0].Opacity = 0.5;
            BRUSHES_TRANSPARENT[1].Opacity = 0.5;

            Grid.SetColumn(this, x);
            Grid.SetRow(this, y);
            BorderBrush = Brushes.White;

            isPlayable = false;
            this.tileState = state;
        }

        public bool IsPlayable
        {
            get
            {
                return isPlayable;
            }
            set
            {
                isPlayable = value;
            }
        }

        public int State
        {
            get
            {
                return tileState;
            }
            set
            {
                tileState = value;
                this.Background = BRUSHES[value + 1];
            }
        }

        public Board Board
        {
            get
            {
                return board;
            }
            set
            {
                board = value;
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (isPlayable)
            {
                if (board.GetTurn())
                {
                    this.Background = BRUSHES_TRANSPARENT[0];
                }
                else
                {
                    this.Background = BRUSHES_TRANSPARENT[1];
                }
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (isPlayable)
            {
                this.Background = BRUSHES[0];
            }
        }

        protected override void OnClick()
        {
            if (isPlayable)
            {
                if (board.PlayMove(posX, posY, board.GetTurn()))
                {
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
