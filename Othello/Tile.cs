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
    /// <summary>
    /// Class representing a button in the game grid
    /// </summary>
    class Tile : Button
    {
        private MainWindow mainWindow;
        private Board board;

        private int posX;
        private int posY;
        private int state; //-1 = no one, 0 = Doge, 1 = Grumpy
        private bool isPlayable;

        //Brushes
        private ImageBrush[] BRUSHES = { Constants.GetTansparentBrush(), Constants.GetBrush(Constants.IMG_DOGE), Constants.GetBrush(Constants.IMG_GRUMPY) };
        private ImageBrush[] BRUSHES_TRANSPARENT = { Constants.GetBrush(Constants.IMG_DOGE), Constants.GetBrush(Constants.IMG_GRUMPY) };

        public Tile(MainWindow parent, Board board, int x, int y, int state)
        {
            posX = x;
            posY = y;
            this.board = board;
            mainWindow = parent;

            //Set the opacity of the semi-transparent token
            BRUSHES_TRANSPARENT[0].Opacity = 0.5;
            BRUSHES_TRANSPARENT[1].Opacity = 0.5;

            Grid.SetColumn(this, x);
            Grid.SetRow(this, y);
            BorderBrush = Brushes.White;

            IsPlayable = false;
            State = state;
        }

        #region Property

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
                return state;
            }
            set
            {
                state = value;
                Background = BRUSHES[value + 1];
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

        #endregion

        #region Event

        /// <summary>
        /// Show a transparent token when the mouse is over and if the tile is playable
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (isPlayable)
            {
                if (board.GetTurn())
                {
                    Background = BRUSHES_TRANSPARENT[0];
                }
                else
                {
                    Background = BRUSHES_TRANSPARENT[1];
                }
            }
        }

        /// <summary>
        /// Remove the transparent token when the mouse leave the tile
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (isPlayable)
            {
                Background = BRUSHES[0];
            }
        }

        /// <summary>
        /// Play the movement when the tile is clicked
        /// </summary>
        protected override void OnClick()
        {
            if (isPlayable)
            {
                if (board.PlayMove(posX, posY, board.GetTurn()))
                {
                    mainWindow.UpdateGridValue();
                }
                else
                {
                    MessageBox.Show("Illegal movement !");
                }
            }
        }

        #endregion

    }
}
