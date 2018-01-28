using System;
using System.ComponentModel;
using System.Timers;

namespace Othello
{
    /// <summary>
    /// Class representing one of the two player of the game
    /// </summary>
    [Serializable]
    public class Player : INotifyPropertyChanged
    {
        private Board parent;

        //Player specific attributes
        private int color;
        private String name;
        private int score;
        private TimeSpan currentTime;

        private TimeSpan interval = new TimeSpan(0, 0, 1);
        [field: NonSerialized] private Timer timer;
        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

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

        #region Property

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

        #endregion

        #region Method

        /// <summary>
        /// Decrement the player TimeSpan and stop the game if the time has elapsed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Reset the score and timer of the player
        /// </summary>
        public void Reset()
        {
            Score = 2;
            CurrentTime = new TimeSpan(0, Constants.TOTAL_TIME, 0);
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        public void StartTimer()
        {
            timer.Start();
        }

        /// <summary>
        /// Stop the timer
        /// </summary>
        public void StopTimer()
        {
            timer.Stop();
        }

        #endregion

        #region Event

        /// <summary>
        /// Notify the UI that changes in data binding has occured
        /// </summary>
        /// <param name="propertyName">Property name that has been modified</param>
        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
