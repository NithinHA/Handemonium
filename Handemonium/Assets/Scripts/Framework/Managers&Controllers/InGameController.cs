using System.Collections.Generic;
using RPSLS.Framework.Services;
using RPSLS.Game;
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
        [SerializeField] private InfoBoard m_InfoBoard;
        [SerializeField] private PlayerRegistry m_PlayerRegistry;
        
        public BasePlayer GetPlayerSelf => m_PlayerRegistry.PlayerSelf;
        public BasePlayer GetPlayerOpponent => m_PlayerRegistry.PlayerOpponent;
        public InfoBoard GetInfoBoard => m_InfoBoard;
        
#region Round results

        private readonly List<RoundResultDesc> _roundResults = new List<RoundResultDesc>();
        public RoundResultDesc CurrentRoundResult { get; private set; }

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
            m_Environment.SetActive(true);
            _scoreHandler.ResetScore();
            UIManager.Instance.InGamePanel.Show();
            m_PlayerRegistry.Setup();
        }

        public void Reset()
        {
            m_Environment.SetActive(false);
            _scoreHandler.ResetScore();
            UIManager.Instance.InGamePanel.Hide();
            m_PlayerRegistry.Reset();
            _roundResults.Clear();
        }

        public void ClickBeginRound()
        {
            ServiceLocator.GetRoundManager().SwitchState(RoundState.Start);     // will start the round timer.
        }

        public void OnTimerComplete()
        {
            ServiceLocator.GetRoundManager().SwitchState(RoundState.End);
        }

        public void ComputeResult()
        {
            CurrentRoundResult = m_PlayerRegistry.GetRoundResult();
            _roundResults.Add(CurrentRoundResult);
            ServiceLocator.GetRoundManager().SwitchState(RoundState.Result);
        }

        public async void OnPostRoundEndContinue()
        {
            await m_PlayerRegistry.HidePlayerHands();
            m_PlayerRegistry.Reset();

            switch (CurrentRoundResult.Result)
            {
                case RoundResultState.None:
                case RoundResultState.Win:
                case RoundResultState.Draw:
                    ServiceLocator.GetRoundManager().SwitchState(RoundState.Start);
                    break;
                case RoundResultState.Lose:
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
                    CurrentRoundResult = new RoundResultDesc();
                    m_InfoBoard.Reset();
                    break;
                case RoundState.Result:
                    switch (CurrentRoundResult.Result)
                    {
                        case RoundResultState.Win:
                            IncrementScore();
                            break;
                        case RoundResultState.Draw:
                            break;
                        case RoundResultState.Lose:
                            break;
                    }
                    break;
            }
        }

#endregion

    }
}