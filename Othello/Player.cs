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
        private TimeSpan currentTime;
        private Timer timer;

        private const int TOTAL_TIME = 30; //30 minutes
        private TimeSpan interval = new TimeSpan(0, 0, 1);

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
            CurrentTime = new TimeSpan(0, TOTAL_TIME, 0);
            timer = new Timer(1000);
            timer.Elapsed += decrementTime;
        }


        public TimeSpan CurrentTime
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
            if (currentTime.TotalSeconds >= 1)
            {
                CurrentTime = currentTime.Subtract(interval);
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
