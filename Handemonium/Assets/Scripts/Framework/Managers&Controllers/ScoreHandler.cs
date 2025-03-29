namespace RPSLS.Framework
{
    public class ScoreHandler
    {
        private int _score;

        public void ResetScore()
        {
            _score = 0;
        }
        
        public void AddScore(int amount)
        {
            _score += amount;
        }

        public int GetScore()
        {
            return _score;
        }
    }
}