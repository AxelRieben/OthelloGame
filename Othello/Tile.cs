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

        int owner; //-1 = no one, 0 = Doge, 1 = Grumpy
        int posX;
        int posY;

        bool isPlayable;

        public Tile(MainWindow parent, int x, int y)
        {
            this.posX = x;
            this.posY = y;
            this.mainWindow = parent;

            Grid.SetColumn(this, x);
            Grid.SetRow(this, y);

            owner = -1;
        }

        public int Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
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
    }
}
