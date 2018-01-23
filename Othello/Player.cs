using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Othello
{
    public class Player : INotifyPropertyChanged
    {
        private int color;
        private String name;
        private int score;
        private int currentTime;
        private Timer timer;

        private const int TOTAL_TIME = 1800; //30 minutes

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Player(String name, int color)
        {
            this.name = name;
            this.color = color;
            Reset();
        }

        public void Reset()
        {
            Score = 2;
            CurrentTime = TOTAL_TIME;
            timer = new Timer(1000);
            timer.Elapsed += decrementTime;
        }


        public int CurrentTime
        {
            get
            {
                return currentTime;
            }

            set
            {
                currentTime = value;
                Notify("CurrentTime");
            }
        }

        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
                Notify("Score");
            }
        }

        public String Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                Notify("Name");
            }
        }

        public int Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                Notify("Color");
            }
        }

        private void decrementTime(Object source, ElapsedEventArgs e)
        {
            if (currentTime >= 1)
            {
                currentTime--;
            }
            else
            {
                timer.Stop();
            }
        }

        
        public void StartTimer()
        {
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }
    }
}
