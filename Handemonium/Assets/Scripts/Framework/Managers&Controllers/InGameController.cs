
using RPSLS.Framework.Services;
using RPSLS.Game;
using RPSLS.UI;

namespace RPSLS.Framework
{
    public class InGameController : Singleton<InGameController>
    {
        public GameRules GameRules;

        public BasePlayer Player1;
        public BasePlayer Player2;
        
#region Current score

        private ScoreHandler _scoreHandler;
        public int GetCurrentScore => _scoreHandler.GetScore();
        public void IncrementScore() => _scoreHandler.AddScore(1);

#endregion

#region Unity callbacks
        
        protected override void Start()
        {
            base.Start();
            ServiceLocator.GetGameManager().AddListener(OnGameStateChanged);
            _scoreHandler = new ScoreHandler();
        }

        protected override void OnDestroy()
        {
            ServiceLocator.GetGameManager()?.RemoveListener(OnGameStateChanged);
            base.OnDestroy();
        }

#endregion

        public void Setup()
        {
            // setup environment
            _scoreHandler.ResetScore();
            UIManager.Instance.InGamePanel.Show();
        }

        public void Reset()
        {
            // reset environment
            _scoreHandler.ResetScore();
            UIManager.Instance.InGamePanel.Hide();
        }

#region Event listeners

        private void OnGameStateChanged(GameState prevState, GameState curState)
        {
            if (curState == GameState.InGame)
            {
                Setup();
            }
        }

#endregion
    }
}