using System;

namespace RPSLS.Framework
{
    public class ScoreHandler
    {
        private int _score;
        
        public static Action<int> OnScoreUpdate;

        public void ResetScore()
        {
            _score = 0;
        }
        
        public void AddScore(int amount)
        {
            _score += amount;
            OnScoreUpdate?.Invoke(_score);
        }

        public int GetScore()
        {
            return _score;
        }
    }
}