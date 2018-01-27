using System;
using System.ComponentModel;
using System.Timers;

namespace Othello
{
    [Serializable]
    public class Player : INotifyPropertyChanged
    {
        private Board parent;
        private int color;
        private String name;
        private int score;
        private TimeSpan currentTime;
        private TimeSpan interval = new TimeSpan(0, 0, 1);
        [field: NonSerialized] private Timer timer;
        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Player(Board parent, String name, int color)
        {
            this.parent = parent;
            Name = name;
            Color = color;
            Score = 2;
            CurrentTime = new TimeSpan(0, Constants.TOTAL_TIME, 0);
            timer = new Timer(1000);
            timer.Elapsed += decrementTime;
        }

        public Player(Player sourcePlayer)
        {
            parent = sourcePlayer.parent;
            Name = sourcePlayer.Name;
            Color = sourcePlayer.Color;
            Score = sourcePlayer.Score;
            CurrentTime = sourcePlayer.CurrentTime;
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
                parent.PlayerTimeElapsed();
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
