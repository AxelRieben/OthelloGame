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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DrawGrid();
        }
        private void DrawGrid()
        {

            double squareHeight = canvas.Height / 8;
            double squareWidth = canvas.Width / 8;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Stroke = Brushes.LightBlue,
                        StrokeThickness = 1
                    };
                    rect.Height = squareHeight;
                    rect.Width = squareWidth;
                    Canvas.SetTop(rect, 0 + i * squareHeight);
                    Canvas.SetLeft(rect, 0 + j * squareWidth);
                    canvas.Children.Add(rect);
                    /*enum background texture(0:claire 1:foncé)?*/
                }
            }
            Rectangle borderRect = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            borderRect.Height = canvas.Height;
            borderRect.Width = canvas.Width;
            Canvas.SetTop(borderRect, 0);
            Canvas.SetLeft(borderRect, 0);
            canvas.Children.Add(borderRect);
        }
    }
}
