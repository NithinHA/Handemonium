using System.Collections.Generic;
using RPSLS.Framework.Services;
using RPSLS.Player;
using RPSLS.UI;
using UnityEngine;

namespace RPSLS.Framework
{
    public class InGameController : Singleton<InGameController>
    {
        public GameRules GameRules;
        public float DecisionTimerInSeconds = 3f;
        [Space]
        [SerializeField] private GameObject m_Environment;
        [SerializeField] private PlayerRegistry _playerRegistry;

        public PlayerRegistry PlayerRegistry => _playerRegistry;
        
#region Round results

        private readonly List<RoundResult> _roundResults = new List<RoundResult>();
        public RoundResult CurrentRoundResult { get; private set; }

#endregion
        
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
            ServiceLocator.GetRoundManager().AddListener(OnRoundStateChanged);
            _scoreHandler = new ScoreHandler();
        }

        protected override void OnDestroy()
        {
            ServiceLocator.GetGameManager()?.RemoveListener(OnGameStateChanged);
            ServiceLocator.GetRoundManager()?.RemoveListener(OnRoundStateChanged);
            _scoreHandler = null;
            base.OnDestroy();
        }

#endregion

        public void Setup()
        {
            // setup environment
            m_Environment.SetActive(true);
            _scoreHandler.ResetScore();
            UIManager.Instance.InGamePanel.Show();
            PlayerRegistry.Setup();
            // can wait for sometime before switching round state; or assign the control to some button.
            
            ServiceLocator.GetRoundManager().SwitchState(RoundState.Start);     // responsible for starting round timer.
        }

        public void Reset()
        {
            // reset environment
            m_Environment.SetActive(false);
            _scoreHandler.ResetScore();
            UIManager.Instance.InGamePanel.Hide();
            PlayerRegistry.Reset();
            _roundResults.Clear();
        }

        public void OnTimerComplete()
        {
            ServiceLocator.GetRoundManager().SwitchState(RoundState.End);
        }

        public void ComputeResult()
        {
            CurrentRoundResult = PlayerRegistry.GetRoundResult();
            _roundResults.Add(CurrentRoundResult);
            ServiceLocator.GetRoundManager().SwitchState(RoundState.Result);
        }

        public async void OnPostRoundEndContinue()
        {
            await PlayerRegistry.HidePlayerHands();
            PlayerRegistry.Reset();

            switch (CurrentRoundResult)
            {
                case RoundResult.None:
                case RoundResult.Win:
                case RoundResult.Draw:
                    ServiceLocator.GetRoundManager().SwitchState(RoundState.Start);
                    break;
                case RoundResult.Lose:
                    ServiceLocator.GetRoundManager().SwitchState(RoundState.Idle);
                    var highscoreService = ServiceLocator.GetHighscoreService();
                    if (highscoreService.GetHighscore() < GetCurrentScore)
                    {
                        highscoreService.SetHighscore(GetCurrentScore);
                    }
                    ServiceLocator.GetGameManager().SwitchState(GameState.MainMenu);
                    break;
            }
        }

#region Event listeners

        private void OnGameStateChanged(GameState prevState, GameState curState)
        {
            if (prevState == GameState.InGame && curState != GameState.InGame)
            {
                Reset();
            }
            else if (curState == GameState.InGame)
            {
                Setup();
            }
        }
        
        private void OnRoundStateChanged(RoundState prevState, RoundState curState)
        {
            switch (curState)
            {
                case RoundState.Start:
                    CurrentRoundResult = RoundResult.None;
                    break;
                case RoundState.Result:
                    switch (CurrentRoundResult)
                    {
                        case RoundResult.Win:
                            IncrementScore();
                            break;
                        case RoundResult.Draw:
                            break;
                        case RoundResult.Lose:
                            break;
                    }
                    break;
            }
        }

#endregion
    }
}