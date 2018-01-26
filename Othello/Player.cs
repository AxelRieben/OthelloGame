using System;
using System.ComponentModel;
using System.Timers;

namespace Othello
{
    [Serializable]
    public class Player : INotifyPropertyChanged
    {
        private int color;
        private String name;
        private int score;
        private TimeSpan currentTime;
        [field: NonSerialized] private Timer timer;

        private const int TOTAL_TIME = 30; //30 minutes
        private TimeSpan interval = new TimeSpan(0, 0, 1);

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Player(String name, int color)
        {
            Name = name;
            Color = color;
            Reset();
        }

        public Player(Player sourcePlayer)
        {
            Name = sourcePlayer.Name;
            Color = sourcePlayer.Color;
            Score = sourcePlayer.Score;
            CurrentTime = sourcePlayer.CurrentTime;
            timer = new Timer(1000);
            timer.Elapsed += decrementTime;
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
