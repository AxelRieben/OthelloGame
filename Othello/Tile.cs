using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Othello
{
    class Tile : Button
    {
        MainWindow mainWindow;

        enum TileState { empty = -1, Player1 = 0, Player2 = 1, Playable = 2 };

        //int owner; //-1 = no one, 0 = Doge, 1 = Grumpy
        int posX;
        int posY;
        TileState state;
        

        public Tile(MainWindow parent, int x, int y, int state)
        {
            this.posX = x;
            this.posY = y;
            this.mainWindow = parent;

            Grid.SetColumn(this, x);
            Grid.SetRow(this, y);

            this.state = (TileState)state;
        }

        public int State
        {
            get { return (int)state; }
            set
            {
                state = (TileState)value;
            }
        }
    }
}
