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

        public void SetRoundResult(RoundResultDesc result)
        {
            CurrentRoundResult = result;
            _roundResults.Add(CurrentRoundResult);
            ServiceLocator.GetRoundManager().SwitchState(RoundState.Result);
        }

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

        public void Setup(Dictionary<object, object> payload)
        {
            m_Environment.SetActive(true);
            _scoreHandler.ResetScore();
            UIManager.Instance.InGamePanel.Show();
            
            // If Local (not networked) or Host, we might do setup
            // Usually PlayerRegistry.Setup() is empty anyway
            if (payload != null && payload.TryGetValue(Constants.EventConstants.GAME_MODE, out var gameMode))
                m_PlayerRegistry.Setup(gameMode as string);

            // If Networking, maybe auto-start round or wait for server?
            // For now, let the standard flow happen.
        }

        public void Reset()
        {
            m_Environment.SetActive(false);
            _scoreHandler.ResetScore();
            UIManager.Instance.InGamePanel.Hide();
            m_PlayerRegistry.Reset(true);
            _roundResults.Clear();
        }

        public void ClickBeginRound()
        {
            // If in multiplayer, signal ready to server
            if (Unity.Netcode.NetworkManager.Singleton != null && 
                Unity.Netcode.NetworkManager.Singleton.IsListening)
            {
                var controller = RPSLS.Framework.Controllers.NetworkGameController.Instance;
                if (controller != null)
                {
                    controller.SetPlayerReadyServerRpc();
                    return; // Server will start round when both ready
                }
            }
            
            // Local game flow
            ServiceLocator.GetRoundManager().SwitchState(RoundState.Start);
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

        public void OnPostRoundEndContinue()
        {
            // In multiplayer, signal ready to server and wait for both players
            if (Unity.Netcode.NetworkManager.Singleton != null && 
                Unity.Netcode.NetworkManager.Singleton.IsListening)
            {
                var controller = RPSLS.Framework.Controllers.NetworkGameController.Instance;
                if (controller != null)
                {
                    controller.SetPlayerContinueReadyServerRpc();
                    return; // Server will call ContinueToNextRoundClientRpc when both ready
                }
            }

            // Single-player: execute immediately
            ContinueToNextRoundLocal();
        }

        /// <summary>
        /// Called by NetworkGameController when both players are ready to continue (multiplayer only)
        /// </summary>
        public async void ContinueToNextRoundMultiplayer()
        {
            await m_PlayerRegistry.HidePlayerHands();
            m_PlayerRegistry.Reset(false);
            
            // In multiplayer, always continue to next round (no Win/Lose branching)
            ServiceLocator.GetRoundManager().SwitchState(RoundState.Start);
        }

        /// <summary>
        /// Single-player continuation with Win/Lose logic
        /// </summary>
        public async void ContinueToNextRoundLocal()
        {
            await m_PlayerRegistry.HidePlayerHands();
            m_PlayerRegistry.Reset(false);

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

        public async void ExitOnBackClick()
        {
            var roundState = ServiceLocator.GetRoundManager().RoundState;
            if (roundState == RoundState.Result || roundState == RoundState.End)
            {
                // If hands are showing, hide them first
                await m_PlayerRegistry.HidePlayerHands();
            }

            // Reset game state
            Reset();
            ServiceLocator.GetRoundManager().SwitchState(RoundState.Idle);
            
            // Disconnect from multiplayer if connected
            if (Unity.Netcode.NetworkManager.Singleton != null && 
                Unity.Netcode.NetworkManager.Singleton.IsListening)
            {
                Unity.Netcode.NetworkManager.Singleton.Shutdown();
            }

            // Return to main menu
            ServiceLocator.GetGameManager().SwitchState(GameState.MainMenu);
        }

#region Event listeners

        private void OnGameStateChanged(GameState prevState, GameState curState, Dictionary<object, object> payload = null)
        {
            if (prevState == GameState.InGame && curState != GameState.InGame)
            {
                Reset();
            }
            else if (curState == GameState.InGame)
            {
                Setup(payload);
            }
        }
        
        private void OnRoundStateChanged(RoundState prevState, RoundState curState)
        {
            switch (curState)
            {
                case RoundState.Idle:
                case RoundState.Start:
                    CurrentRoundResult = new RoundResultDesc();
                    m_InfoBoard.Reset();
                    break;
                case RoundState.Result:
                    switch (CurrentRoundResult.Result)
                    {
                        case RoundResultState.Win:
                            IncrementScore();
                            AudioManager.Instance?.PlaySound(Constants.Audio.WIN);
                            break;
                        case RoundResultState.Draw:
                            AudioManager.Instance?.PlaySound(Constants.Audio.DRAW);
                            break;
                        case RoundResultState.Lose:
                            AudioManager.Instance?.PlaySound(Constants.Audio.LOSE);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Called when the back button is clicked in the in-game panel
        /// </summary>
        public void OnBackButtonClick()
        {
            // In multiplayer, notify server and other clients about disconnect
            if (Unity.Netcode.NetworkManager.Singleton != null && 
                Unity.Netcode.NetworkManager.Singleton.IsListening)
            {
                var controller = RPSLS.Framework.Controllers.NetworkGameController.Instance;
                if (controller != null)
                {
                    // Notify other clients that this player is leaving
                    controller.NotifyPlayerLeavingServerRpc();
                }
            }
            else
            {
                ExitOnBackClick();
            }
        }

#endregion

    }
}