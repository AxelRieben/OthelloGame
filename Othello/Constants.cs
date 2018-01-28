using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Othello
{
    /// <summary>
    /// Contains constants values of the game
    /// </summary>
    class Constants
    {
        //Grid
        public static int GRID_SIZE = 8;

        //Player time
        public static int TOTAL_TIME = 30; //Official 30 minutes

        //Images
        public static String IMG_FOLDER = "images/";
        public static String IMG_DOGE = IMG_FOLDER + "doge.png";
        public static String IMG_GRUMPY = IMG_FOLDER + "grumpy.png";

        public static ImageBrush GetBrush(String imagePath)
        {
            Uri uriImage = new Uri(imagePath, UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(uriImage);
            BitmapFrame bitmap = BitmapFrame.Create(streamInfo.Stream);

            ImageBrush brush = new ImageBrush();
            brush.Stretch = Stretch.Uniform;
            brush.ImageSource = bitmap;

            return brush;
        }

        public static ImageBrush GetTansparentBrush()
        {
            ImageBrush brush = new ImageBrush();
            brush.Opacity = 0;
            return brush;
        }
    }
}
